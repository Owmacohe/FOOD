using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuInteraction : MonoBehaviour
{
    GameObject[] menuItems;
    int currentItem;
    
    void Start()
    {
        menuItems = GameObject.FindGameObjectsWithTag("MenuItem");

        foreach (GameObject i in menuItems)
        {
            Outline outline = i.AddComponent<Outline>();

            outline.effectColor = new Color(0.6f, 0.4f, 0.3f);
            outline.effectDistance = Vector2.one * 8;

            outline.enabled = false;
        }
        
        SetCurrentItem(0);
    }

    public void SetCurrentItem(int current)
    {
        menuItems[currentItem].GetComponent<Outline>().enabled = true;
    }

    public void GoToNextItem()
    {
        menuItems[currentItem].GetComponent<Outline>().enabled = false;
        
        currentItem++;

        if (currentItem > menuItems.Length - 1)
        {
            currentItem = 0;
        }

        SetCurrentItem(currentItem);
    }

    public void ActivateItem()
    {
        
    }
}