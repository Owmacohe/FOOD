using System;
using System.IO;
using UnityEngine;

public class Options
{
    public bool IsMuted { get; set; }
    public float MasterVolume { get; set; }
    public float MusicVolume { get; set; }
    public float SoundEffectVolume { get; set; }
    public string CurrentKey { get; set; }
    
    string path = Application.persistentDataPath + "/Scripts/Options/";

    public Options(bool fromFile)
    {
        if (fromFile)
        {
            string[] lines = File.ReadAllLines(path + "player_options");
            
            IsMuted = Convert.ToBoolean(int.Parse(lines[0]));
            MasterVolume = float.Parse(lines[1]);
            MusicVolume = float.Parse(lines[2]);
            SoundEffectVolume = float.Parse(lines[3]);
            CurrentKey = lines[4];
        }
        else
        {
            IsMuted = false;
            MasterVolume = 0.5f;
            MusicVolume = 0.5f;
            SoundEffectVolume = 0.5f;
            CurrentKey = KeyCode.Space.ToString();   
        }
    }

    public void WriteToFile()
    {
        File.WriteAllText(path + "player_options", 
            Convert.ToInt32(IsMuted)
            + "\n" + MasterVolume
            + "\n" + MusicVolume
            + "\n" + SoundEffectVolume
            + "\n" + CurrentKey
        );
    }
}