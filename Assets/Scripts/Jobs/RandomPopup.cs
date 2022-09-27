using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RandomPopup : MonoBehaviour
{
    [SerializeField]
    Sprite[] positivePopups, negativePopups;
    [SerializeField, Range(0, 1)]
    float popupChance = 0.02f;

    IslandManager manager;
    Image img;
    bool hasShownPositive, hasShownNegative;

    void Start()
    {
        manager = FindObjectOfType<IslandManager>();
        img = GetComponent<Image>();
    }

    void FixedUpdate()
    {
        if (manager.hasStartedTimer && Random.Range(0f, 1f) <= popupChance)
        {
            if (!hasShownPositive && !manager.IsHalfTime())
            {
                img.enabled = true;
                img.sprite = positivePopups[Random.Range(0, positivePopups.Length)];
                hasShownPositive = true;
                
                Invoke(nameof(HidePopup), 5);
            }
            else if (!hasShownNegative && manager.IsHalfTime())
            {
                img.enabled = true;
                img.sprite = negativePopups[Random.Range(0, negativePopups.Length)];
                hasShownNegative = true;
                
                Invoke(nameof(HidePopup), 5);
            }
        }
    }

    void HidePopup()
    {
        img.enabled = false;
    }
}