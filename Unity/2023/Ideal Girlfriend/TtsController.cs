using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class TtsController
{
    public static async UniTask<AudioClip> GetVoiceClipFromTextAsync(string text, VoiceType voiceType = VoiceType.nova, VoiceResponseFormatType voiceResponseFormatType = VoiceResponseFormatType.mp3, float speakSpeed = 1f)
    {
        Dictionary<string, string> headers = new()
        {
            {"Authorization", "Bearer " + GameData.instance.ApiKey},
            {"Content-type", "application/json"},
            {"X-Slack-No-Retry", "1"}
        };

        TtsRequest request = new()
        {
            model = ConstData.TTS_MODEL_NAME,
            input = text,
            voice = voiceType.ToString(),
            response_format = voiceResponseFormatType.ToString(),
            speed = speakSpeed
        };

        string jsonRequest = JsonUtility.ToJson(request);

        using UnityWebRequest uwr = new(ConstData.TTS_API_URL, "POST")
        {
            uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonRequest)),
            downloadHandler = new DownloadHandlerAudioClip(string.Empty, AudioType.MPEG)
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

        return DownloadHandlerAudioClip.GetContent(uwr);
    }
}