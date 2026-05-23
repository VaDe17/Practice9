using Pathfinding;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private EnemyBody enemyBody;

    private AIPath pathSetter => enemyBody.GetAIPath;
    //private AIDestinationSetter destinationSetter => enemyBody.GetDestinationSetter;

    public void Initialize(EnemyBody enemyBody)
    {
        this.enemyBody = enemyBody;
    }

    public void SetSpeed(float newSpeed)
    {
        pathSetter.maxSpeed = newSpeed;
    }
    public void ResetSpeed()
    {
        pathSetter.maxSpeed = enemyBody.GetStats.MoveSpeed;
    }

    public void SetCanMove(bool canMove)
    {
        pathSetter.canMove = canMove;
        enemyBody.GetAnimator.SetBool("IsMoving", canMove);
    }

    private void Update()
    {
        enemyBody.GetAnimator.SetBool("IsMoving", enemyBody.GetAIPath.canMove);

        if (enemyBody.GetAIPath.canMove == true)
        {
            bool playerOnTheRightSide = PlayerBody.Instance.transform.position.x - transform.position.x > 0;
            enemyBody.GetSprite.flipX = !playerOnTheRightSide;
        }
    }
}
