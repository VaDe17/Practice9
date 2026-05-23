using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic; // Include this if using TextMeshPro Dropdowns

public class ScreenSettingsManager : MonoBehaviour
{
    // UI References
    [Header("UI References")]
    public TMP_Dropdown resolutionDropdown; // Or Dropdown
    public TMP_Dropdown fullscreenModeDropdown; // Or Dropdown
    public TMP_Dropdown fpsDropdown; // Or Slider

    public Toggle vSyncMode;
    public Toggle screenShakeMode;

    private const string VSYNC_KEY = "VSyncEnabled";

    // Data
    private Resolution[] resolutions;
    private FullScreenMode[] fullscreenModes = {
        FullScreenMode.ExclusiveFullScreen,
        FullScreenMode.FullScreenWindow,
        FullScreenMode.MaximizedWindow,
        FullScreenMode.Windowed
    };
    
    private int[] fpsOptions = { 30, 60, 90, 120, 144, 180, -1 }; // -1 represents unlimited
    private string[] fpsLabels = { "30 FPS", "60 FPS", "90 FPS", "120 FPS", "144 FPS", "180 FPS", "Unlimited" };

    void Start()
    {
        // Load saved settings
        LoadSettings();

        // Setup Resolution Dropdown
        SetupResolutionDropdown();

        // Setup Fullscreen Mode Dropdown
        SetupFullscreenDropdown();

        // Setup FPS Dropdown
        SetupFPSDropdown();

        bool isVSyncOn = PlayerPrefs.GetInt(VSYNC_KEY, 1) == 1;
        SetVSync(isVSyncOn);
        vSyncMode.isOn = isVSyncOn;
    }

    void SetupResolutionDropdown()
    {
        // Get all supported resolutions
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        int currentResolutionIndex = 0;
        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " @ " + resolutions[i].refreshRateRatio + "Hz";
            options.Add(option);

            // Check if this is our current resolution
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height &&
                resolutions[i].refreshRateRatio.Equals(Screen.currentResolution.refreshRateRatio))
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // Add listener
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    void SetupFullscreenDropdown()
    {
        fullscreenModeDropdown.ClearOptions();
        List<string> options = new List<string>();

        foreach (FullScreenMode mode in fullscreenModes)
        {
            options.Add(mode.ToString());
        }

        fullscreenModeDropdown.AddOptions(options);
        fullscreenModeDropdown.value = (int)Screen.fullScreenMode;
        fullscreenModeDropdown.RefreshShownValue();

        // Add listener
        fullscreenModeDropdown.onValueChanged.AddListener(SetFullscreenMode);
    }

    void SetupFPSDropdown()
    {
        fpsDropdown.ClearOptions();
        List<string> options = new List<string>();

        foreach (string label in fpsLabels)
        {
            options.Add(label);
        }

        fpsDropdown.AddOptions(options);

        // Find current FPS setting index
        int currentFPS = (QualitySettings.vSyncCount > 0) ? -2 : Application.targetFrameRate; // -2 indicates VSync is ON
        int currentIndex = 0;

        if (QualitySettings.vSyncCount > 0)
        {
            // If VSync is on, we can't get the exact FPS from targetFrameRate. We'll set a default index or handle it.
            // For simplicity, we'll check if targetFrameRate matches any option.
            currentIndex = System.Array.IndexOf(fpsOptions, Application.targetFrameRate);
            if (currentIndex == -1) currentIndex = 5; // Default to Unlimited if not found
        }
        else
        {
            currentIndex = System.Array.IndexOf(fpsOptions, Application.targetFrameRate);
            if (currentIndex == -1) currentIndex = 5; // Default to Unlimited if not found
        }

        fpsDropdown.value = currentIndex;
        fpsDropdown.RefreshShownValue();

        // Add listener
        fpsDropdown.onValueChanged.AddListener(SetFPS);
    }

    // --- Setting Functions ---

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode, resolution.refreshRateRatio);
        // Save the setting
        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
        PlayerPrefs.Save();
    }

    public void SetFullscreenMode(int modeIndex)
    {
        Screen.fullScreenMode = fullscreenModes[modeIndex];
        PlayerPrefs.SetInt("FullscreenModeIndex", modeIndex);
        PlayerPrefs.Save();
    }

    public void SetFPS(int fpsIndex)
    {
        int targetFPS = fpsOptions[fpsIndex];
        
        if (targetFPS == -1) // Unlimited
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = -1;
        }
        else
        {
            // When using targetFrameRate, VSync must be disabled
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = targetFPS;
        }
        
        PlayerPrefs.SetInt("FPSIndex", fpsIndex);
        PlayerPrefs.Save();
    }

    public void SetVSync(bool isOn)
    {
        QualitySettings.vSyncCount = isOn ? 1 : 0; // 1 = VSync On, 0 = VSync Off[reference:1]
        PlayerPrefs.SetInt(VSYNC_KEY, isOn ? 1 : 0);
        PlayerPrefs.Save();

         // --- Disable FPS dropdown when VSync is on ---
        fpsDropdown.interactable = !isOn;
        
        // If VSync is turned ON, set the in-memory targetFrameRate to -1 (no limit)
        // so it doesn't conflict when VSync is turned off again.
        if (isOn)
        {
            Application.targetFrameRate = -1;
        }
        else
        {
            // Re-apply the saved FPS value from your dropdown
            // This depends on how you implemented your FPS setting.
            // You might call your existing SetFPS() function here.
            SetFPS(fpsDropdown.value);
        }
    }

    // --- Saving and Loading ---

    void LoadSettings()
    {
        // Load Resolution
        if (PlayerPrefs.HasKey("ResolutionIndex"))
        {
            // Wait until after Start() to apply, after the dropdown is setup
            // We'll apply it after the UI is ready
        }

        // Load Fullscreen Mode
        if (PlayerPrefs.HasKey("FullscreenModeIndex"))
        {
            int modeIndex = PlayerPrefs.GetInt("FullscreenModeIndex");
            Screen.fullScreenMode = fullscreenModes[modeIndex];
        }

        // Load FPS
        if (PlayerPrefs.HasKey("FPSIndex"))
        {
            int fpsIndex = PlayerPrefs.GetInt("FPSIndex");
            int targetFPS = fpsOptions[fpsIndex];
            if (targetFPS == -1)
            {
                QualitySettings.vSyncCount = 0;
                Application.targetFrameRate = -1;
            }
            else
            {
                QualitySettings.vSyncCount = 0;
                Application.targetFrameRate = targetFPS;
            }
        }
    }
}