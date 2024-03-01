using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGenerator : MonoBehaviour
{
    [SerializeField]
    private BlockDataSO blockDataSO;

    [HideInInspector]
    public BlockDataSO.BlockData[] nextBlockDatas;

    private bool stop;

    public void SetUpBlockGenerator()
    {
        nextBlockDatas = new BlockDataSO.BlockData[GameData.instance.AppointmentsNumber];

        for (int i = 0; i < nextBlockDatas.Length; i++)
        {
            nextBlockDatas[i] = blockDataSO.blockDataList[Random.Range(0, blockDataSO.blockDataList.Count)];
        }
    }

    public GameObject GenerateBlock(BlockDataSO.BlockData blockData = null)
    {
        if (stop)
        {
            return null;
        }

        BlockDataSO.BlockData originalData = blockData == null ? nextBlockDatas[0] : blockData;

        GameObject generatedBlock = Instantiate(originalData.prefab);

        float x = originalData.isEvenWidth ? 0f : 0.5f;

        generatedBlock.transform.position = new Vector3(x, 25f, 0f);

        if (generatedBlock.TryGetComponent(out BlockController blockController))
        {
            blockController.SetUpBlock(originalData);
        }
        else
        {
            Debug.Log("¶¬‚µ‚½ƒuƒƒbƒN‚©‚ç‚ÌBlockController‚ÌŽæ“¾‚ÉŽ¸”s");
        }

        if (blockData != null)
        {
            return generatedBlock;
        }

        UpdateNextBlockDatas();

        UIManager.instance.SetImgNextBlocks(nextBlockDatas);

        return generatedBlock;
    }

    private void UpdateNextBlockDatas()
    {
        for (int i = 0; i < nextBlockDatas.Length; i++)
        {
            if (i == (nextBlockDatas.Length - 1))
            {
                nextBlockDatas[i] = blockDataSO.blockDataList[Random.Range(0, blockDataSO.blockDataList.Count)];

                break;
            }

            nextBlockDatas[i] = nextBlockDatas[i + 1];
        }
    }

    public void StopGenerateBlock()
    {
        stop = true;
    }
}