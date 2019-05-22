using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : MonoBehaviour
{
    protected string namePower, efect;
    public float damage = 1f;
    public float dashSpeed = 20f;

    public void UsePower(Rigidbody2D rb, float direction)
    {
        rb.velocity = new Vector2(direction * dashSpeed, 5f);
    }

    public string GetName()
    {
        return namePower;
    }
}
