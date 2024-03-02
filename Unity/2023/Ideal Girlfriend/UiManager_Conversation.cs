using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Color = UnityEngine.Color;
using UniRx;
using UniRx.Triggers;

public class UiManager_Conversation : MonoBehaviour
{
    [SerializeField]
    private Image imgBackground;

    [SerializeField]
    private Image imgAnswerBackground;

    [SerializeField]
    private CanvasGroup cgConversationSceneUi;

    [SerializeField]
    private TextMeshProUGUI tmpAnswer;

    [SerializeField]
    private RectTransform rtLoading;

    [SerializeField]
    private RectTransform rtAnswerBackground;

    [SerializeField]
    private Button btnEnterPrompt;

    [SerializeField]
    private GameObject objVoiceHint;

    [SerializeField]
    private PopupController popupController;

    [SerializeField]
    private VoiceManager voiceManager;

    [SerializeField]
    private ExceptionDataSO exceptionDataSO;

    private void Start()
    {
        PrepareUpdateImgAnswerBacgroundHeight();

        btnEnterPrompt.onClick.AddListener(OnBtnEnterPromptClicked);

        imgBackground.gameObject.SetActive(true);

        btnEnterPrompt.interactable = false;

        rtLoading.gameObject.SetActive(false);

        objVoiceHint.SetActive(false);

        imgBackground.DOFade(0f, ConstData.UI_FADE_TIME).OnComplete(() =>
        {
            btnEnterPrompt.interactable = true;
        });

        this.UpdateAsObservable()
            .Where(_ => Input.GetMouseButtonDown(0) && objVoiceHint.activeSelf)
            .Subscribe(_ => objVoiceHint.SetActive(false))
            .AddTo(this);
    }

    private async void SendPromptToGptAsync(string prompt)
    {
        string answer;

        rtLoading.gameObject.SetActive(true);

        CancellationTokenSource cts = new();

        CancellationToken token = cts.Token;

        PlayLoadAnimationAsync(token);

        try
        {
            answer = await GptController.GetGptAnswerAsync(RoleType.user, prompt);
        }
        catch (Exception e)
        {
            cts.Cancel();

            rtLoading.gameObject.SetActive(false);

            SetTmpAnswer(exceptionDataSO.GetErrorMessageByEcceptionText(e.ToString()), true);

            PrepareUpdateImgAnswerBacgroundHeight();

            btnEnterPrompt.interactable = true;

            return;
        }

        cts.Cancel();

        rtLoading.gameObject.SetActive(false);

        EditGptAnswerText(ref answer);

        SetTmpAnswer(answer);

        PrepareUpdateImgAnswerBacgroundHeight();

        voiceManager.GenerateAndPlayVoiceFromText(answer);

        btnEnterPrompt.interactable = true;
    }

    private void EditGptAnswerText(ref string gptAnswerText)
    {
        gptAnswerText = gptAnswerText.Replace("\\n", string.Empty)
            .Replace("\\r", string.Empty)
            .Replace("\n", string.Empty)
            .Replace("\n", "")
            .Replace("\r", string.Empty);

        gptAnswerText = gptAnswerText.Replace("*", string.Empty)
            .Replace("#", string.Empty);
    }

    private async void PlayLoadAnimationAsync(CancellationToken token)
    {
        PrepareUpdateImgAnswerBacgroundHeight(true);

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

    private void OnBtnEnterPromptClicked()
    {
        btnEnterPrompt.interactable = false;

        popupController.PrepareMakingPopupAppear();
    }

    private void OnEnteredResetCommand()
    {
        GameData.instance.ResetSaveData();

        imgBackground.DOFade(1f, ConstData.UI_FADE_TIME).OnComplete(() =>
        {
            SceneManager.LoadScene(ConstData.ENTER_KEY_SCENE_NAME);
        });
    }

    public void OnFinishedEnteringPrompt(string enterdPrompt)
    {
        if (string.IsNullOrEmpty(enterdPrompt))
        {
            btnEnterPrompt.interactable = true;

            return;
        }

        if (ColorUtility.TryParseHtmlString(ConstData.MAIN_COLOR_CODE, out Color mainColor)) imgAnswerBackground.color = new Color(mainColor.r, mainColor.g, mainColor.b, imgAnswerBackground.color.a);

        else Debug.LogError("Conversion from color code to main color failed.");

        SetTmpAnswer(string.Empty);

        objVoiceHint.SetActive(false);

        if (enterdPrompt == ConstData.RESET_COMMAND)
        {
            OnEnteredResetCommand();

            return;
        }

        SendPromptToGptAsync(enterdPrompt);
    }

    private void PrepareUpdateImgAnswerBacgroundHeight(bool updateForLoadingUI = false) => UpdateImgAnswerBacgroundHeightAsync(this.GetCancellationTokenOnDestroy(), updateForLoadingUI).Forget();

    private async UniTaskVoid UpdateImgAnswerBacgroundHeightAsync(CancellationToken token, bool updateForLoadingUI)
    {
        if (updateForLoadingUI)
        {
            rtAnswerBackground.sizeDelta = new(rtAnswerBackground.sizeDelta.x, ConstData.IMG_ANSWER_BACKGROUND_HEIGHT_FOR_LOADING_UI);

            rtAnswerBackground.anchoredPosition = new(rtAnswerBackground.anchoredPosition.x, GetImgAnswerBackgroundPosY());

            return;
        }

        if (string.IsNullOrEmpty(tmpAnswer.text)) return;

        await UniTask.WaitUntil(() => tmpAnswer.textInfo.lineCount >= 1, cancellationToken: token);

        float height = tmpAnswer.textInfo.lineCount * ConstData.IMG_ANSWER_BACKGROUND_HEIGHT_PER_LINE;

        rtAnswerBackground.sizeDelta = new(rtAnswerBackground.sizeDelta.x, height);

        rtAnswerBackground.anchoredPosition = new(rtAnswerBackground.anchoredPosition.x, GetImgAnswerBackgroundPosY());
    }

    private float GetImgAnswerBackgroundPosY() => ConstData.WIDTH_BETWEEN_IMG_ANSWER_BACKGROUND_AND_BOTTOM_EDGE + (rtAnswerBackground.sizeDelta.y / 2);

    public void SetTmpAnswer(string text, bool isErrorMessage = false)
    {
        if (isErrorMessage)
        {
            imgAnswerBackground.color = new Color(Color.black.r, Color.black.g, Color.black.b, imgAnswerBackground.color.a);

            tmpAnswer.text = "<color=" + ConstData.ERROR_COLOR_CODE + ">" + text + "</color>";

            return;
        }

        tmpAnswer.text = text;
    }

    public void SetObjVoiceHintActive() => objVoiceHint.SetActive(true);
}