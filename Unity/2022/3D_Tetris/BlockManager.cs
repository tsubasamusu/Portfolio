using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BlockManager : MonoBehaviour
{
    public static BlockManager instance;

    [SerializeField]
    private BlockGenerator blockGenerator;

    private GameManager gameManager;

    [SerializeField]
    private Material ghostMaterial;

    [HideInInspector]
    public List<GameObject> cubeList = new();

    private GameObject currentBlock;

    private BlockDataSO.BlockData holdBlockData;

    private GameObject ghost;

    private bool endDigestion = true;

    public bool EndDigestion
    {
        get
        {
            return endDigestion;
        }
    }

    public BlockDataSO.BlockData HoldBlockData
    {
        get
        {
            return holdBlockData;
        }
    }

    public GameObject CurrentBlock
    {
        get
        {
            return currentBlock;
        }
        set
        {
            currentBlock = value;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetUpBlockManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public void StoppedCurrentBlock()
    {
        for (int i = 0; i < 4; i++)
        {
            cubeList.Add(currentBlock.transform.GetChild(0).transform.GetChild(0).gameObject);

            currentBlock.transform.GetChild(0).transform.GetChild(0).transform.SetParent(transform);
        }

        if (currentBlock.TryGetComponent(out BlockController blockController))
        {
            blockController.enabled = false;
        }
        else
        {
            Debug.Log("現在アクティブなブロックからのBlockControllerの取得に失敗");
        }

        for (int j = 0; j < cubeList.Count; j++)
        {
            if (cubeList[j].transform.position.y > 20.5f)
            {
                gameManager.PrepareGameOver();

                return;
            }
        }

        CheckDigested();

        currentBlock = blockGenerator.GenerateBlock();
    }

    private void CheckDigested()
    {
        int digestedCount = 0;

        for (int i = 1; i < 21; i++)
        {
            List<GameObject> samePosYList = cubeList.FindAll(x => (x.transform.position.y > (i - 0.5f)) && (x.transform.position.y < (i + 0.5f)));

            if (samePosYList.Count < 10)
            {
                continue;
            }

            endDigestion = false;

            for (int j = 0; j < 10; j++)
            {
                GameObject cube = samePosYList[0];

                cubeList.Remove(cubeList.Find(x => x == cube));

                samePosYList.RemoveAt(0);

                Destroy(cube);
            }

            digestedCount++;

            for (int k = 0; k < cubeList.Count; k++)
            {
                if (cubeList[k].transform.position.y > i)
                {
                    cubeList[k].transform.DOMoveY(cubeList[k].transform.position.y - digestedCount, 0.5f)
                        .OnComplete(() => endDigestion = true);
                }
            }
        }

        if (digestedCount > 0)
        {
            SoundManager.instance.PlaySound(SoundDataSO.SoundName.DigestionSE);

            UIManager.instance.UpdateTxtScore(GameData.instance.ScorePerColumn * digestedCount);
        }
    }

    public void HoldBlock(BlockDataSO.BlockData blockData)
    {
        Destroy(currentBlock);

        if (holdBlockData == null)
        {
            holdBlockData = blockData;

            UIManager.instance.SetImgHoldBllock(blockData.sprite);

            currentBlock = blockGenerator.GenerateBlock();
        }
        else
        {
            UIManager.instance.ClearImgHoldBlock();

            currentBlock = blockGenerator.GenerateBlock(holdBlockData);

            holdBlockData = null;
        }
    }

    public void MakeGhost()
    {
        if (ghost != null)
        {
            Destroy(ghost);
        }

        List<MeshRenderer> meshRenderersList = new();

        ghost = Instantiate(CurrentBlock);

        if (ghost.TryGetComponent(out BlockController blockController))
        {
            blockController.enabled = false;
        }
        else
        {
            Debug.Log("ゴーストからのBlockControllerの取得に失敗");
        }

        for (int i = 0; i < 4; i++)
        {
            if (ghost.transform.GetChild(0).transform.GetChild(i).gameObject.TryGetComponent(out BoxCollider collider))
            {
                collider.enabled = false;
            }
            else
            {
                Debug.Log("ゴーストの孫からのコライダーの取得に失敗");
            }

            if (ghost.transform.GetChild(0).transform.GetChild(i).gameObject.TryGetComponent(out MeshRenderer meshRenderer))
            {
                meshRenderer.material = ghostMaterial;

                meshRenderersList.Add(meshRenderer);

                meshRenderer.enabled = false;
            }
            else
            {
                Debug.Log("ゴーストからのMeshRendererの取得に失敗");
            }
        }

        ghost.AddComponent<GhostController>()
            .SetUpGhost(meshRenderersList);
    }
}