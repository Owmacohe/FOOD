using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    TMP_Text money;
    [SerializeField]
    TMP_Text highScore;
    [SerializeField]
    RawImage strike1, strike2, strike3;
    [SerializeField]
    TMP_Text moveButton;
    [SerializeField]
    TMP_Text timer;

    [SerializeField]
    Texture2D heart, skeleton;

    public void SetMoney(float m)
    {
        money.text = "$" + m;
    }

    public void SetHighScore(float hs)
    {
        highScore.text = "Best: $" + hs;
    }

    public void SetStrikes(int s)
    {
        strike1.texture = heart;
        strike2.texture = heart;
        strike3.texture = heart;
        
        if (s > 0)
        {
            strike1.texture = skeleton;
            
            if (s > 1)
            {
                strike2.texture = skeleton;
                
                if (s > 2)
                {
                    strike3.texture = skeleton;
                }
            }
        }
    }

    public void SetMoveButtonKey(string k)
    {
        moveButton.text = "Press and hold [" + k + "] to move";
    }

    public void SetTimer(float t)
    {
        int m = 0;

        while (t > 60)
        {
            t -= 60;
            m++;
        }

        string minutes = "" + m;

        if (int.Parse(minutes) < 10)
        {
            minutes = "0" + minutes;
        }

        string seconds = "" + (int)t;

        if (int.Parse(seconds) < 10)
        {
            seconds = "0" + (int)t;
        }

        timer.text = minutes + ":" + seconds;
    }
}