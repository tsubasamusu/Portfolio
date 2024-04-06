using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Tsubasa;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private UIManager uIManager;

    [SerializeField]
    private CharacterManager characterManager;

    [SerializeField]
    private CameraController cameraController;

    private bool isSolo;

    private bool useTamako;

    private IEnumerator Start()
    {
        SetControllersFalse();

        for (int i = 0; i < characterManager.characterClassDataList.Count; i++)
        {
            characterManager.GetCharacterHealth((CharacterManager.CharaName)i).SetUpCharacterHealth(this, cameraController);
        }

        uIManager.HideCursor();

        SoundManager.instance.PlaySound(SoundManager.instance.GetBgmData(SoundDataSO.BgmName.Main).clip, true);

        yield return uIManager.PlayGameStart();

        yield return uIManager.SetModeSelect();

        yield return CheckModeSelect();

        if (!isSolo)
        {
            SoundManager.instance.ChangeBgmMainToGame();

            yield return StartGame();

            for (int i = 0; i < characterManager.characterClassDataList.Count; i++)
            {
                characterManager.GetCharacterController((CharacterManager.CharaName)i).enabled = true;

                characterManager.GetCharacterController((CharacterManager.CharaName)i).SetUpCharacterController(characterManager);
            }

            yield break;
        }

        yield return uIManager.SetCharaSelect();

        yield return CheckCharaSelect();

        SoundManager.instance.ChangeBgmMainToGame();

        yield return StartGame();

        if (useTamako)
        {
            characterManager.GetCharacterController(CharacterManager.CharaName.Tamako).enabled = true;

            characterManager.GetCharacterController(CharacterManager.CharaName.Tamako).SetUpCharacterController(characterManager);

            characterManager.GetNpcController(CharacterManager.CharaName.Mashiro).enabled = true;
        }
        else
        {
            characterManager.GetCharacterController(CharacterManager.CharaName.Mashiro).enabled = true;

            characterManager.GetCharacterController(CharacterManager.CharaName.Mashiro).SetUpCharacterController(characterManager);

            characterManager.GetNpcController(CharacterManager.CharaName.Tamako).enabled = true;
        }
    }

    private void SetControllersFalse()
    {
        for (int i = 0; i < characterManager.characterClassDataList.Count; i++)
        {
            characterManager.GetCharacterController((CharacterManager.CharaName)i).enabled = false;

            characterManager.GetNpcController((CharacterManager.CharaName)i).enabled = false;
        }
    }

    private IEnumerator CheckModeSelect()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SoundManager.instance.PlaySound(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.Select).clip);

                isSolo = true;

                break;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SoundManager.instance.PlaySound(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.Select).clip);

                isSolo = false;

                break;
            }

            yield return null;
        }
    }

    private IEnumerator CheckCharaSelect()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SoundManager.instance.PlaySound(SoundManager.instance.GetVoiceData(SoundDataSO.VoiceName.MashiroName).clip);

                SoundManager.instance.PlaySound(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.Select).clip);

                useTamako = false;

                break;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SoundManager.instance.PlaySound(SoundManager.instance.GetVoiceData(SoundDataSO.VoiceName.TamakoName).clip);

                SoundManager.instance.PlaySound(SoundManager.instance.GetSoundEffectData(SoundDataSO.SoundEffectName.Select).clip);

                useTamako = true;

                break;
            }

            yield return null;
        }
    }

    private IEnumerator StartGame()
    {
        yield return uIManager.GoToGame();

        SoundManager.instance.PlaySound(SoundManager.instance.GetVoiceData(SoundDataSO.VoiceName.CountDown).clip);

        yield return uIManager.CountDown();
    }

    public void SetUpEndGame()
    {
        StartCoroutine(EndGame());
    }

    private IEnumerator EndGame()
    {
        SoundManager.instance.PlaySound(SoundManager.instance.GetVoiceData(SoundDataSO.VoiceName.GameSet).clip);

        SoundManager.instance.StopSound(0.5f);

        yield return uIManager.EndGame();

        SceneManager.LoadScene("Main");
    }
}