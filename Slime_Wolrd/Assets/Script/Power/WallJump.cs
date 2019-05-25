using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJump : Power
{
    public float distanceCollition = 1.30f;
    public Transform posWallJump;
    public GameObject shot;
    public Transform shotSpawn;
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
        RaycastHit2D hit = Physics2D.Raycast(posWallJump.position, (Vector2.right * direction) * posWallJump.localScale.x, distanceCollition);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Wall")
            {
                rb.velocity = new Vector2(-direction * speed, height);
            }
        }
        else
        {
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        }
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * transform.localScale.x * distanceCollition);
    }
    */
}
