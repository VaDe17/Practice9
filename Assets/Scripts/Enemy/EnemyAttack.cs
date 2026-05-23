using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private EnemyBody enemyBody;
    private EnemyStats stats => enemyBody.GetStats;
    private EnemyDetection detection => enemyBody.GetDetection;
    
    private float attackDamage => stats.AttackDamage;
    private float attackDetection => stats.AttackDetection;
    private float attackRange => stats.AttackRange;
    private float attackSpeed => stats.AttackSpeed;
    private float attackWindup => stats.AttackWindup;

    // Приватные переменные
    private PlayerBody playerBody => PlayerBody.Instance;

    // Свойства
    public bool PlayerInAttackRange { get; private set; }
    public bool PlayerInStartAttackRange { get; private set; }

    [HideInInspector] public bool IsAttacking;
    [HideInInspector] public bool CanAttack = true;

    public void Initialize(EnemyBody enemyBody)
    {
        this.enemyBody = enemyBody;
    }

    private void Update()
    {
        if (!detection.IsAggroed)
        {
            PlayerInAttackRange = false;
            return;
        }

        // Проверяем дистанцию до игрока
        float distanceToPlayer = Vector2.Distance(transform.position, playerBody.transform.position);
        PlayerInAttackRange = distanceToPlayer <= attackRange;
        PlayerInStartAttackRange = distanceToPlayer <= attackDetection;
        // Если игрок в радиусе атаки и атака не на кулдауне — атакуем
        if (PlayerInStartAttackRange && CanAttack && !IsAttacking && !playerBody.GetPlayerDash.InDash)
        {
            StartCoroutine(PerformAttack());
        }
    }

    /// <summary>
    /// Корутина атаки с подготовкой (windup) и кулдауном
    /// </summary>
    private IEnumerator PerformAttack()
    {
        Vector3 attackStartPosition = transform.position;
        Vector3 playerSnapshotPosition = playerBody.transform.position;

        Debug.Log("Start Attack");
        IsAttacking = true;
        CanAttack = false;

        enemyBody.GetMovement.SetCanMove(false);
        transform.position = attackStartPosition;
        enemyBody.GetAnimator.SetBool("IsAttacking", true);

        // Фаза подготовки (windup) — можно проиграть анимацию замаха
        // Здесь можно вызвать событие для анимации
        enemyBody.GetSoundLibrary.PlaySoundEffect("Windup", false);
        yield return new WaitForSeconds(attackWindup);
        enemyBody.GetSoundLibrary.PlaySoundEffect("Swing", false);

        Vector2 pounceVector = (playerSnapshotPosition - attackStartPosition) * 700f;
        enemyBody.GetRigidBody2D.AddForce(pounceVector);
        yield return new WaitForSeconds(.1f);
        // Проверяем, всё ещё ли игрок в радиусе атаки после подготовки
        // Если игрок вышел за время windup — атака отменяется
        if (PlayerInAttackRange && !playerBody.GetPlayerDash.InDash)
        {
            // Наносим урон игроку
            playerBody.GetPlayerHealth.TakeDamage(attackDamage);
            Debug.Log($"{gameObject.name} нанёс {attackDamage} урона игроку!");
        }

        // Завершаем атаку
        IsAttacking = false;

        enemyBody.GetMovement.SetCanMove(true);
        enemyBody.GetAnimator.SetBool("IsAttacking", false);

        // Ждём кулдаун
        yield return new WaitForSeconds(attackSpeed - attackWindup);
        
        CanAttack = true;
    }

    public void OnDeath()
    {
        StopAllCoroutines();
        enemyBody.GetMovement.SetCanMove(true);
        enemyBody.GetAnimator.SetBool("IsAttacking", false);
        //EnemyDetection.SetAggro(false);

        enabled = false; // Отключаем скрипт
    }
}