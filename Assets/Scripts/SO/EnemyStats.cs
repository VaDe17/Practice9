using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Stats", menuName = "Unit Stats/Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    [Header("Health")]
    public float MaxHealth = 10f;

    [Header("Movement")]
    public float MoveSpeed = 8f;

    [Header("Attack Values")]
    public float AttackDamage = 10f;
    public float AttackDetection = 3f;
    public float AttackRange = 2f;
    public float AttackSpeed = 0.3f;
    public float AttackWindup = 0.075f;
}
