using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager_Start : MonoBehaviour
{
    [SerializeField]
    private Image imgBtnStart;

    [SerializeField]
    private TextMeshProUGUI tmpBtnStart;

    [SerializeField]
    private Button btnStart;

    [SerializeField]
    private CanvasGroup cgStartSceneUi;

    private void Start()
    {
        btnStart.onClick.AddListener(OnBtnStartClicked);
    }

    private void OnBtnStartClicked()
    {
        (tmpBtnStart.color, imgBtnStart.color) = (imgBtnStart.color, tmpBtnStart.color);

        btnStart.interactable = false;

        cgStartSceneUi.DOFade(0f, ConstData.UI_FADE_TIME).OnComplete(() =>
        {
            string nextSceneName = string.IsNullOrEmpty(GameData.instance.ApiKey) ? ConstData.ENTER_KEY_SCENE_NAME : ConstData.CONVERSATION_SCENE_NAME;

            SceneManager.LoadScene(nextSceneName);
        });
    }
}