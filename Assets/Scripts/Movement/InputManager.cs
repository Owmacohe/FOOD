using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    DirectionController direction;

    bool isDown;

    void Start()
    {
        direction = GetComponent<DirectionController>();
    }

    void OnShort()
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

    /*
    void OnLong()
    {
        print("long");
    }
    */
}