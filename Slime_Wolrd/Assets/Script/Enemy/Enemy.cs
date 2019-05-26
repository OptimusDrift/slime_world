using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    public Collider2D damageColider;
    private float cooldownPower, powerDurationTime;
    public float defaultPowerDurationTime = 4f;
    public float timeCooldownPower = 4f;
    private Power power;
    private bool reset;
    private RaycastHit2D[] rcs;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        power = GetComponent<FirePower>();
        cooldownPower = 0f;
        powerDurationTime = defaultPowerDurationTime;
        reset = true;
    }
    private void ActiveAtack()
    {
        for (int i = 0; i < rcs.Length; i++)
        {
            if (rcs[i].collider.tag.Equals("Player"))
            {
                if (cooldownPower <= 0 && reset)
                {
                    Debug.Log("Agrgar animaciones :V");
                    if (powerDurationTime > 0)
                    {
                        power.UsePowerEnemy(damageColider,true);
                        powerDurationTime -= Time.deltaTime;
                        //animacion de ataque activada 1 sola vez
                    }
                    else
                    {
                        cooldownPower = timeCooldownPower;
                    }
                }
                else if (powerDurationTime <= 0)
                {
                    if (cooldownPower > 0)
                    {
                        cooldownPower -= Time.deltaTime;
                        power.UsePowerEnemy(damageColider, false);
                        reset = false;
                    }
                    else
                    {
                        powerDurationTime = defaultPowerDurationTime;
                        reset = true;
                    }
                    //Aninacion de ataque desactivada 1 sola vez
                    Debug.Log("Agrgar animaciones :V");
                }
                break;
            }
            else if (i + 1 == rcs.Length)
            {
                power.UsePowerEnemy(damageColider, false);
                cooldownPower = 0f;
                powerDurationTime = defaultPowerDurationTime;
                Debug.Log("Agrgar animaciones :V");
            }
        }
    }
    private void FixedUpdate()
    {
        rcs = Physics2D.CircleCastAll(transform.position, 10f, transform.position);
        ActiveAtack();
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
