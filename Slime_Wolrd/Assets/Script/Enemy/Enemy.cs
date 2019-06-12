using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    //public float timeCooldownPower = 4f;
    private Power power;
    private bool collision;
    private float moveInput = 1;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        power = GetComponent<Power>();
        power.coolDownAtackTime = 0f;
        Physics2D.IgnoreLayerCollision(11, 11);
    }
    //Giro del pj
    private void AnimatedWalk(float moveInput)
    {
        this.moveInput = moveInput;
        //Si es mayor a 0 y no esta atacando gira el pj a la derecha
        if (moveInput > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }//Si es mayor a 0 y no esta atacando gira el pj a la izquierda
        else if (moveInput < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }
    private void ActiveAtack()
    {
        AnimatedWalk(power.UsePowerEnemy(rb, moveInput));
    }

    private void FixedUpdate()
    {
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
