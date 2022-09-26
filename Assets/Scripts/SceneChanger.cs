using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public bool isExit;
    public string target;

    [SerializeField]
    bool changeOnStart;

    void Start()
    {
        if (changeOnStart)
        {
            Change(target);
        }
    }

    public void Change(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void Exit()
    {
        Application.Quit(0);
    }

    public static void StaticChange(string name)
    {
        SceneManager.LoadScene(name);
    }
}