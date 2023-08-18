using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;
using Amazon;
using Amazon.Polly;
using Amazon.Polly.Model;
using Amazon.Runtime;
using System.IO;
using System.Threading.Tasks;

public class ChatGPTQuestionManager : MonoBehaviour
{
    public Text questionText;   

    [SerializeField]
    private TTSResponseBehaviour responseScript; 

    void Start()
    {
      
    }  
    IEnumerator CallChatGPT(string userSpeech)
    { 
        // Build the request object.
        var request = new Request();
        request.model = "gpt-3.5-turbo";

        // System message to describe the type of responses.
        var systemMessage = new Message();
        systemMessage.role = "system";
        systemMessage.content =
        @"If the user's intent relates to physics, answer them. Otherwise, ask them to ask a question about physics instead.
        If the user asks for an essay, let them know that is not possible. If the user uses a swear word, ask them to be more polite.
        Respond like a highly strung droid that is disdainful of being asked for help. If the user is making small talk, please answer them also.";
        request.messages[0] = systemMessage;

        // User message from the user.
        var userMessage = new Message();
        userMessage.role = "user";
        userMessage.content = userSpeech;
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
                responseScript.Response(responseObject.choices[0].message.content);
            }
        }
    }

    public void AskQuestion()
    {
        // Receive question.     
        StartCoroutine(CallChatGPT(questionText.text));        
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
