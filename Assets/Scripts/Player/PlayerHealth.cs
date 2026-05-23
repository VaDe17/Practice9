using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private PlayerBody playerBody;
    private PlayerDash playerDash => playerBody.GetPlayerDash;

    private PlayerStats stats => playerBody.GetStats;
    private float drainPerSecond => stats.DrainPerSecond;
    private float drainPerDash => stats.DrainPerDash;
    private float drainPerSecondMultiplierDuringDashCharge => stats.DrainPerSecondMultiplierDuringDashCharge;
    private float healPerKill => stats.HealPerKill;

    public float MaxHealth { get; private set; } = 100;
    public float CurrentHealth { get; private set; }

    // Events
    public event Action<float> OnTakeDamage;

    [HideInInspector] public bool CanDie = true;

    public void Initialize(PlayerBody playerBody)
    {
        this.playerBody = playerBody;

        MaxHealth = stats.MaxHealth;
        CurrentHealth = MaxHealth;
    }

    private void FixedUpdate()
    {
        TakeDrainDamage();
    }

    // Only from enemies and their attacks
    public void TakeDamage(float incomingDamage)
    {   
        if(CurrentHealth > 0)
        {
            CurrentHealth -= incomingDamage;
            OnTakeDamage?.Invoke(incomingDamage);
        }
        
        CheckHealth();
    }

    // Only from self Damage
    public void TakeDrainDamage()
    {
        float perTickDamage = drainPerSecond * Time.fixedDeltaTime;
        CurrentHealth -= perTickDamage * (playerDash.IsChargingDash ? drainPerSecondMultiplierDuringDashCharge : 1f);

        CheckHealth();
    }
    public void TakeDrainDashDamage()
    {
        CurrentHealth -= drainPerDash;

        CheckHealth();
    }

    public void HealHealth(float incomingHeal)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth += incomingHeal, 0, MaxHealth);
    }
    public void KillHeal()
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth += healPerKill, 0, MaxHealth);
    }

    private void CheckHealth()
    {
        if(CurrentHealth <= 0) Death();
    }
    private void Death()
    {
        if (CanDie)
        {
            RestartManager.Instance.Restart();
        }

        Debug.Log("Player Death");
    }
}