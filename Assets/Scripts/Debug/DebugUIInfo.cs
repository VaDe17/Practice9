using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DebugUIInfo : MonoBehaviour
{
    private PlayerBody playerBody;
    private PlayerMovement playerMovement;
    private PlayerHealth playerHealth;
    private PlayerDash playerDash;

    [Header("Movement")]
    [SerializeField] private TMP_Text DashStateText;
    [SerializeField] private TMP_Text MoveStateText;
    [SerializeField] private TMP_Text MoveSpeedText;
    [SerializeField] private TMP_Text ChargingDashText;
    [SerializeField] private TMP_Text DashChargeDurationText;
    [SerializeField] private TMP_Text CurrentChargeMultiplierText;

    [Header("Health")]
    [SerializeField] private TMP_Text MaxHealthText;
    [SerializeField] private TMP_Text CurrentHealthText;

    [Header("Experemental")]
    [SerializeField] private Toggle GodMode;
    [SerializeField] private Toggle TimeSlowOnCharge;
    [SerializeField] private Toggle DashDistanceMarker;

    private void Start()
    {
        playerBody = PlayerBody.Instance;

        playerMovement = playerBody.GetPlayerMovement;
        playerDash = playerBody.GetPlayerDash;
        playerHealth = playerBody.GetPlayerHealth;
    }

    private void Update()
    {
        // Backend stats
        DashStateText.text                  = playerDash.CanDash.ToString();
        MoveStateText.text                  = playerMovement.CanMove.ToString();
        MoveSpeedText.text                  = playerBody.GetRigidBody2D.linearVelocity.ToString();
        ChargingDashText.text               = playerDash.IsChargingDash.ToString();
        DashChargeDurationText.text         = playerDash.GetCurrentDashChargeDuration.ToString();
        CurrentChargeMultiplierText.text    = playerDash.CurrentDashMultiplier().ToString();

        // Health
        MaxHealthText.text                  = playerHealth.MaxHealth.ToString();
        CurrentHealthText.text              = playerHealth.CurrentHealth.ToString();

        // Experemental Features
        playerHealth.CanDie                 = !GodMode.isOn;
        playerDash.SlowTimeOnDashCharge     = TimeSlowOnCharge.isOn;
        playerDash.UseDashDistanceIndicator = DashDistanceMarker.isOn;
    }
}
