using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : Power
{
    private Transform target;
    private RaycastHit2D[] rcs;
    private bool reset;
    private float powerDurationTime;
    public float defaultPowerDurationTime = 4f;
    private bool playerInRange;
    // Start is called before the first frame update
    void Start()
    {
        namePower = "flyPower";
        playerInRange = false;
    }

    override
        public void UsePowerPlayer(Rigidbody2D rb, float direction)
    {
        Debug.Log("vuelva vuelaaaaaaa");
        rb.velocity = new Vector2(rb.velocity.x, height);
    }

    override
        public float UsePowerEnemy(Rigidbody2D rb, float direction)
    {
        rcs = Physics2D.CircleCastAll(transform.position, 10f, transform.position);
        Debug.Log(rcs.Length);
        foreach (RaycastHit2D v in rcs)
        {
            Debug.Log(v.collider.tag);
            if (v.collider.tag.Equals("Player"))
            {
                target = v.collider.transform;
                playerInRange = true;
            }
            break;
        }
        if (playerInRange)
        {
            rb.transform.position = Vector3.MoveTowards(rb.position, target.position, speed * Time.fixedDeltaTime);
            transform.up = new Vector2(target.position.x - rb.position.x, target.position.y - rb.position.y);
            powerDurationTime -= Time.fixedDeltaTime;
            //animacion de ataque activada 1 sola vez
        }
        return 0f;
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 10f);
    }*/
}
