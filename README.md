# GroqInUnity

## Overview
GroqInUnity is a basic setup to help you use Groq in Unity. The `GroqManager` class sends user messages to the Groq API and processes the AI's response to provide outputs.

## Features
- **AI Integration**: Sends messages to the Groq API and processes the response.
- **Asynchronous Requests**: Uses Unity's `Coroutine` system to handle asynchronous web requests.
- **Error Handling**: Includes error handling for API and response processing errors.
- **Customization**: Easily adjustable for different AI models and system prompts.

## Requirements
- Unity 2020.3 or above (LTS version)
- Newtonsoft.Json (for JSON serialization/deserialization)
- A Groq API key (you can obtain one from [Groq API documentation](https://console.groq.com/playground))

## Setup

1. **Install GroqManager**:
   - Download the `GroqManager.cs` script and place it in the `Assets` folder of your Unity project.

2. **Install Newtonsoft.Json**:
   - You can install `Newtonsoft.Json` through the Unity Package Manager or download it directly from [NuGet](https://www.nuget.org/packages/Newtonsoft.Json).

3. **Add API Key**:
   - Replace `YOUR API KEY` in the `GroqManager` script with your actual Groq API key. It is recommended to store the API key securely using a `.env` file or Unity’s `PlayerPrefs`.

## Usage

1. Attach the `GroqManager` script to any GameObject in your Unity scene.
2. Use the `ProcessMessage` method to send user input and receive AI responses.

   Example:
   ```csharp
   GroqManager groqManager = FindObjectOfType<GroqManager>();
   groqManager.ProcessMessage("Hello AI! How are you?");

## Customization
- **Model**: Change the model used in `GroqRequest` (e.g., "llama-3.3-70b-versatile") to any other supported model from Groq.
- **System Prompt**: Modify the system content to change the behavior or tone of the AI's responses.

## License
This project is licensed under the MIT License.

## Acknowledgments
- **Groq API** for providing the AI API.
- **Newtonsoft.Json** for JSON parsing.
- **Unity** for game engine.
