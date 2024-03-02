using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text txtmetMembersCount;

    [SerializeField]
    private Text txtMessage;

    [SerializeField]
    private Image imgLogo;

    [SerializeField]
    private bool cursorIsVisible;

    private void Start()
    {
        UpdateTxtMembersCount();

        UpdateTxtMessage("", Color.black);

        PlayGameStart();

        Cursor.visible = cursorIsVisible;
    }

    public void UpdateTxtMembersCount()
    {
        txtmetMembersCount.text = "You Met\n" + GameData.instance.MetMembersCount.ToString() + "/8\nMembers";
    }

    public void UpdateTxtMessage(string text, Color color)
    {
        txtMessage.text = text;

        txtMessage.color = color;
    }

    private void PlayGameStart()
    {
        imgLogo.DOFade(0f, 0f);

        imgLogo.DOFade(1f, 2f).SetLoops(2, LoopType.Yoyo);
    }
}