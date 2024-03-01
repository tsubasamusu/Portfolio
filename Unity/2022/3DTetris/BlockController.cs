using UnityEngine;

public class BlockController : MonoBehaviour
{
    private float currentFallSpeed;

    private GameObject mainCamera;

    private BlockDataSO.BlockData myBlockData;

    public BlockDataSO.BlockData BlockData
    {
        get
        {
            return myBlockData;
        }
    }

    private void Update()
    {
        if (CheckContactedDown())
        {
            SetMeRightPos();

            BlockManager.instance.StoppedCurrentBlock();

            return;
        }

        currentFallSpeed = Input.GetKey(KeyCode.DownArrow) ? GameData.instance.SpecialFallSpeed : GameData.instance.NormalFallSpeed;

        transform.Translate(0f, -(currentFallSpeed * Time.deltaTime), 0f);

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if ((mainCamera.transform.position.z < 0f && CheckContactedRight()) || (mainCamera.transform.position.z >= 0f && CheckContactedLeft()))
            {
                return;
            }

            float moveValue = mainCamera.transform.position.z < 0f ? 1f : -1f;

            transform.Translate(new Vector3(moveValue, 0f, 0f));

            BlockManager.instance.MakeGhost();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if ((mainCamera.transform.position.z < 0f && CheckContactedLeft()) || (mainCamera.transform.position.z >= 0f && CheckContactedRight()))
            {
                return;
            }

            float moveValue = mainCamera.transform.position.z < 0f ? -1f : 1f;

            transform.Translate(new Vector3(moveValue, 0f, 0f));

            BlockManager.instance.MakeGhost();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            BlockManager.instance.HoldBlock(myBlockData);
        }

        if (Mathf.Abs(transform.position.x) > (5f - myBlockData.rotLength) || transform.position.y < (0.5f + myBlockData.rotLength) || !CheckLengthToOtherCube())
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1))
            {
                SoundManager.instance.PlaySound(SoundDataSO.SoundName.CannotRotSE);
            }

            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            float rotateValue = mainCamera.transform.position.z < 0 ? 90f : -90f;

            transform.GetChild(0).transform.eulerAngles = new Vector3(0f, 0f, transform.GetChild(0).transform.eulerAngles.z + rotateValue);

            BlockManager.instance.MakeGhost();
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            float rotateValue = mainCamera.transform.position.z < 0 ? -90f : 90f;

            transform.GetChild(0).transform.eulerAngles = new Vector3(0f, 0f, transform.GetChild(0).transform.eulerAngles.z + rotateValue);

            BlockManager.instance.MakeGhost();
        }
    }

    private bool CheckContactedRight()
    {
        for (int i = 0; i < transform.GetChild(0).transform.childCount; i++)
        {
            Ray ray = new(transform.GetChild(0).transform.GetChild(i).transform.position, new Vector3(1f, 0f, 0f));

            if (!Physics.Raycast(ray, out RaycastHit hit, 0.5f))
            {
                continue;
            }

            int isNotGrandchildCount = 0;

            for (int j = 0; j < transform.GetChild(0).transform.childCount; j++)
            {
                if (hit.transform.gameObject == transform.GetChild(0).transform.GetChild(j).gameObject)
                {
                    continue;
                }

                isNotGrandchildCount++;

                if (isNotGrandchildCount == transform.GetChild(0).transform.childCount)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool CheckContactedLeft()
    {
        for (int i = 0; i < transform.GetChild(0).transform.childCount; i++)
        {
            Ray ray = new(transform.GetChild(0).transform.GetChild(i).transform.position, new Vector3(-1f, 0f, 0f));

            if (!Physics.Raycast(ray, out RaycastHit hit, 0.5f))
            {
                continue;
            }

            int isNotGrandchildCount = 0;

            for (int j = 0; j < transform.GetChild(0).transform.childCount; j++)
            {
                if (hit.transform.gameObject == transform.GetChild(0).transform.GetChild(j).gameObject)
                {
                    continue;
                }

                isNotGrandchildCount++;

                if (isNotGrandchildCount == transform.GetChild(0).transform.childCount)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool CheckContactedDown()
    {
        for (int i = 0; i < transform.GetChild(0).transform.childCount; i++)
        {
            Ray ray = new(transform.GetChild(0).transform.GetChild(i).transform.position, Vector3.down);

            if (!Physics.Raycast(ray, out RaycastHit hit, 0.7f))
            {
                continue;
            }

            int isNotGrandchildCount = 0;

            for (int j = 0; j < transform.GetChild(0).transform.childCount; j++)
            {
                if (hit.transform.gameObject == transform.GetChild(0).transform.GetChild(j).gameObject)
                {
                    continue;
                }

                isNotGrandchildCount++;

                if (isNotGrandchildCount == transform.GetChild(0).transform.childCount)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void SetUpBlock(BlockDataSO.BlockData yourBlockData)
    {
        myBlockData = yourBlockData;

        mainCamera = GameObject.FindWithTag("MainCamera");

        BlockManager.instance.MakeGhost();
    }

    private void SetMeRightPos()
    {
        for (int i = 0; i < transform.GetChild(0).transform.childCount; i++)
        {
            Ray ray = new(transform.GetChild(0).transform.GetChild(i).transform.position, Vector3.down);

            if (!Physics.Raycast(ray, out RaycastHit hit, 0.7f))
            {
                continue;
            }

            if (hit.transform.gameObject == transform.GetChild(0).transform.GetChild(i).gameObject)
            {
                continue;
            }

            float length = Mathf.Abs(transform.GetChild(0).transform.GetChild(i).transform.position.y - hit.transform.position.y);

            transform.position = new Vector3(transform.position.x, transform.position.y + (1f - length), 0f);
        }
    }

    private bool CheckLengthToOtherCube()
    {
        for (int i = 0; i < BlockManager.instance.cubeList.Count; i++)
        {
            if (Mathf.Abs(BlockManager.instance.cubeList[i].transform.position.x - transform.position.x) < (myBlockData.rotLength + 1) && Mathf.Abs(BlockManager.instance.cubeList[i].transform.position.y - transform.position.y) < (myBlockData.rotLength + 1))
            {
                return false;
            }
        }

        return true;
    }
}