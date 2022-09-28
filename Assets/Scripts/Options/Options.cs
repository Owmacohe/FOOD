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

    public Options(bool fromFile)
    {
        string text = Resources.Load<TextAsset>("player_options").text;
        
        if (fromFile)
        {
            string[] lines = text.Split('\n');

            if (lines.Length == 5)
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
        
        //WriteToFile();
    }

    /*
    public void WriteToFile()
    {
        File.WriteAllText(Application.dataPath + "/Resources/player_options.txt", ToString());
    }
    */

    public string ToString()
    {
        return
            CurrentKey
            + "\n" + Convert.ToInt32(IsMuted)
            + "\n" + MasterVolume
            + "\n" + MusicVolume
            + "\n" + SoundEffectVolume;
    }
}