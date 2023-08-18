using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ChatGPTLessonManager : MonoBehaviour
{
    [SerializeField]
    private TTSResponseBehaviour responseScript;
    void Start()
    {
        
    }

    IEnumerator CallChatGPT()
    {
        // Build the request object.
        var request = new Request();
        request.model = "gpt-3.5-turbo";

        // System message to describe the type of responses.
        var systemMessage = new Message();
        systemMessage.role = "system";
        systemMessage.content =
        @"Speak disdainfully.";
        request.messages[0] = systemMessage;

        // User message from the user.
        var userMessage = new Message();
        userMessage.role = "user";
        userMessage.content = "Provide a short lecture on one random topic from the physics school syllabus to the student.";
        request.messages[1] = userMessage;

        // Convert the obect to json.
        string json = JsonUtility.ToJson(request);

        using (UnityWebRequest www = UnityWebRequest.Post(
            "https://api.openai.com/v1/chat/completions",
            json,
            "application/json"))
        {
            www.SetRequestHeader("Authorization", "Bearer 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");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Response responseObject = JsonUtility.FromJson<Response>(www.downloadHandler.text);

                // Speak answer.
                //  // Speak answer.                
                responseScript.Response(responseObject.choices[0].message.content);                
            }
        }
    }

    public void ProvideLecture()
    {
        StartCoroutine(CallChatGPT());
    }
   

    // Chat Completions.
    // Creating classes to send the POST request to the OpenAI API.
    [System.Serializable]
    public class Request
    {
        public string model;
        public Message[] messages = new Message[2];
    }

    [System.Serializable]
    public class Message
    {
        public string role;
        public string content;
    }

    // Creating classes to receive responses from the OpenAI API.
    [System.Serializable]
    public class Response
    {
        public string id;
        public string objectName;
        public int created;
        public string model;
        public Choice[] choices;
        public Usage usage;
    }

    [System.Serializable]
    public class Choice
    {
        public int index;
        public Message message;
        public string finish_reason;
    }

    [System.Serializable]
    public class Usage
    {
        public int prompt_tokens;
        public int completion_tokens;
        public int total_tokens;
    }
}
