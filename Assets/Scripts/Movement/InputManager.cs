using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public bool inputPaused;

    [SerializeField]
    bool isMenu;
    
    Options playerOptions;
    DirectionController direction;
    MenuInteraction menu;

    bool isDown;
    string currentKeyName;

    void Start()
    {
        playerOptions = new Options(true);

        if (!isMenu)
        {
            direction = GetComponent<DirectionController>();
            FindObjectOfType<UIManager>().SetMoveButtonKey(playerOptions.CurrentKey);   
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
        if (!inputPaused && currentKeyName.Equals(playerOptions.CurrentKey))
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
        if (!inputPaused && currentKeyName.Equals(playerOptions.CurrentKey))
        {
            if (isMenu)
            {
                
            }
        }
    }
}