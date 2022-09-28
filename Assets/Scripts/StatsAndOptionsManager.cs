using System;
using UnityEngine;

public class StatsAndOptionsManager : MonoBehaviour
{
    public Options options;
    public Stats stats;

    void Start()
    {
        options = new Options(false);
        stats = new Stats(false);
    }
}