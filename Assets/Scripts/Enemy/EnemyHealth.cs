using Pathfinding;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private EnemyBody enemyBody;
    private EnemyStats stats => enemyBody.GetStats;

    public float MaxHealth { get; private set; }
    public float CurrentHealth { get; private set; }

    public bool isDead;
    public bool isAnticipatingDeath;

    public void Initialize(EnemyBody enemyBody)
    {
        this.enemyBody = enemyBody;
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(float incomingDamage)
    {
        CurrentHealth -= incomingDamage;

        if(CurrentHealth <= 0) Death();
    }

    public void AnticipateDamage(float incomingDamage)
    {
        if (CurrentHealth <= incomingDamage && !isAnticipatingDeath)
        {
            isAnticipatingDeath = true;

            Debug.Log("SetHurt");
            enemyBody.GetAttack.OnDeath();
            enemyBody.GetAnimator.SetTrigger("Hurt");
            enemyBody.GetAnimator.SetBool("IsDead", true);
            enemyBody.GetMovement.SetSpeed(1.5f);
            
            Vector2 vectorToPlayer = ((Vector2)PlayerBody.Instance.transform.position - new Vector2(transform.position.x, transform.position.y)).normalized;
            float angleToPlayer = Mathf.Atan2(vectorToPlayer.y, vectorToPlayer.x) * Mathf.Rad2Deg + 90;
            enemyBody.GetVFX.TriggerVFX("BloodSquirt", angleToPlayer);
        }
    }

    public void HealHealth(float incomingHeal)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth += incomingHeal, 0, MaxHealth);
    }

    private void Death()
    {
        if(isDead)  
            return;

        enemyBody.GetAnimator.SetBool("IsDead", true);

        enemyBody.GetAttack.OnDeath();
        PlayerBody.Instance.GetPlayerHealth.KillHeal();

        enemyBody.GetDetection.AlertUnitsAround(4f);
        enemyBody.GetDetection.SetAggro(false);


        enemyBody.GetSoundLibrary.PlaySoundEffect("Death", true);
        
        enemyBody.Enable(false);
        isDead = true;
        isAnticipatingDeath = false;
        Debug.Log("Unit Death");
    }
}