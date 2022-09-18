using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Stats
{
    public float Money { get; set; }
    public int Strikes { get; set; }
    public List<float> JobMaxTimes { get; set; }
    public List<float> JobCompletionTimes { get; set; }
    
    string path = Application.dataPath + "/Scripts/Jobs/";
    
    public Stats(bool fromFile)
    {
        if (fromFile)
        {
            string[] lines = File.ReadAllLines(path + "player_stats.txt");

            if (lines.Length == 4)
            {
                Money = float.Parse(lines[0]);
                Strikes = int.Parse(lines[1]);
                JobMaxTimes = new List<float>();
                JobCompletionTimes = new List<float>();

                if (lines[2].Length > 0)
                {
                    foreach (string i in lines[2].Split(','))
                    {
                        JobMaxTimes.Add(float.Parse(i));
                    }   
                }

                if (lines[3].Length > 0)
                {
                    foreach (string j in lines[3].Split(','))
                    {
                        JobCompletionTimes.Add(float.Parse(j));
                    }
                }

                return;
            }
        }

        Money = 0;
        Strikes = 0;
        JobMaxTimes = new List<float>();
        JobCompletionTimes = new List<float>();
        
        WriteToFile();
    }
    
    public void WriteToFile()
    {
        File.WriteAllText(path + "player_stats.txt", ToString());
    }
    
    public string ToString()
    {
        string jobMaxTimes = "";
        string jobCompletionTimes = "";

        foreach (float i in JobMaxTimes)
        {
            jobMaxTimes += i + ",";
        }

        if (jobMaxTimes.Length > 0)
        {
            jobMaxTimes = jobMaxTimes.Substring(0, jobMaxTimes.Length - 1);
        }
        
        foreach (float j in JobCompletionTimes)
        {
            jobCompletionTimes += j + ",";
        }
        
        if (jobCompletionTimes.Length > 0)
        {
            jobCompletionTimes = jobCompletionTimes.Substring(0, jobCompletionTimes.Length - 1);
        }

        return
            Money
            + "\n" + Strikes
            + "\n" + jobMaxTimes
            + "\n" + jobCompletionTimes;
    }
}