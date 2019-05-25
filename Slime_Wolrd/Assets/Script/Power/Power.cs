using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : MonoBehaviour
{
    protected string namePower, efect;
    public float damage = 0f;
    public float speed = 0f;
    public float height = 0f;
    public float coolDownAtackTime = 0f;
    public float initialTimeAtack = 0f;

    virtual
    public void UsePowerPlayer(Rigidbody2D rb, float direction)
    {

    }

    virtual
    public void UsePowerEnemy(Rigidbody2D rb, float direction)
    {

    }
    public string GetName()
    {
        return namePower;
    }
    virtual
    public Power ChangePower()
    {
        return new Power();
    }
}
