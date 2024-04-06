using UnityEngine;

public class PlayerController : ControllerBase
{
    private Vector3 firstPos;

    public void SetUpPlayerController()
    {
        firstPos = transform.position;
    }

    protected override Vector3 GetMoveDir()
    {
        if (!Input.anyKey)
        {
            return Vector3.zero;
        }

        float moveH = Input.GetAxis("Horizontal");

        float moveV = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveH, 0, moveV);

        return Camera.main.transform.forward * movement.z + Camera.main.transform.right * movement.x;
    }

    protected override void ControlRacket()
    {
        if (Input.GetMouseButtonDown(0))
        {
            racketController.Drive(false);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            racketController.Drive(true);
        }
    }

    protected override void SetCharaDirection()
    {
        transform.eulerAngles = new Vector3(0f, Camera.main.transform.eulerAngles.y, 0f);
    }

    public void ResetPlayerPos()
    {
        charaController.enabled = false;

        transform.position = firstPos;

        charaController.enabled = true;
    }
}