using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class GptController
{
    public static async UniTask<string> GetGptAnswerAsync(RoleType roleType, string prompt, bool isActualPerformance = true, string apiKey = null)
    {
        Dictionary<string, string> headers = new()
        {
            {"Authorization", "Bearer " + (apiKey ?? GameData.instance.ApiKey)},
            {"Content-type", "application/json"},
            {"X-Slack-No-Retry", "1"}
        };

        GptRequest.Message message = new()
        {
            role = roleType.ToString(),
            content = prompt,
        };

        List<GptRequest.Message> messages = new();

        if (isActualPerformance)
        {
            GameData.instance.messages.Add(message);

            messages = GameData.instance.messages;
        }
        else
        {
            messages.Add(message);
        }

        GptRequest request = new()
        {
            model = ConstData.GPT_MODEL_NAME,
            messages = messages,
            max_tokens = ConstData.MAX_TOKENS
        };

        string jsonRequest = JsonUtility.ToJson(request);

        using UnityWebRequest uwr = new(ConstData.GPT_API_URL, "POST")
        {
            uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonRequest)),
            downloadHandler = new DownloadHandlerBuffer()
        };

        foreach (KeyValuePair<string, string> header in headers)
        {
            uwr.SetRequestHeader(header.Key, header.Value);
        }

        try
        {
            await uwr.SendWebRequest();
        }
        catch (Exception e)
        {
            throw e;
        }

        if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Original Error:Failed to send Web request.");

            throw new Exception();
        }

        string answer = JsonUtility.FromJson<GptResponse>(uwr.downloadHandler.text).choices[0].message.content;

        if (isActualPerformance)
        {
            GptRequest.Message responseMessage = new()
            {
                role = RoleType.assistant.ToString(),
                content = answer,
            };

            GameData.instance.messages.Add(responseMessage);
        }

        return answer;
    }
}