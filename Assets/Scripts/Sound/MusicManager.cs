using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField]
    AudioClip clip;
    [Range(0, 1)]
    public float volume = 0.5f;
    
    AudioSource source;
    Options playerOptions;
    
    void Start()
    {
        playerOptions = new Options(true);
        
        source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.loop = true;
        source.clip = clip;
        source.volume = volume * playerOptions.MasterVolume;
        
        source.Play();
    }

    public void ResetAudio()
    {
        playerOptions = new Options(true);
        source.volume = volume * playerOptions.MasterVolume;
    }
}