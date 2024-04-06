using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    List<MeshRenderer> meshRenderersList = new();

    private void Update()
    {
        if (CheckContactedDown())
        {
            LandingMe();

            return;
        }

        if (!BlockManager.instance.EndDigestion)
        {
            return;
        }

        transform.Translate(0f, -1f, 0f);
    }

    public void SetUpGhost(List<MeshRenderer> meshRenderersList)
    {
        this.meshRenderersList = meshRenderersList;

        if (!BlockManager.instance.CurrentBlock.TryGetComponent(out BlockController blockController))
        {
            Debug.Log("現在アクティブなブロックからのBlockControllerの取得に失敗");

            return;
        }

        BlockDataSO.BlockData myBlockData = blockController.BlockData;

        float posY = myBlockData.isEvenWidth ? 25.5f : 25f;

        transform.position = new Vector3(transform.position.x, posY, 0f);
    }

    private bool CheckContactedDown()
    {
        for (int i = 0; i < 4; i++)
        {
            Ray ray = new(transform.GetChild(0).transform.GetChild(i).transform.position, Vector3.down);

            if (Physics.Raycast(ray, out RaycastHit hit, 0.6f) && hit.transform.root.gameObject != BlockManager.instance.CurrentBlock)
            {
                return true;
            }
        }

        return false;
    }

    private void LandingMe()
    {
        SetMeRightPos();

        for (int i = 0; i < meshRenderersList.Count; i++)
        {
            meshRenderersList[i].enabled = true;
        }

        if (TryGetComponent(out GhostController ghostController))
        {
            ghostController.enabled = false;
        }
        else
        {
            Debug.Log("自身のGhostControllerの取得に失敗");
        }
    }

    private void SetMeRightPos()
    {
        float excess = transform.position.y % 0.5f;

        float valueY = excess < 0.25 ? -excess : 0.5f - excess;

        transform.position = new Vector3(transform.position.x, transform.position.y + valueY, 0f);
    }
}