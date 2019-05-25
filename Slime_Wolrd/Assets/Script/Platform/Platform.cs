using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private PlatformEffector2D effector;
    private float time = 0f;
    public float defaulTime = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            if (Input.GetAxisRaw("Vertical") < 0)
            {
                if (time <= 0)
                {
                    effector.rotationalOffset = 180;
                    time = defaulTime;
                }
                else
                {
                    time -= Time.fixedDeltaTime;
                }
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        effector.rotationalOffset = 0;
    }
}
