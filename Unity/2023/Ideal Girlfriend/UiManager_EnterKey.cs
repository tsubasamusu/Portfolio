using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager_EnterKey : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup cgEnterKeySceneUi;

    [SerializeField]
    private RectTransform rtLoading;

    [SerializeField]
    private TMP_InputField ifKey;

    [SerializeField]
    private TextMeshProUGUI tmpBtnConfirm;

    [SerializeField]
    private Button btnConfirm;

    [SerializeField]
    private Image imgBtnConfirm;

    [SerializeField]
    private GameObject objError;

    private void Start()
    {
        btnConfirm.onClick.AddListener(OnBtnConfirmClicked);

        ifKey.onValueChanged.AddListener(OnIfKeyTextChanged);

        btnConfirm.interactable = ifKey.interactable = false;

        rtLoading.gameObject.SetActive(false);

        objError.SetActive(false);

        cgEnterKeySceneUi.alpha = 0f;

        cgEnterKeySceneUi.DOFade(1f, ConstData.UI_FADE_TIME).OnComplete(() =>
        {
            btnConfirm.interactable = ifKey.interactable = true;
        });
    }

    private async void OnBtnConfirmClicked()
    {
        btnConfirm.interactable = ifKey.interactable = false;

        objError.SetActive(false);

        rtLoading.gameObject.SetActive(true);

        CancellationTokenSource cts = new();

        CancellationToken token = cts.Token;

        PlayLoadAnimationAsync(token);

        try
        {
            await GptController.GetGptAnswerAsync(RoleType.user, ConstData.GPT_TEST_PROMPT, false, ifKey.text);
        }
        catch (Exception)
        {
            ifKey.text = string.Empty;

            objError.SetActive(true);

            btnConfirm.interactable = ifKey.interactable = true;

            cts.Cancel();

            rtLoading.gameObject.SetActive(false);

            return;
        }

        cts.Cancel();

        GameData.instance.ApiKey = ifKey.text;

        rtLoading.gameObject.SetActive(false);

        cgEnterKeySceneUi.DOFade(0f, ConstData.UI_FADE_TIME).OnComplete(() =>
        {
            SceneManager.LoadScene(ConstData.CONVERSATION_SCENE_NAME);
        });
    }

    private async void PlayLoadAnimationAsync(CancellationToken token)
    {
        rtLoading.eulerAngles = Vector3.zero;

        while (true)
        {
            rtLoading.Rotate(0f, 0f, ConstData.LOAD_ANIMATION_ANGLE);

            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(ConstData.LOAD_ANIMATION_SPAN), cancellationToken: token);
            }
            catch (Exception e)
            {
                if (e is not OperationCanceledException) Debug.LogException(e);

                break;
            }
        }
    }

    private void OnIfKeyTextChanged(string afterChangeText)
    {
        objError.SetActive(false);
    }
}