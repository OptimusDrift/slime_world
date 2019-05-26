using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : Power
{

    // Start is called before the first frame update
    void Start()
    {
        namePower = "flyPower";
    }

    override
        public void UsePowerPlayer(Rigidbody2D rb, float direction)
    {
        Debug.Log("vuelva vuelaaaaaaa");
        rb.velocity = new Vector2(rb.velocity.x, height);
    }

    override
        public void UsePowerEnemy(Collider2D damageColider, bool direction)
    {

    }
}
