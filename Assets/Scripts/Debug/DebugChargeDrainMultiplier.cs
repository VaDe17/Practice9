using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugChargeDrainMultiplier : MonoBehaviour
{
    [SerializeField] private TMP_Text multValueText;

    private PlayerStats playerStats => PlayerBody.Instance.GetStats;

    private void Start()
    {
        multValueText.text = "x" + playerStats.DrainPerSecondMultiplierDuringDashCharge.ToString();
    }

    public void ChangeMult(float value)
    {
        playerStats.DrainPerSecondMultiplierDuringDashCharge += value;

        multValueText.text = "x" + playerStats.DrainPerSecondMultiplierDuringDashCharge.ToString();
    }
}
