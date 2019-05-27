using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePower : Power
{
    [Header ("Enemy")]
    public Collider2D damageColider;
    private RaycastHit2D[] rcs;
    private bool reset;
    private float powerDurationTime;
    public float defaultPowerDurationTime = 4f;

    private void Start()
    {
        namePower = "firePower";
        powerDurationTime = defaultPowerDurationTime;
        reset = true;
    }

    override
        public void UsePowerPlayer(Rigidbody2D rb, float direction)
    {
        Debug.Log("asd");
    }
    override
        public float UsePowerEnemy(Rigidbody2D rb, float direction)
    {
        rcs = Physics2D.CircleCastAll(transform.position, 10f, transform.position);
        for (int i = 0; i < rcs.Length; i++)
        {
            if (rcs[i].collider.tag.Equals("Player"))
            {
                if (coolDownAtackTime <= 0 && reset)
                {
                    Debug.Log("Agrgar animaciones :V");
                    if (powerDurationTime > 0)
                    {
                        damageColider.enabled = true;
                        powerDurationTime -= Time.fixedDeltaTime;
                        //animacion de ataque activada 1 sola vez
                    }
                    else
                    {
                        coolDownAtackTime = initialTimeAtack;
                    }
                }
                else if (powerDurationTime <= 0)
                {
                    if (coolDownAtackTime > 0)
                    {
                        coolDownAtackTime -= Time.fixedDeltaTime;
                        damageColider.enabled = false;
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
                damageColider.enabled = false;
                coolDownAtackTime = 0f;
                powerDurationTime = defaultPowerDurationTime;
                Debug.Log("Agrgar animaciones :V");
            }
        }
        return 0f;
    }
}
