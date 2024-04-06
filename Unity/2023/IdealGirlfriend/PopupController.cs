using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Scripting;

public class PopupController : MonoBehaviour
{
    [SerializeField]
    private UiManager_Conversation uiManager;

#if UNITY_WEBGL && !UNITY_EDITOR

    [DllImport("__Internal")]
    private static extern void MakePopupAppear(string objectID, string popupDescription);

#else

    private static void MakePopupAppear(string objectID, string popupDescription) { }

#endif

    [Preserve]
    public void PrepareMakingPopupAppear()
    {
#if UNITY_WEBGL && !UNITY_EDITOR

        SaltName();

        MakePopupAppear(gameObject.name, ConstData.POP_UP_DESCRIPTION_ENTER_PROMPT);

#endif
    }

    [Preserve]
    public void ReceiveEnterdText(string text)
    {
        UnSaltName();

        uiManager.OnFinishedEnteringPrompt(text);
    }

    [Preserve]
    private void SaltName()
    {
        if (!gameObject.name.Contains(ConstData.SALT))
        {
            gameObject.name += ConstData.SALT;
        }
    }

    private void UnSaltName()
    {
        if (gameObject.name.Contains(ConstData.SALT))
        {
            gameObject.name = gameObject.name.Replace(ConstData.SALT, string.Empty);
        }
    }
}