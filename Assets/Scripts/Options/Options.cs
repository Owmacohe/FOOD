using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Options
{
    public string CurrentKey { get; set; }
    public bool IsMuted { get; set; }
    public float MasterVolume { get; set; }
    public float MusicVolume { get; set; }
    public float SoundEffectVolume { get; set; }
    
    string path = Application.dataPath + "/Scripts/Options/";

    public Options(bool fromFile)
    {
        if (fromFile)
        {
            string[] lines = File.ReadAllLines(path + "player_options.txt");

            if (lines.Length != 0)
            {
                CurrentKey = lines[0];
                IsMuted = Convert.ToBoolean(int.Parse(lines[1]));
                MasterVolume = float.Parse(lines[2]);
                MusicVolume = float.Parse(lines[3]);
                SoundEffectVolume = float.Parse(lines[4]);

                return;
            }
        }
        
        CurrentKey = KeyCode.Space.ToString();
        IsMuted = false;
        MasterVolume = 0.5f;
        MusicVolume = 0.5f;
        SoundEffectVolume = 0.5f;
        
        WriteToFile();
        
        Debug.Log(ToString());
    }

    public void WriteToFile()
    {
        File.WriteAllText(path + "player_options.txt",
            CurrentKey
            + "\n" + Convert.ToInt32(IsMuted)
            + "\n" + MasterVolume
            + "\n" + MusicVolume
            + "\n" + SoundEffectVolume
        );
    }

    public string ToString()
    {
        return
            "key: " + CurrentKey
            + ", muted: " + IsMuted
            + ", master volume: " + MasterVolume
            + ", music volume: " + MusicVolume
            + ", sound effect volume: " + SoundEffectVolume;
    }
}