using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField]
    AudioClip clip;
    
    AudioSource source;
    
    void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.loop = true;
        source.clip = clip;
        
        source.Play();
    }
}