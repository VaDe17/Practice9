using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    private PlayerHealth playerHealth;

    private void Start()
    {
        playerHealth = PlayerBody.Instance.GetPlayerHealth;

        healthBar.maxValue = playerHealth.MaxHealth;
    }

    private void Update()
    {
        healthBar.value = playerHealth.CurrentHealth;
    }
}
