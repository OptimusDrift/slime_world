using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJump : Power
{
    private float distance = 1.30f;
    WallJump()
    {
        namePower = "wallJump";
        damage = 0f;
        coolDownAtackTime = 0.05f;
        initialTimeAtack = 0.05f;
        speed = 30f;
        height = 10f;
    }
    // Start is called before the first frame update
    void Start()
    {
        namePower = "wallJump";
        damage = 0f;
        coolDownAtackTime = 0.05f;
        initialTimeAtack = 0.05f;
        speed = 30f;
        height = 10f;
    }

    override
    public void UsePowerPlayer(Rigidbody2D rb, float direction)
    {
        Debug.Log(speed);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, (Vector2.right * direction) * transform.localScale.x, distance);
        if (hit.collider!=null)
        {
            rb.velocity = new Vector2(-direction * speed, height);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * transform.localScale.x * distance);
    }
}
