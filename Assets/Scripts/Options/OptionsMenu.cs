using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField]
    TMP_Text interactionKey;
    [SerializeField]
    TMP_Text interactionKeyButton;
    [SerializeField]
    TMP_Text isMutedButton;
    [SerializeField]
    Slider masterVolumeSlider;
    [SerializeField]
    Slider musicVolumeSlider;
    [SerializeField]
    Slider soundEffectVolumeSlider;

    StatsAndOptionsManager manager;

    bool isListening;
    float masterVolume, musicVolume, soundEffectVolume;

    void Start()
    {
        manager = FindObjectOfType<StatsAndOptionsManager>();
        ResetUI(true);
    }

    void Update()
    {
        if (Time.time > 2)
        {
            masterVolume = masterVolumeSlider.value;
            musicVolume = musicVolumeSlider.value;
            soundEffectVolume = soundEffectVolumeSlider.value;   
        }
    }

    void ResetUI(bool isFromStart = false)
    {
        Options temp = manager.options;
        
        interactionKey.text = "Interaction Key: " + temp.CurrentKey;
        interactionKeyButton.text = "> listen for new key <";
        isMutedButton.text = "Is muted: " + temp.IsMuted;

        if (isFromStart)
        {
            masterVolumeSlider.value = temp.MasterVolume; 
            musicVolumeSlider.value = temp.MusicVolume;
            soundEffectVolumeSlider.value = temp.SoundEffectVolume; 
        }
    }

    public void Save()
    {
        //manager.options.WriteToFile();
    }
    
    public void ListenForNewKey()
    {
        isListening = true;
        interactionKeyButton.text = "listening...";
    }

    public void ToggleIsMuted()
    {
        manager.options.IsMuted = !manager.options.IsMuted;
        ResetUI();
    }

    public void SetMasterVolume()
    {
        if (masterVolume > 0)
        {
            manager.options.MasterVolume = masterVolume;
            Save();
            ResetAudioSources(); 
        }
    }
    
    public void SetMusicVolume()
    {
        manager.options.MusicVolume = musicVolume;
        Save();
        ResetAudioSources();
    }
    
    public void SetSoundEffectVolume()
    {
        manager.options.SoundEffectVolume = soundEffectVolume;
        Save();
        ResetAudioSources();
    }

    void ResetAudioSources()
    {
        foreach (MusicManager i in FindObjectsOfType<MusicManager>())
        {
            i.ResetAudio();
        }
        
        foreach (SoundEffectManager j in FindObjectsOfType<SoundEffectManager>())
        {
            j.ResetAudio();
        }
    }

    void OnGUI()
    {
        if (isListening)
        {
            Event e = Event.current;
        
            if (e.isKey && e.keyCode != KeyCode.None)
            {
                manager.options.CurrentKey = e.keyCode.ToString();
                ResetUI();
            }
        }
    }
}