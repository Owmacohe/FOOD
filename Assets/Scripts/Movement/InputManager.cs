using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    Options playerOptions;
    DirectionController direction;

    bool isDown;
    string currentKeyName;

    void Start()
    {
        playerOptions = new Options(true);
        direction = GetComponent<DirectionController>();
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
        if (currentKeyName.Equals(playerOptions.CurrentKey))
        {
            if (!isDown)
            {
                direction.SetDirection();
            }
            else
            {
                direction.SetForce();
            }
        
            isDown = !isDown;   
        }
    }

    /*
    void OnLong()
    {
        print("long");
    }
    */
}