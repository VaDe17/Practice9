using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    private EnemyBody enemyBody;

    private AIDestinationSetter destinationSetter => enemyBody.GetDestinationSetter;
    private AIPath pathfinding => enemyBody.GetAIPath;
    
    private LayerMask obstacleLayers => enemyBody.ObstacleMask;
    [SerializeField] private CircleCollider2D AwakeTriggerCollider;

    private Collider2D[] enemyInRange;

    public bool IsAggroed {get; private set;}

    private PlayerBody playerBody => PlayerBody.Instance;

    public void Initialize(EnemyBody enemyBody)
    {
        this.enemyBody = enemyBody;
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (IsAggroed)
            return;

        if (other.transform.parent != playerBody.transform)
            return;
        
        if (IsClearLOS(gameObject, other.gameObject, obstacleLayers))
        {
            AlertUnitsAround(AwakeTriggerCollider.radius * 1.5f);
        }
    }

    private bool IsClearLOS(GameObject from, GameObject to, LayerMask obstacleLayers)
    {
        Vector2 start = from.transform.position;
        Vector2 end = to.transform.position;
        
        RaycastHit2D hit = Physics2D.Linecast(start, end, obstacleLayers);
        
        if (hit.collider != null)
        {
            return hit.collider.gameObject == to;
        }
        
        return true;
    }

    public void AlertUnitsAround(float radius)
    {
        enemyInRange = Physics2D.OverlapCircleAll(transform.position, radius, playerBody.EnemiesMask);

        foreach (var collider in enemyInRange)
        {
            EnemyDetection detectedEnemy = collider.transform.parent?.GetComponent<EnemyDetection>();

            if(detectedEnemy != null)
            {
                detectedEnemy.SetAggro(true);
            }
        }
        enemyInRange = null;
    }

    public void SetAggro(bool aggro)
    {   
        IsAggroed = aggro;

        if (aggro)
        {
            // Start the Chase
            destinationSetter.enabled = true;
            destinationSetter.target = playerBody.transform;

            enemyBody.GetMovement.SetCanMove(true);
            pathfinding.canSearch = true;
            pathfinding.autoRepath.mode = AutoRepathPolicy.Mode.Dynamic;
        }
        else
        {
            // Stop the Chase

            destinationSetter.target = null;
            destinationSetter.enabled = false;

            enemyBody.GetMovement.SetCanMove(false);
            pathfinding.canSearch = false;
            pathfinding.SetPath(null);
        }
    }
}
