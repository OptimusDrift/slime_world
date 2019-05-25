using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    public Collider2D damageColider;
    private float cooldownPower, powerDurationTime;
    public float defaultPowerDurationTime = 1f;
    public float timeCooldownPower = 1f;
    private bool isCollision, reset;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cooldownPower = 0f;
        powerDurationTime = defaultPowerDurationTime;
        reset = true;
    }
    private void Update()
    {
        if (isCollision)
        {
            if (cooldownPower <= 0 && reset)
            {
                Debug.Log("cooldownPower <= 0");
                if (powerDurationTime > 0)
                {
                    Debug.Log("powerDurationTime > 0");
                    damageColider.enabled = true;
                    powerDurationTime -= Time.deltaTime;
                    //animacion de ataque activada 1 sola vez
                }
                else
                {
                    Debug.Log("power <= 0");
                    cooldownPower = timeCooldownPower;
                }
            }
            else if (powerDurationTime <= 0)
            {
                Debug.Log("powerDurationTime <= 0");
                if (cooldownPower > 0)
                {
                    Debug.Log("cooldown > 0");
                    cooldownPower -= Time.deltaTime;
                    damageColider.enabled = false;
                    reset = false;
                }
                else
                {
                    Debug.Log("cooldownPower <= 0");
                    powerDurationTime = defaultPowerDurationTime;
                    reset = true;
                }
                //Aninacion de ataque desactivada 1 sola vez
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            isCollision = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            isCollision = false;
            damageColider.enabled = false;
            cooldownPower = 0f;
            powerDurationTime = defaultPowerDurationTime;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            rb.velocity = Vector2.zero;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            rb.velocity = Vector2.zero;
        }
    }
}
