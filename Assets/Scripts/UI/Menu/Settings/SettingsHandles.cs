using UnityEngine;
using UnityEngine.UI;

public class SettingsHandles : MonoBehaviour
{
    public Button ScreenHandle;
    public GameObject ScreenPage;

    public Button SoundHandle;
    public GameObject SoundPage;

    public Button AccessibilityHandle;
    public GameObject AccessibilityPage;

    private void Start()
    {
        ScreenHandle.onClick.AddListener(SwitchToScreen);
        SoundHandle.onClick.AddListener(SwitchToSound);
        AccessibilityHandle.onClick.AddListener(SwitchToAccesibility);

        SwitchToScreen();
    }

    private void SwitchToScreen()
    {
        SoundPage.SetActive(false);
        AccessibilityPage.SetActive(false);

        ScreenPage.SetActive(true);
    }
    private void SwitchToSound()
    {
        ScreenPage.SetActive(false);
        AccessibilityPage.SetActive(false);

        SoundPage.SetActive(true);
    }
    private void SwitchToAccesibility()
    {
        ScreenPage.SetActive(false);
        SoundPage.SetActive(false);

        AccessibilityPage.SetActive(true);
    }
}
