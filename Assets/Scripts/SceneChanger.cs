using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string target;

    public void Change(string name)
    {
        SceneManager.LoadScene(name);
    }

    public static void StaticChange(string name)
    {
        SceneManager.LoadScene(name);
    }
}