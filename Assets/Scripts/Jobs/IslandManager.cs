using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class IslandManager : MonoBehaviour
{
    [SerializeField]
    bool isTesting;
    [SerializeField]
    bool isInteracting = true;
    [SerializeField]
    string nextScene;
    
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

    [HideInInspector]
    public bool hasStartedTimer;
    float timerOffset;
    
    enum ObstacleType { Rock, Whirlpool, Pirates }

    void Start()
    {
        if (isInteracting)
        {
            player = GameObject.FindWithTag("Player").transform;
            player.position = startingPosition.position;
            UI = FindObjectOfType<UIManager>();
            input = FindObjectOfType<InputManager>();
            direction = FindObjectOfType<DirectionController>();
            
            stats = new Stats(true);
            UI.SetMoney(stats.Money);

            endStateCanvas.SetActive(false);
        }

        islands = new List<Island>();

        foreach (Transform i in islandPositions)
        {
            CreateIsland(i);
        }
        
        foreach (Transform j in rockPositions)
        {
            CreateObstacle(ObstacleType.Rock, j);
        }

        if (isInteracting)
        {
            ChooseTarget();   
        }
        
        timerOffset = 1;
    }

    void FixedUpdate()
    {
        if (isInteracting && deliveryTarget != null)
        {
            targetArrow.LookAt(deliveryTarget.Object.transform);

            if (hasStartedTimer && !hasJustFailedDelivery)
            {
                UI.SetTimer(deliveryTarget.DeliveryTime - (Time.time - deliveryStartTime) * timerOffset);
                    
                if ((Time.time - deliveryStartTime) * timerOffset > deliveryTarget.DeliveryTime)
                {
                    FailDelivery();
                }
            }
        }
    }

    public void StartTimer()
    {
        deliveryStartTime = Time.time;
        hasStartedTimer = true;
    }

    void CreateIsland(Transform trans)
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
            Random.Range(0, 60) + 60
        );

        islands.Add(tempIsland);
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
        deliveryTarget = islands[Random.Range(0, islands.Count)];
        UI.SetTimer(deliveryTarget.DeliveryTime);

        //print("Target position: " + deliveryTarget.Object.transform.position);
    }

    public void CompleteDelivery()
    {
        stats.Money += Mathf.RoundToInt((deliveryTarget.DeliveryTime - (Time.time - deliveryStartTime)) * 100f) / 100f;
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
        hasJustFailedDelivery = true;
        
        stats.Strikes++;
        stats.WriteToFile();
        
        UI.SetStrikes(stats.Strikes);
        
        input.inputPaused = true;
        SetEndState(false);
            
        Invoke(nameof(Reset), 3);
    }

    void Reset()
    {
        input.inputPaused = false;
        endStateCanvas.SetActive(false);
        
        player.position = startingPosition.position;
        
        deliveryStartTime = 0;
        
        StartCoroutine(direction.FadeArrow(false));
        
        if (isTesting)
        {
            deliveryTarget = null;
        
            hasJustFailedDelivery = false;
        
            ChooseTarget();
        }
        else
        {
            if (!hasJustFailedDelivery)
            {
                if (nextScene != "")
                {
                    SceneChanger.StaticChange(nextScene);   
                }
                else
                {
                    SceneChanger.StaticChange("UltimateWin");
                }
            }
            else
            {
                if (stats.Strikes >= 3)
                {
                    SceneChanger.StaticChange("UltimateLose");
                }
                
                StartTimer();
            }
        
            hasJustFailedDelivery = false;
        }
    }

    public bool IsDeliveryTarget(GameObject island)
    {
        if (deliveryTarget != null)
        {
            return deliveryTarget.Object.Equals(island);   
        }

        return false;
    }

    void SetEndState(bool isWin)
    {
        endStateCanvas.SetActive(true);
        
        if (isWin)
        {
            endStateImage.sprite = winState;
        }
        else
        {
            endStateImage.sprite = loseState;
        }
    }

    public bool IsHalfTime()
    {
        return (Time.time - deliveryStartTime) > (deliveryTarget.DeliveryTime / 2f);
    }
}