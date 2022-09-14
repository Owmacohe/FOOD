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
        playerOptions.WriteToFile();
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
    }
    
    public void SetMusicVolume()
    {
        playerOptions.MusicVolume = musicVolume;
    }
    
    public void SetSoundEffectVolume()
    {
        playerOptions.SoundEffectVolume = soundEffectVolume;
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