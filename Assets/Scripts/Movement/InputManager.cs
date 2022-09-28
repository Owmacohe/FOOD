using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public bool inputPaused;

    [SerializeField]
    bool isMenu;
    
    StatsAndOptionsManager manager;
    DirectionController direction;
    MenuInteraction menu;

    bool isDown;
    string currentKeyName;

    void Start()
    {
        manager = FindObjectOfType<StatsAndOptionsManager>();

        if (!isMenu)
        {
            direction = GetComponent<DirectionController>();
            FindObjectOfType<UIManager>().SetMoveButtonKey(manager.options.CurrentKey);   
        }
        else
        {
            menu = GetComponent<MenuInteraction>();
        }
    }
    
    void OnGUI()
    {
        Event e = Event.current;
        
        if (e.isKey && e.keyCode != KeyCode.None)
        {
            currentKeyName = e.keyCode.ToString();
        }
    }

    void OnShort()
    {
        if (!inputPaused && currentKeyName.Trim().Equals(manager.options.CurrentKey.Trim()))
        {
            if (!isMenu)
            {
                if (!isDown)
                {
                    direction.SetDirection();
                }
                else
                {
                    direction.SetForce();
                }
            }
            else
            {
                if (isDown)
                {
                    menu.GoToNextItem();   
                }
            }
            
            isDown = !isDown; 
        }
    }

    void OnLong()
    {
        if (!inputPaused && currentKeyName.Trim().Equals(manager.options.CurrentKey.Trim()))
        {
            if (isMenu)
            {
                menu.ActivateItem();
            }
        }
    }
}