using System;
using TMPro;
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

    Options playerOptions;

    bool isListening;
    float masterVolume, musicVolume, soundEffectVolume;

    void Start()
    {
        playerOptions = new Options(true);
        ResetUI(true);
    }

    void FixedUpdate()
    {
        masterVolume = masterVolumeSlider.value;
        musicVolume = musicVolumeSlider.value;
        soundEffectVolume = soundEffectVolumeSlider.value;
    }

    void ResetUI(bool isFromStart = false)
    {
        interactionKey.text = "Interaction Key: " + playerOptions.CurrentKey;
        interactionKeyButton.text = "> listen for new key <";
        isMutedButton.text = "Is muted: " + playerOptions.IsMuted;

        if (isFromStart)
        {
            masterVolumeSlider.value = playerOptions.MasterVolume; 
            musicVolumeSlider.value = playerOptions.MusicVolume;
            soundEffectVolumeSlider.value = playerOptions.SoundEffectVolume; 
        }
    }

    public void Save()
    {
        if (Time.time < 2)
        {
            playerOptions.WriteToFile();   
        }
    }
    
    public void ListenForNewKey()
    {
        isListening = true;
        interactionKeyButton.text = "listening...";
    }

    public void ToggleIsMuted()
    {
        playerOptions.IsMuted = !playerOptions.IsMuted;
        ResetUI();
    }

    public void SetMasterVolume()
    {
        playerOptions.MasterVolume = masterVolume;
        Save();
        ResetAudioSources();
    }
    
    public void SetMusicVolume()
    {
        playerOptions.MusicVolume = musicVolume;
    }
    
    public void SetSoundEffectVolume()
    {
        playerOptions.SoundEffectVolume = soundEffectVolume;
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
                playerOptions.CurrentKey = e.keyCode.ToString();
                ResetUI();
            }
        }
    }
}