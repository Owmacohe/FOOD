using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuInteraction : MonoBehaviour
{
    [SerializeField]
    TMP_Text instructionText1;
    [SerializeField]
    TMP_Text instructionText2;
    
    GameObject[] menuItems;
    int currentItem;
    
    void Start()
    {
        menuItems = GameObject.FindGameObjectsWithTag("MenuItem");

        if (instructionText1 != null && instructionText2 != null)
        {
            StatsAndOptionsManager manager = FindObjectOfType<StatsAndOptionsManager>();
            
            instructionText1.text = "Press " + manager.options.CurrentKey + " to switch selection >";
            instructionText2.text = "Hold " + manager.options.CurrentKey + " to confirm selection";
        }

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
        SceneChanger temp = menuItems[currentItem].GetComponent<SceneChanger>();
        
        if (temp != null)
        {
            if (temp.isExit)
            {
                Application.Quit(0);
            }
            
            temp.Change(temp.target);
        }
    }
}