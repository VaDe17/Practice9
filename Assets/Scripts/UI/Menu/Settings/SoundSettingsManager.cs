using System;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettingsManager : MonoBehaviour
{
    // --- Reference to the Audio Mixer ---
    public AudioMixer gameAudioMixer;

    // --- UI References ---
    [Header("Volume Sliders")]
    public Slider masterVolumeSlider;
    public TMP_Text masterVolumeValueText;

    public Slider musicVolumeSlider;
    public TMP_Text musicVolumeValueText;

    public Slider sfxVolumeSlider;
    public TMP_Text sfxVolumeValueText;

    public Slider uiVolumeSlider;
    public TMP_Text uiVolumeValueText;

    // --- Keys for Saving Data ---
    private const string MASTER_VOLUME_KEY = "MasterVolume";
    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";
    private const string UI_VOLUME_KEY = "UIVolume";

    private void Start()
    {
        // --- Load and Set Saved Values ---
        int savedMasterVol = PlayerPrefs.GetInt(MASTER_VOLUME_KEY, 75);
        int savedMusicVol = PlayerPrefs.GetInt(MUSIC_VOLUME_KEY, 75);
        int savedSFXVol = PlayerPrefs.GetInt(SFX_VOLUME_KEY, 75);
        int savedUIVol = PlayerPrefs.GetInt(UI_VOLUME_KEY, 75);

        masterVolumeSlider.value = savedMasterVol;
        musicVolumeSlider.value = savedMusicVol;
        sfxVolumeSlider.value = savedSFXVol;
        uiVolumeSlider.value = savedUIVol;

        // Apply loaded values to the audio system and the UI sliders
        SetMasterVolume(savedMasterVol);
        SetMusicVolume(savedMusicVol);
        SetSFXVolume(savedSFXVol);
        SetUIVolume(savedUIVol);
        
        // --- Add Listeners for Live Updates & Saving ---
        masterVolumeSlider.onValueChanged.AddListener(val => { SetMasterVolume(val); SaveVolume(MASTER_VOLUME_KEY, val); });
        musicVolumeSlider.onValueChanged.AddListener(val => { SetMusicVolume(val); SaveVolume(MUSIC_VOLUME_KEY, val); });
        sfxVolumeSlider.onValueChanged.AddListener(val => { SetSFXVolume(val); SaveVolume(SFX_VOLUME_KEY, val); });
        uiVolumeSlider.onValueChanged.AddListener(val => { SetUIVolume(val); SaveVolume(UI_VOLUME_KEY, val); });
    }

    // --- Audio Control Functions ---
    public void SetMasterVolume(float sliderValue)
    {
        SetVolumeForMixer("MasterVolume", sliderValue);
        masterVolumeValueText.text = sliderValue.ToString() + "%";
    }

    public void SetMusicVolume(float sliderValue)
    {
        SetVolumeForMixer("MusicVolume", sliderValue);
        musicVolumeValueText.text = sliderValue.ToString() + "%";
    }

    public void SetSFXVolume(float sliderValue)
    {
        SetVolumeForMixer("SFXVolume", sliderValue);
        sfxVolumeValueText.text = sliderValue.ToString() + "%";
    }
    public void SetUIVolume(float sliderValue)
    {
        SetVolumeForMixer("UIVolume", sliderValue);
        uiVolumeValueText.text = sliderValue.ToString() + "%";
    }

    // A helper function to do the math, reducing repeated code.
    private void SetVolumeForMixer(string exposedParamName, float sliderValue)
    {
        float normalized = Mathf.Clamp01(sliderValue / 100f);
        float decibelValue = Mathf.Log10(Mathf.Max(normalized, 0.0001f)) * 20f;
        gameAudioMixer.SetFloat(exposedParamName, decibelValue);
    }

    // --- Saving Function ---
    // Called whenever a slider is moved.
    private void SaveVolume(string key, float value)
    {
        int intValue = Mathf.RoundToInt(value);
        PlayerPrefs.SetInt(key, intValue);

        // PlayerPrefs.Save();
    }
}
