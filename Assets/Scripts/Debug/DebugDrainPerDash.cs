using TMPro;
using UnityEngine;

public class DebugDrainPerDash : MonoBehaviour
{
    [SerializeField] private TMP_Text multValueText;

    private PlayerStats playerStats => PlayerBody.Instance.GetStats;

    private void Start()
    {
        multValueText.text = playerStats.DrainPerDash.ToString();
    }

    public void ChangeValue(float value)
    {
        playerStats.DrainPerDash += value;

        multValueText.text = playerStats.DrainPerDash.ToString();
    }
}
