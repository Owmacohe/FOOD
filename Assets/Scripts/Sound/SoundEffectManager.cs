using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundEffectManager : MonoBehaviour
{
    [SerializeField]
    AudioClip[] clips;
    [SerializeField, Range(0, 1)]
    float volume = 0.5f;
    [SerializeField]
    bool changePitch;
    [SerializeField]
    bool makeSoundsRandomly;
    [SerializeField]
    int randomChance = 100;
    
    AudioSource source;
    AudioClip lastPlayed;
    
    void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
    }

    void FixedUpdate()
    {
        if (makeSoundsRandomly && Random.Range(0, randomChance) == 0 && !source.isPlaying)
        {
            Play();
        }
    }

    public void Play()
    {
        source.clip = clips[Random.Range(0, clips.Length)];

        if (clips.Length > 1)
        {
            while (source.clip.Equals(lastPlayed))
            {
                source.clip = clips[Random.Range(0, clips.Length)];
            }
        }
        
        source.volume = volume;

        if (changePitch)
        {
            source.pitch = 1 + Random.Range(-0.5f, 0.5f);
        }
        
        source.Play();
        
        lastPlayed = source.clip;
    }
}