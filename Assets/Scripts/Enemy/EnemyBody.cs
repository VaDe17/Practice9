using System.Collections;
using Pathfinding;
using UnityEngine;

public class EnemyBody : MonoBehaviour
{
    [Header("Enemy Stats Reference")]
    [SerializeField] private EnemyStats enemyStats;
    public EnemyStats GetStats => enemyStats;

    [Header("Enemy Scripts References")]
    public EnemyHealth GetHealth;
    public EnemyMovement GetMovement;
    public EnemyDetection GetDetection;
    public EnemyAttack GetAttack;
    public EnemyVFX GetVFX;

    [Header("Component References")]
    public Collider2D GetMovementCollider;
    public Rigidbody2D GetRigidBody2D;
    public SpriteRenderer GetSprite;
    public Animator GetAnimator;
    public SoundLibrary GetSoundLibrary;

    [Header("Pathfinding References")]
    public AIPath GetAIPath;
    public AIDestinationSetter GetDestinationSetter;

    [Header("Masks References")]
    public LayerMask ObstacleMask;
    public LayerMask PlayerMask;

    private Vector3 startPostion;

    private void Start()
    {
        startPostion = transform.position;

        RestartManager.Instance.AddEnemy(this);

        GetHealth.Initialize(this);
        GetMovement.Initialize(this);
        GetDetection.Initialize(this);
        GetAttack.Initialize(this);
    }

    public void Enable(bool isActive)
    {
        GetHealth.enabled = isActive;
        GetMovement.enabled = isActive;
        GetDetection.enabled = isActive;
        GetAttack.enabled = isActive;

        GetMovementCollider.enabled = isActive;
        GetAIPath.enabled = isActive;

        if (!isActive)
            StartCoroutine(BodyCleanUp());
        else 
            StopAllCoroutines();
    }

    private IEnumerator BodyCleanUp()
    {
        yield return new WaitForSeconds(4f);
        gameObject.SetActive(false);
    }

    public void Restart()
    {
        gameObject.SetActive(true);
        GetHealth.isDead = false;
        GetHealth.isAnticipatingDeath = false;

        Enable(true);
        
        GetMovement.ResetSpeed();
        GetDetection.SetAggro(false);

        GetAttack.IsAttacking = false;
        GetAttack.CanAttack = true;

        GetAnimator.SetBool("IsDead", false);
        GetAnimator.SetTrigger("Restart");

        transform.position = startPostion;
    }
}
