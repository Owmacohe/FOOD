using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    DirectionController direction;

    bool isDown;
    string currentKeyName;

    void Start()
    {
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
        // TODO: check if currentKeyName equals the Option object's CurrentKey
        
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

    /*
    void OnLong()
    {
        print("long");
    }
    */
}