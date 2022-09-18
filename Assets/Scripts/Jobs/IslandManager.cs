﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class IslandManager : MonoBehaviour
{
    [SerializeField]
    Transform startingPosition;
    [SerializeField]
    Transform[] islandPositions;
    [SerializeField]
    Transform[] rockPositions;

    [SerializeField]
    GameObject[] islandPrefabs;
    [SerializeField]
    GameObject[] rockPrefabs;

    Transform player;
    Stats stats;
    
    List<Island> islands;
    Island deliveryTarget;
    float deliveryStartTime;
    
    enum ObstacleType { Rock, Whirlpool, Pirates }

    void Start()
    {
        stats = new Stats(true);
        
        player = GameObject.FindWithTag("Player").transform;
        player.position = startingPosition.position;

        islands = new List<Island>();

        foreach (Transform i in islandPositions)
        {
            CreateIsland(i);
        }
        
        foreach (Transform j in rockPositions)
        {
            CreateObstacle(ObstacleType.Rock, j);
        }
        
        // TODO: spawn other obstacles
    }

    Island CreateIsland(Transform trans)
    {
        GameObject temp = Instantiate(
            islandPrefabs[Random.Range(0, islandPrefabs.Length)],
            trans.position,
            Quaternion.Euler(Vector3.up * Random.Range(0f, 360f))
        );
            
        temp.transform.SetParent(trans.parent);
        Destroy(trans.gameObject);
        
        Island tempIsland = new Island(
            trans.position,
            Random.Range(1, 15),
            Random.Range(0, 60) + 60 // TODO: this should be chosen based (somewhat) on current player distance to the island
        );

        islands.Add(tempIsland);
        
        return tempIsland;
    }

    void CreateObstacle(ObstacleType type, Transform trans)
    {
        GameObject[] prefabArray;
        
        switch (type)
        {
            case ObstacleType.Rock:
                prefabArray = rockPrefabs;
                break;
            default:
                prefabArray = null;
                break;
        }

        if (prefabArray != null)
        {
            GameObject temp = Instantiate(
                prefabArray[Random.Range(0, prefabArray.Length)],
                trans.position,
                Quaternion.Euler(Vector3.up * Random.Range(0f, 360f))
            );
            
            temp.transform.SetParent(trans.parent);
            Destroy(trans.gameObject);
        }
    }
    
    void ChooseTarget()
    {
        deliveryTarget = islands[Random.Range(0, islands.Count)]; // TODO: this should be more likely to choose a closer target than a far one (or follow a preset path)
        deliveryStartTime = Time.time;
    }

    void CompleteDelivery()
    {
        stats.Money += deliveryTarget.DeliveryTime - (Time.time - deliveryStartTime); // TODO: this may want to be tweaked still
        stats.JobCompletionTimes.Add(Time.time - deliveryStartTime);
        stats.JobMaxTimes.Add(deliveryTarget.DeliveryTime);
        stats.WriteToFile();
        
        islands.Remove(deliveryTarget);
        
        deliveryTarget = null;
        deliveryStartTime = 0;
        
        // TODO: win state
        
        Reset();
    }

    void FailDelivery()
    {
        stats.Strikes++;
        stats.WriteToFile();

        if (stats.Strikes >= 3)
        {
            // TODO: lose state
        }
        else
        {
            Reset();   
        }
    }

    void Reset()
    {
        deliveryTarget = null;
        deliveryStartTime = 0;
        
        ChooseTarget(); // TODO: this may want to be delayed
    }
}