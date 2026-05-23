using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    private PlayerBody playerBody;
    private PlayerDash playerDash => playerBody.GetPlayerDash;
    //private PlayerMovement playerMovement => playerBody.GetPlayerMovement;
    private PlayerHealth playerHealth => playerBody.GetPlayerHealth;

    [SerializeField] private SoundLibrary soundLibrary;

    public void Initialize(PlayerBody playerBody)
    {
        this.playerBody = playerBody;

        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        playerHealth.OnTakeDamage += HandleTakeDamage;

        playerDash.OnDashCharge += HandleDashCharging;
        playerDash.OnDashStart += HandleDashStart;
        playerDash.OnDashHit += HandleDashHit;
    }

    private void OnDisable()
    {
        playerHealth.OnTakeDamage -= HandleTakeDamage;

        playerDash.OnDashCharge -= HandleDashCharging;
        playerDash.OnDashStart -= HandleDashStart;
        playerDash.OnDashHit -= HandleDashHit;
    }

    private void HandleTakeDamage(float damageAmount)
    {
        soundLibrary.PlaySoundEffect("GetHurt", true);
    }
    private void HandleDashCharging()
    {
        
    }
    private void HandleDashStart()
    {
        soundLibrary.PlaySoundEffectWithCustomPitch("DashStart", 0f, playerBody.GetStats.ChargedDashDistanceMultiplier, playerDash.GetCurrentDashChargeDuration, .6f);
    }
    private void HandleDashHit(int hitCount)
    {
        soundLibrary.PlaySoundEffect("DashHit", false);
    }
}
