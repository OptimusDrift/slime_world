using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float moveInput, atackInput;
    private Rigidbody2D rb;
    private bool isGrounded;
    private float jumpTimeCounter;
    private bool isJumping;
    private int direction = 1;
    private CapsuleCollider2D cc2d;
    //POWER
    private Power power;
    private bool isAtack = false;
    private float timeAtack;
    private float initialTimeAtack = 0.3f;
    private float cooldownAtack = 0f;
    private float cooldownAtackTime = 0.6f;

    public float speed;
    public int countLive = 3;
    public float live;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;
    public float jumpForce;
    public float jumpTime;
    private float jumpInput;
    private float inmuneTime;
    private float flyDamageTime;
    public float initialFlyDamageTime = 0.2f;
    public float initialInmuneTime = 0.5f;
    private bool inDamage = false;
    public float pushDamage = 10f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cc2d = GetComponent<CapsuleCollider2D>();
        power = GetComponent<Power>();
        timeAtack = initialTimeAtack;
    }
    private void Move()
    {
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
    }
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);
    }
    private void AnimatedWalk()
    {
        if (moveInput > 0 && !isAtack)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            direction = 1;
        }
        else if (moveInput < 0 && !isAtack)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            direction = -1;
        }
    }
    private void GetDamage(float damage)
    {
        if (inmuneTime <= 0)
        {
            live -= damage;
            inmuneTime = initialInmuneTime;
            flyDamageTime = initialFlyDamageTime;
            inDamage = true;
        }
    }
    private void Jump()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space) && !isAtack)
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up * jumpForce;
        }

        if (Input.GetKey(KeyCode.Space) && !isAtack)
        {
            if (jumpTimeCounter > 0 && isJumping)
            {
                rb.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }
    }
    private void Atack()
    {
        if (atackInput > 0 && !isAtack && cooldownAtack < 0f)
        {
            isAtack = true;
        }
        else if (isAtack && timeAtack > 0)
        {
            timeAtack -= Time.deltaTime;
        }
        else if (timeAtack <= 0)
        {
            isAtack = false;
            timeAtack = initialTimeAtack;
            cooldownAtack = cooldownAtackTime;
        }
    }
    private void FixedUpdate()
    {
        if (inDamage)
        {
            if (flyDamageTime  > 0)
            {
                rb.velocity = new Vector2(direction * -pushDamage, pushDamage);
            }
            else
            {
                inDamage = false;
            }
        }
        else if(inmuneTime <= 0)
        {
            moveInput = Input.GetAxisRaw("Horizontal");
            atackInput = Input.GetAxisRaw("Atack");
            //jumpInput = Input.GetAxis("Jump");
            Move();
            if (isAtack)
            {
                power.UsePower(rb, direction);
            }
        }    
    }

    private void Update()
    {
        isGrounded = IsGrounded();
        Atack();
        cooldownAtack -= Time.deltaTime;
        AnimatedWalk();
        Jump();
        inmuneTime -= Time.deltaTime;
        flyDamageTime -= Time.deltaTime;
        Debug.Log(live.ToString());
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (power.GetName().Equals("default"))
        {
            if (collision.gameObject.tag.Equals("Enemy"))
            {
                if (isAtack)
                {
                    power = collision.gameObject.GetComponent<Power>();
                    Destroy(collision.gameObject);  //Animacion de muerte
                }
                else
                {
                    GetDamage(collision.gameObject.GetComponent<Power>().damage);
                }
            }
        }
        else
        {
            if (collision.gameObject.tag.Equals("Enemy"))
            {
                GetDamage(collision.gameObject.GetComponent<Power>().damage);
            }
        }
    }
}
