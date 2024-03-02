using System;
using UnityEngine;

public class VoiceManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private UiManager_Conversation uiManager;

    [SerializeField]
    private ExceptionDataSO exceptionDataSO;

    public async void GenerateAndPlayVoiceFromText(string text)
    {
        try
        {
            AudioClip voiceClip = await TtsController.GetVoiceClipFromTextAsync(text);

            if (audioSource.isPlaying) audioSource.Stop();

            audioSource.clip = voiceClip;

            audioSource.Play();

            uiManager.SetObjVoiceHintActive();
        }
        catch (Exception e)
        {
            uiManager.SetTmpAnswer(exceptionDataSO.TtsErrorMessageByEcceptionText(e.ToString()), true);
        }
    }
}