using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private SoundLibrary soundLibrary;
    
    private void Start()
    {
        soundLibrary.PlaySoundEffect("Music_1", false);
    }
}
