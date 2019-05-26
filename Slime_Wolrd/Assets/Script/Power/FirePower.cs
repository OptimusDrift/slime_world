using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePower : Power
{
    private void Start()
    {
        namePower = "firePower";
    }

    override
        public void UsePowerPlayer(Rigidbody2D rb, float direction)
    {
        Debug.Log("asd");
    }
    override
        public void UsePowerEnemy(Collider2D damageColider, bool active)
    {
        damageColider.enabled = active;
    }
}
