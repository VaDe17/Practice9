using TMPro;
using UnityEngine;

public class DebugDrainPerSecond : MonoBehaviour
{
    [SerializeField] private TMP_Text multValueText;

    private PlayerStats playerStats => PlayerBody.Instance.GetStats;

    private void Start()
    {
        multValueText.text = playerStats.DrainPerSecond.ToString();
    }

    public void ChangeValue(float value)
    {
        playerStats.DrainPerSecond += value;

        multValueText.text = playerStats.DrainPerSecond.ToString();
    }
}
