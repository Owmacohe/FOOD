using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public bool inputPaused;
    
    Options playerOptions;
    DirectionController direction;

    bool isDown;
    string currentKeyName;

    void Start()
    {
        playerOptions = new Options(true);
        direction = GetComponent<DirectionController>();
        
        FindObjectOfType<UIManager>().SetMoveButtonKey(playerOptions.CurrentKey);
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