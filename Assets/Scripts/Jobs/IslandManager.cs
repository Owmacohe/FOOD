using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class IslandManager : MonoBehaviour
{
    [SerializeField]
    Transform targetArrow;
    
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

    [SerializeField]
    GameObject endStateCanvas;
    [SerializeField]
    Image endStateImage;
    [SerializeField]
    TMP_Text endStateText;
    [SerializeField]
    Sprite winState, loseState;

    Transform player;
    Stats stats;
    UIManager UI;
    InputManager input;
    DirectionController direction;
    
    List<Island> islands;
    Island deliveryTarget;
    float deliveryStartTime;
    bool hasJustFailedDelivery;
    
    enum ObstacleType { Rock, Whirlpool, Pirates }

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        player.position = startingPosition.position;
        stats = new Stats(true);
        UI = FindObjectOfType<UIManager>();
        input = FindObjectOfType<InputManager>();
        direction = FindObjectOfType<DirectionController>();
        
        UI.SetMoney(stats.Money);
        // TODO: set UI highscore here too
        
        endStateCanvas.SetActive(false);

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
        
        ChooseTarget();
    }

    void FixedUpdate()
    {
        if (deliveryTarget != null)
        {
            targetArrow.LookAt(deliveryTarget.Object.transform);
            
            UI.SetTimer(deliveryTarget.DeliveryTime - (Time.time - deliveryStartTime));

            if (!hasJustFailedDelivery && (Time.time - deliveryStartTime) > deliveryTarget.DeliveryTime)
            {
                FailDelivery();
            }
        }
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
            temp,
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
        
        print("Target position: " + deliveryTarget.Object.transform.position);
    }

    public void CompleteDelivery()
    {
        stats.Money += Mathf.RoundToInt((deliveryTarget.DeliveryTime - (Time.time - deliveryStartTime)) * 100f) / 100f; // TODO: this may want to be tweaked still
        stats.JobCompletionTimes.Add(Time.time - deliveryStartTime);
        stats.JobMaxTimes.Add(deliveryTarget.DeliveryTime);
        stats.WriteToFile();
        
        UI.SetMoney(stats.Money);
        
        deliveryTarget = null;
        deliveryStartTime = 0;
        
        islands.Remove(deliveryTarget);

        if (islands.Count > 0)
        {
            input.inputPaused = true;
            SetEndState(true);
            
            Invoke(nameof(Reset), 3);
        }
    }

    void FailDelivery()
    {
        print("fail");
        
        hasJustFailedDelivery = true;
        
        stats.Strikes++;
        stats.WriteToFile();
        
        UI.SetStrikes(stats.Strikes);
        
        input.inputPaused = true;
        SetEndState(false);
            
        Invoke(nameof(Reset), 3);

        if (stats.Strikes >= 3)
        {
            // TODO: what happens after 3 strikes?
        }
    }

    void Reset()
    {
        hasJustFailedDelivery = false;
        
        input.inputPaused = false;
        endStateCanvas.SetActive(false);
        
        // TODO: this reset needs to be MUCH more graceful
        
        player.position = startingPosition.position;

        deliveryTarget = null;
        deliveryStartTime = 0;

        StartCoroutine(direction.FadeArrow(false));
        
        ChooseTarget();
    }

    public bool IsDeliveryTarget(GameObject island)
    {
        return deliveryTarget.Object.Equals(island);
    }

    void SetEndState(bool isWin)
    {
        endStateCanvas.SetActive(true);
        
        if (isWin)
        {
            endStateImage.sprite = winState;
            endStateText.text = "Yay! Delivery on time. You're a superstar!";
        }
        else
        {
            endStateImage.sprite = loseState;
            endStateText.text = "Boo! All the food has spoiled. You ran out of time!";
        }
    }
}