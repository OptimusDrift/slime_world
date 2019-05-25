using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAtack : Power
{
    SimpleAtack()
    {
        namePower = "default";
        coolDownAtackTime = 0.6f;
        initialTimeAtack = 0.3f;
        damage = 1f;
        speed = 20f;
        damage = 1f;
        height = 5f;
    }
    // Start is called before the first frame update
    void Start()
    {
        namePower = "default";
        coolDownAtackTime = 0.6f;
        initialTimeAtack = 0.3f;
        damage = 1f;
        speed = 20f;
        damage = 1f;
        height = 5f;
    }
    //Dash
    override
        public void UsePowerPlayer(Rigidbody2D rb, float direction)
    {
        rb.velocity = new Vector2(direction * speed, height);
    }
    override
        public Power ChangePower()
    {
        return new SimpleAtack();
    }
}
