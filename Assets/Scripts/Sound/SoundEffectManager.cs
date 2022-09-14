using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundEffectManager : MonoBehaviour
{
    [SerializeField]
    AudioClip[] clips;
    [SerializeField]
    bool makeSoundsRandomly;
    [SerializeField]
    float randomChance = 100;
    
    AudioSource source;
    
    void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
    }

    void FixedUpdate()
    {
        if (makeSoundsRandomly && Random.Range(0, randomChance) == 0)
        {
            Play();
        }
    }

    public void Play()
    {
        source.clip = clips[Random.Range(0, clips.Length)];
        source.pitch = 1 + Random.Range(-0.5f, 0.5f);
        source.Play();
    }
}