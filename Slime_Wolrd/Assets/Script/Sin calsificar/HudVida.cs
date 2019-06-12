using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudVida : MonoBehaviour
{
    public Image[] life;
    public Image[] canvasLife;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < player.GetComponent<PlayerController>().maxLive; i++)
        {
            life[i].GetComponent<Image>().enabled = true;
            canvasLife[i].GetComponent<Image>().enabled = true;
        }
    }
    public bool LostLive(float count)
    {
        float x = count;
        for (int i = life.Length-1; i > -1; i--)
        {
            if (x <= 0)
            {
                break;
            }
            if (life[i].GetComponent<Image>().enabled)
            {
                life[i].GetComponent<Image>().enabled = false;
                if (i == 0)
                {
                    return true;
                }
                x--;
            }
        }
        return false;
    }

    public bool WinLife()
    {
        float x = 1f;
        for (int i = 0; i < life.Length; i++)
        {
            if (x <= 0)
            {
                break;
            }
            if (!life[i].GetComponent<Image>().enabled && canvasLife[i].GetComponent<Image>().enabled)
            {
                life[i].GetComponent<Image>().enabled = true;
                x--;
            }
        }
        if (x == 0)
        {
            return true;
        }
        return false;
    }

    public bool WinMaxLife()
    {
        for (int i = canvasLife.Length - 1; i > -1; i--)
        {
            if (canvasLife[i].GetComponent<Image>().enabled)
            {
                if (i == canvasLife.Length - 1)
                {
                    return false;
                }
                canvasLife[i + 1].GetComponent<Image>().enabled = true;
                life[i + 1].GetComponent<Image>().enabled = true;
            }
        }
        return true;
    }

    public float WinFullLife()
    {
        float x = 0;
        for (int i = 0; i < life.Length; i++)
        {
            if (x >= life.Length)
            {
                break;
            }
            if (!life[i].GetComponent<Image>().enabled && canvasLife[i].GetComponent<Image>().enabled)
            {
                life[i].GetComponent<Image>().enabled = true;
                x++;
            }
        }
        return x;
    }
}
