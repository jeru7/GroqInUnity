using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Responsible for handling AI responses from Groq API.
/// </summary>
public class GroqManager : MonoBehaviour
{
    [SerializeField]
    private string _apiKey;
    private string _groqUrl;

    private void Start()
    {
        _apiKey = "YOUR API KEY"; // DO NOT HARDCODE YOUR API KEY
        _groqUrl = "https://api.groq.com/openai/v1/chat/completions"; // check groq's documentation, this endpoint is for Groq's chat completion.
    }

    /// <summary>
    /// Responsible for handling AI responses from Groq API.
    /// </summary>
    public void ProcessMessage(string userMessage)
    {
        StartCoroutine(SendRequest(userMessage));
    }

    /// <summary>
    /// Sends the user message to Groq API and processes its response.
    /// </summary>
    private IEnumerator SendRequest(string userMessage)
    {
        if (string.IsNullOrEmpty(_apiKey))
        {
            Debug.LogError("API Key is missing!");
            yield break;
        }

        GroqRequest requestPayload =
            new GroqRequest("llama-3.3-70b-versatile", // model
                            "system prompt", userMessage);

        string jsonData = JsonConvert.SerializeObject(requestPayload);
        byte[] postData = Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest request = new UnityWebRequest(_groqUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(postData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + _apiKey);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseText = request.downloadHandler.text;

            try
            {
                GroqResponse jsonResponse =
                    JsonConvert.DeserializeObject<GroqResponse>(responseText);
                string aiMessage = jsonResponse?.choices?[0]?.message?.content;

                if (!string.IsNullOrEmpty(aiMessage))
                {
                    Debug.Log("AI response: " + aiMessage); // you can create a method here to show the response on your unity app
                }
                else
                {
                    Debug.Log("AI message is empty.");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error in processing AI response.");
            }
        }
        else
        {
            string errorResponseText = request.downloadHandler.text;
            Debug.LogError("Error response JSON: " + errorResponseText);

            try
            {
                GroqErrorResponse errorResponse =
                    JsonConvert.DeserializeObject<GroqErrorResponse>(errorResponseText);
                if (errorResponse?.error != null)
                {
                    Debug.LogError(
                        $"API error: {errorResponse.error.message} (Type: {errorResponse.error.type}, Code: {errorResponse.error.code})");
                }
                else
                {
                    Debug.LogError("An API error occured.");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("JSON parsing error for error response: " + ex.Message);
            }
        }
    }
}

// Helper Class Definitions

[System.Serializable]
public class GroqResponse
{
    public string id { get; set; }
    public string @object { get; set; }
    public long created { get; set; }
    public string model { get; set; }
    public List<Choice> choices { get; set; }
    public Usage usage { get; set; }
}

[System.Serializable]
public class Choice
{
    public int index { get; set; }
    public Message message { get; set; }
    public string finish_reason { get; set; }
}

[System.Serializable]
public class Message
{
    public string role { get; set; }
    public string content { get; set; }
}

[System.Serializable]
public class Usage
{
    public int total_tokens { get; set; }
}

[System.Serializable]
public class GroqRequest
{
    public string model { get; set; }
    public List<Message> messages { get; set; }

    public GroqRequest(string modelName, string systemContent,
                       string userMessage)
    {
        model = modelName;
        messages = new List<Message> {
      new Message { role = "system", content = systemContent },
      new Message { role = "user", content = userMessage },
    };
    }
}

[System.Serializable]
public class GroqErrorResponse
{
    public GroqError error { get; set; }
}

[System.Serializable]
public class GroqError
{
    public string message { get; set; }
    public string type { get; set; }
    public string code { get; set; }
}
