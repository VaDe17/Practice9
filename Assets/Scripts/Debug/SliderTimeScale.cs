using UnityEngine;
using UnityEngine.UI;

public class SliderTimeScale : MonoBehaviour
{
    [SerializeField] Slider slider;

    private void Update()
    {
        if (PlayerBody.Instance.GetPlayerDash.SlowTimeOnDashCharge)
            return;

        float newTimeScale = 1 - slider.value;

        Time.timeScale = newTimeScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }
}
