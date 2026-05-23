using System;
using UnityEngine;

public class DebugMode : MonoBehaviour
{
    public static DebugMode Instance {get; private set;}
    public GameObject DEBUG_UI_WINDOW;

    private bool isDebugModeActive = true;
    public bool IsDebugModeActive
    {
        get => isDebugModeActive;
        set
        {
            if(isDebugModeActive != value)
            {
                isDebugModeActive = value;
                OnDebugModeChanged?.Invoke(value);
            }
        }
    }

    public Action<bool> OnDebugModeChanged;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        OnDebugModeChanged += ToggleDebugUI;
        OnDebugModeChanged?.Invoke(IsDebugModeActive);

    }
    private void OnDisable()
    {
        OnDebugModeChanged -= ToggleDebugUI;
    }

    private void ToggleDebugUI(bool enabled)
    {
        DEBUG_UI_WINDOW.SetActive(enabled);
    }
}
