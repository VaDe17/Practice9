using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class SoundEffect
{
    public string Name;

    public AudioClip clip;
    public bool loop = false;

    [Range(0f, 1f)]
    public float volume = 0.7f;
    [Range(0f, 1f)]
	public float volumeVariance = 0f;

	[Range(.1f, 3f)]
	public float pitch = 1f;
	[Range(0f, 1f)]
	public float pitchVariance = 0f;

	public AudioMixerGroup mixerGroup;

	[HideInInspector]
	public AudioSource source;
}
