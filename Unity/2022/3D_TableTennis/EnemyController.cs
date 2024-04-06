using UnityEngine;

public class EnemyController : ControllerBase
{
    [SerializeField]
    private BoundPoint enemyBoundPoint;

    [SerializeField]
    private BallController ballController;

    private Vector3 firstPos;

    protected override Vector3 GetMoveDir()
    {
        if (ballController.CurrentOwner == OwnerType.Enemy)
        {
            return firstPos - transform.position;
        }

        if (ballController.InCourt)
        {
            return ballController.transform.position - transform.position;
        }

        return Vector3.zero;
    }

    public void SetUpEnemyController(BallController ballController)
    {
        this.ballController = ballController;

        firstPos = transform.position;
    }

    protected override void ControlRacket()
    {
        if (Mathf.Abs((transform.position - ballController.transform.position).magnitude) > GameData.instance.EnemyShotRange)
        {
            return;
        }

        if (!racketController.IsIdle)
        {
            return;
        }

        racketController.Drive(transform.position.x >= ballController.transform.position.x);
    }

    protected override void SetCharaDirection()
    {
        transform.LookAt(enemyBoundPoint.transform.position);
    }
}