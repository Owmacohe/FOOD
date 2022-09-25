﻿using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundEffectManager : MonoBehaviour
{
    [SerializeField]
    AudioClip[] clips;
    [Range(0, 1)]
    public float volume = 0.5f;
    [SerializeField]
    bool changePitch;
    [SerializeField]
    bool makeSoundsRandomly;
    [SerializeField]
    int randomChance = 100;
    
    AudioSource source;
    AudioClip lastPlayed;
    Options playerOptions;
    
    void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        
        playerOptions = new Options(true);
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
        
        source.volume = volume * playerOptions.MasterVolume;

        if (changePitch)
        {
            source.pitch = 1 + Random.Range(-0.5f, 0.5f);
        }
        
        source.Play();
        
        lastPlayed = source.clip;
    }
    
    public void ResetAudio()
    {
        playerOptions = new Options(true);
        source.volume = volume * playerOptions.MasterVolume;
    }
}