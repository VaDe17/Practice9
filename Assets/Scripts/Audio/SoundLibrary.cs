using System;
using UnityEngine;
using UnityEngine.Audio;

public class SoundLibrary : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup mixerGroup;
    [SerializeField] private SoundEffect[] soundEffects;
    private void Awake()
    {
        foreach (var soundEffect in soundEffects)
        {
            soundEffect.source = gameObject.AddComponent<AudioSource>();
			soundEffect.source.clip = soundEffect.clip;
			soundEffect.source.loop = soundEffect.loop;

			soundEffect.source.outputAudioMixerGroup = mixerGroup;

            soundEffect.source.playOnAwake = false;
        }
    }

    public void PlaySoundEffect(string soundName, bool isOneShot)
    {
        // Check if Sounds and Music are enabled | need the AudioService Script

        SoundEffect sfx = Array.Find(soundEffects, item => item.Name == soundName);
        if(sfx == null)
        {
            Debug.LogError($"Sound Effect not found for {transform.parent.name}. SFX = {soundName}.");
            return;
        }

        sfx.source.volume = sfx.volume * (1f + UnityEngine.Random.Range(-sfx.volumeVariance / 2f, sfx.volumeVariance / 2f));
        sfx.source.pitch = sfx.pitch * (1f + UnityEngine.Random.Range(-sfx.pitchVariance / 2f, sfx.pitchVariance / 2f));

        if(!isOneShot)
            sfx.source.Play();
        else 
            sfx.source.PlayOneShot(sfx.source.clip);
    }
    public void PlaySoundEffectWithCustomPitch(string soundName, float startPoint, float endPoint, float value, float pitchMultiplier)
    {
        // Check if Sounds and Music are enabled | need the AudioService Script

        SoundEffect sfx = Array.Find(soundEffects, item => item.Name == soundName);
        if(sfx == null)
        {
            Debug.LogError($"Sound Effect not found for {transform.parent.name}. SFX = {soundName}.");
            return;
        }

        sfx.source.volume = sfx.volume * (1f + UnityEngine.Random.Range(-sfx.volumeVariance / 2f, sfx.volumeVariance / 2f));

        float t = Mathf.Lerp(startPoint, endPoint, value / endPoint);
        sfx.source.pitch = sfx.pitch + (pitchMultiplier * t) * (1f + UnityEngine.Random.Range(-sfx.pitchVariance / 2f, sfx.pitchVariance / 2f));

        sfx.source.PlayOneShot(sfx.source.clip);
    }
}
