using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField]
    AudioClip clip;
    [Range(0, 1)]
    public float volume = 0.5f;
    
    AudioSource source;
    StatsAndOptionsManager manager;
    
    void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.loop = true;
        source.clip = clip;
        source.volume = volume;

        manager = FindObjectOfType<StatsAndOptionsManager>();
        
        Invoke(nameof(ResetAudio), 0.1f);
        
        source.Play();
    }

    public void ResetAudio()
    {
        source.volume = volume * (manager.options.MasterVolume * 2f);
    }
}