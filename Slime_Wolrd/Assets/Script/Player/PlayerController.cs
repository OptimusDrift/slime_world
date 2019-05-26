using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header ("Salto")]
    //moveImput es el valor hacia donde se mueve control
    //atackImput es para saber si esta atacando
    //jumpImput es para saber si esta saltando
    //changePowerInput es para saber si esta cambiando de poder
    private float moveInput, atackInput, jumpInput, changePowerInput;
    private Rigidbody2D rb;
    //si esta tocando el suelo
    private bool isGrounded;
    //Si esta saltando
    private bool isJumping;
    //Que es lo que se considera suelo
    public LayerMask whatIsGround;

    //A donde esta apuntando el pj
    public int direction = 1;
    //colisionador
    private CapsuleCollider2D cc2d;

    [Header("Poderes")]
    //POWERS
    //Lista de poderes
    private List<string> allPowerGet;
    //Cooldown del cambio de poder
    private float changeCooldown;
    //tiempo inicial del cambio de poder
    public float timeChangeCooldown = 0.1f;
    //Puntero del poder
    private int curretPowerSelect;
    //Poder actual
    private Power currentPower;
    //Esta atacando
    private bool isAtack = false;
    //Tiempo para volver a atacar
    private float coolDownAtack = 0f;
    //Velocidad del pj
    public float speed;

    [Header("Vida")]
    //Valores del pj
    //Vida
    public float live;
    //Posicion del piso
    public Transform feetPos;
    //Radio de los pies
    public float checkRadius;
    //Fuerza del salto
    public float jumpForce;

    [Header("Tiempo")]
    //Tiempos
    //Tiempo actual del salto
    public float jumpTime;
    //Tiempo de inmunidad
    private float inmuneTime;
    //Tiempo del ataque
    private float timeAtack;
    //El tiempo maximo de salto
    private float jumpTimeCounter;
    //Tiempo en el que el pj sale volando
    private float flyDamageTime;
    //Tiempo de vuelo inicial
    public float initialFlyDamageTime = 0.2f;
    //Tiempo de inmunidad inicial
    public float initialInmuneTime = 0.5f;
    //esta resiviendo daño
    private bool inDamage = false;
    //Velocidad del vuelo cuando dañan al pj
    public float pushDamage = 10f;
    //Si la tecla del ataque sigue apretada
    private bool atackKeyPress = false;

    // Start is called before the first frame update
    void Start()
    {
        //Asignacion inicial del pj
        rb = GetComponent<Rigidbody2D>();
        cc2d = GetComponent<CapsuleCollider2D>();
        currentPower = GetComponent<SimpleAtack>();
        allPowerGet = new List<string>
        {
            GetComponent<SimpleAtack>().GetName()
        };

    }
    //Movimiento del personaje
    private void Move()
    {
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
    }
    //Devuelve si esta en el suelo
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);
    }
    //Giro del pj
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
    //Resivir daño
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
    //Saltar
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
    //Ataque
    private void Atack()
    {
        if (atackInput > 0 && !isAtack && coolDownAtack < 0f)
        {
            atackKeyPress = true;
            isAtack = true;
        }
    }


    //Cambio de poder
    private void ChangePower(string name)
    {
        Debug.Log("AGREGAR ATAQUES!!!" + name);
        switch (name)
        {
            case "default":
                currentPower = gameObject.GetComponent<SimpleAtack>();
                break;
            case "wallJump":
                currentPower = gameObject.GetComponent<WallJump>();
                break;
            case "firePower":
                currentPower = gameObject.GetComponent<FirePower>();
                break;
            case "flyPower":
                currentPower = gameObject.GetComponent<Fly>();
                break;
            default:
                //gameObject.AddComponent<WallJump>();
                break;
        }
    }
    //Colicion del personaje
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (currentPower.GetName().Equals("default"))
        {
            if (collision.gameObject.tag.Equals("Enemy"))
            {
                if (isAtack)
                {
                    ChangePower(collision.gameObject.GetComponent<Power>().GetName());
                    Debug.Log(collision.gameObject.GetComponent<Power>().GetName());
                    allPowerGet.Add(currentPower.GetName());
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
    //Cambio de poderes
    private void ChangePowerButton()
    {
        //Cambia al precionar Accion3
        if (changePowerInput > 0)
        {
            if (curretPowerSelect + 1 >= allPowerGet.Count)
            {
                if (!(curretPowerSelect == 0))
                {
                    curretPowerSelect = 0;
                    ChangePower(allPowerGet[curretPowerSelect]);
                }
            }
            else
            {
                curretPowerSelect++;
                ChangePower(allPowerGet[curretPowerSelect]);
            }
            changeCooldown = timeChangeCooldown;
        }//Cambia al precionar Accion4
        else if (changePowerInput < 0)
        {
            if (curretPowerSelect == 0)
            {
                if (!(curretPowerSelect + 1 == allPowerGet.Count))
                {
                    curretPowerSelect = allPowerGet.Count-1;
                    ChangePower(allPowerGet[curretPowerSelect]);
                }
            }
            else
            {
                curretPowerSelect--;
                ChangePower(allPowerGet[curretPowerSelect]);
            }
            changeCooldown = timeChangeCooldown;
        }
    }
    private void FixedUpdate()
    {
        if (inDamage)
        {
            if (flyDamageTime > 0)
            {
                rb.velocity = new Vector2(direction * -pushDamage, pushDamage);
            }
            else
            {
                inDamage = false;
            }
        }
        else if (inmuneTime <= 0)
        {
            moveInput = Input.GetAxisRaw("Horizontal");
            atackInput = Input.GetAxisRaw("Atack");
            changePowerInput = Input.GetAxisRaw("Accion3");
            if (!isAtack)
            {
                //cambio de poder
                if (isGrounded && changeCooldown <= 0)
                {
                    ChangePowerButton();
                }
                Move();
            }
            else
            {
                Debug.Log(currentPower.GetName());
                currentPower.UsePowerPlayer(rb, direction);
            }
        }
    }

    private void Update()
    {
        isGrounded = IsGrounded();
        if (isAtack && timeAtack > 0)
        {
            timeAtack -= Time.deltaTime;
        }
        else if (timeAtack <= 0)
        {
            isAtack = false;
            timeAtack = currentPower.initialTimeAtack;
            coolDownAtack = currentPower.coolDownAtackTime;
        }
        if (atackInput == 0)
        {
            atackKeyPress = false;
        }
        if (!atackKeyPress)
        {
            Atack();
        }
        coolDownAtack -= Time.deltaTime;
        changeCooldown -= Time.deltaTime;
        AnimatedWalk();
        Jump();
        inmuneTime -= Time.deltaTime;
        flyDamageTime -= Time.deltaTime;
    }
}
