using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    private Canvas canvasPause;
    private bool stop;

    // Start is called before the first frame update
    void Start()
    {
        canvasPause = GetComponent<Canvas>();
    }

    public void SetPuase()
    {
        canvasPause.enabled = !canvasPause.enabled;
        Time.timeScale = (canvasPause.enabled) ? 0f : 1f;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Pause") > 0 && !stop)
        {
            SetPuase();
            stop = true;
        }
        else if (Input.GetAxisRaw("Pause") == 0)
        {
            stop = false;
        }
    }
}
