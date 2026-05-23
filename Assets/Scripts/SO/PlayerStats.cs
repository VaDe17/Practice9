using UnityEngine;

[CreateAssetMenu(fileName = "Player Stats", menuName = "Unit Stats/Player Stats")]
public class PlayerStats : ScriptableObject
{
    [Header("Health")]
    public float MaxHealth = 100f;
    public float HealPerKill = 10f;

    [Header("Drain Cost")]
    public float DrainPerSecond = 5f;
    public float DrainPerDash = 5f;
    public float DrainPerSecondMultiplierDuringDashCharge = 2f;

    [Header("Movement")]
    public float MoveSpeed = 7f;
    public float ChargingMoveSpeedMultiplier = 0.33f;

    [Header("Dash Values")]
    public float DashDistance = 3f;
    public float ChargedDashDistanceMultiplier = 2f;
    public float MaxChargeDuration = 0.3f;
    public float DashDuration = 0.075f;
    public float DashDamage = 10f;
    public float DashCoyoteTime = 0.15f;
}