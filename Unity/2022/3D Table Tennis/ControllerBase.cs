using UnityEngine;

public class ControllerBase : MonoBehaviour
{
    protected CharacterController charaController;

    protected RacketController racketController;

    public void SetUpControllerBase()
    {
        charaController = GetComponent<CharacterController>();

        racketController = transform.GetChild(1).GetComponent<RacketController>();

        racketController.SetUpRacketController();
    }

    private void Update()
    {
        SetCharaDirection();

        if (!racketController.IsIdle)
        {
            return;
        }

        Move();

        ControlRacket();
    }

    private void Move()
    {
        charaController.Move(GetMoveDir() * Time.deltaTime * GameData.instance.MoveSpeed + (Vector3.down * GameData.instance.Gravity));
    }

    protected virtual Vector3 GetMoveDir()
    {
        return Vector3.zero;
    }

    protected virtual void ControlRacket()
    {

    }

    protected virtual void SetCharaDirection()
    {

    }
}