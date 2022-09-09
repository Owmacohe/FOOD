using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class IslandManager : MonoBehaviour
{
    List<Island> islands;
    
    Island deliveryTarget;
    float deliveryStartTime;
    
    float funds; // TODO: this may want to be moved to another script
    int strikes;

    Island CreateIsland(Vector3 pos)
    {
        Island temp = new Island(
            pos,
            Random.Range(1, 15),
            Random.Range(0, 60) + 60 // TODO: this should be chosen based (somewhat) on current player distance to the island
        );

        islands.Add(temp);
        
        return temp;
    }
    
    void ChooseTarget()
    {
        deliveryTarget = islands[Random.Range(0, islands.Count)]; // TODO: this should be more likely to choose a closer target than a far one (or follow a preset path)
        deliveryStartTime = Time.time;
    }

    void CompleteDelivery()
    {
        funds += deliveryTarget.DeliveryTime - (Time.time - deliveryStartTime); // TODO: this may want to be tweaked still
        islands.Remove(deliveryTarget);
        
        deliveryTarget = null;
        deliveryStartTime = 0;
        
        // TODO: add win condition?
        
        Reset();
    }

    void FailDelivery()
    {
        strikes++;

        if (strikes >= 3)
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