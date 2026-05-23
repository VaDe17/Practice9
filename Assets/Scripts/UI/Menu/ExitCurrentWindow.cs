using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ExitCurrentWindow : MonoBehaviour
{
    public RectTransform Settings;
    public RectTransform Credits;

    public void ExitWindow()
    {
        Settings.gameObject.SetActive(false);
        Credits.gameObject.SetActive(false);
    }

    public void OpenWindow()
    {
        switch (MenuStateManager.Instance.GetCurrentState)
        {
            case MenuState.Settings:
                Settings.gameObject.SetActive(true);
                break;
            case MenuState.Credits:
                Credits.gameObject.SetActive(true);
                break;
            default:
                return;
        }
    }
}
