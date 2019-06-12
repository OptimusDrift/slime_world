using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header ("Salto")]
    //moveImput es el valor hacia donde se mueve control
    //atackImput es para saber si esta atacando
    //changePowerInput es para saber si esta cambiando de poder
    private float moveInput, atackInput, changePowerInput;
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
    //Velocidad del pj
    private float actualSpeed;

    [Header("Vida")]
    //Valores del pj
    //Vida
    public float live;
    public float maxLive;
    //Posicion del piso izq
    public Transform feetPosLeft;
    //Posicion del piso der
    public Transform feetPosRigth;
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

    [Header("HUD")]
    public GameObject Hud;

    // Start is called before the first frame update
    void Start()
    {
        //Asignacion inicial del pj
        rb = GetComponent<Rigidbody2D>();
        cc2d = GetComponent<CapsuleCollider2D>();
        currentPower = GetComponent<SimpleAtack>();
        //Lista de poderes actuales solo empieza con el poder inicial
        allPowerGet = new List<string>
        {
            GetComponent<SimpleAtack>().GetName()
        };
        actualSpeed = speed;
    }

    //Movimiento del personaje
    private void Move()
    {
        //Asigna la velocidad de movimiendo y la direccion, manteniendo la velocidad de caida
        rb.velocity = new Vector2(moveInput * actualSpeed, rb.velocity.y);
    }
    //Devuelve si esta en el suelo
    private bool IsGrounded()
    {
        //Revisa el objeto de feetPos y revisa si esta colicionando con el tag "Ground"
        return Physics2D.OverlapCircle(feetPosLeft.position, checkRadius, whatIsGround) || Physics2D.OverlapCircle(feetPosRigth.position, checkRadius, whatIsGround);
    }
    //Giro del pj
    private void AnimatedWalk()
    {
        //Si es mayor a 0 y no esta atacando gira el pj a la derecha
        if (moveInput > 0 && !isAtack)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            direction = 1;
        }//Si es mayor a 0 y no esta atacando gira el pj a la izquierda
        else if (moveInput < 0 && !isAtack)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            direction = -1;
        }
    }
    //Resivir daño
    private void GetDamage(float damage)
    {
        //Si resive daño el pj tiene un pequeño tiempo de inmunidad
        if (inmuneTime <= 0)
        {
            live -= damage;
            if (Hud.GetComponent<HudVida>().LostLive(damage))
            {
                Debug.Log("Insertar Muerte");
                Debug.Log("Pero como es un mundo loco te revivo");
                live = Hud.GetComponent<HudVida>().WinFullLife();
                Debug.Log("Y te agrego 1 vida extra");
                if (Hud.GetComponent<HudVida>().WinMaxLife())
                {
                    maxLive++;
                }
            }
            inmuneTime = initialInmuneTime;
            flyDamageTime = initialFlyDamageTime;
            inDamage = true;
            Physics2D.IgnoreLayerCollision(10, 11);
        }
    }
    bool stopPress = false;
    //Saltar
    private void Jump()
    {
        //Si no esta en el aire puede saltar
        if (isGrounded && Input.GetAxisRaw("Jump") > 0 && !isAtack)
        {
            if (!stopPress)
            {
                isJumping = true;
                jumpTimeCounter = jumpTime;
                rb.velocity = Vector2.up * jumpForce;
                stopPress = true;
            }
        }

        if (Input.GetAxisRaw("Jump") > 0 && !isAtack && stopPress)
        {
            if (stopPress)
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
        }

        if (Input.GetAxisRaw("Jump") == 0)
        {
            isJumping = false;
            stopPress = false;
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
                currentPower =  gameObject.GetComponent<WallJump>();
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
    //Busca el ataque
    private bool NewAtack(string name)
    {
        foreach (var item in allPowerGet)
        {
            if (item.Equals(name))
            {
                return true;
            }
        }
        return false;
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
                    if (Hud.GetComponent<HudVida>().WinLife())
                    {
                        live++;
                    }
                    if (!NewAtack(collision.gameObject.GetComponent<Power>().GetName()))
                    {
                        allPowerGet.Add(collision.gameObject.GetComponent<Power>().GetName());
                    }
                    //ChangePower(collision.gameObject.GetComponent<Power>().GetName());
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
    bool changePress = false;
    //Cambio de poderes
    private void ChangePowerButton()
    {
        //Cambia al precionar Accion3
        if (changePowerInput > 0)
        {
            if (!changePress)
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
                changePress = true;
            }
        }//Cambia al precionar Accion4
        else if (changePowerInput < 0)
        {
            if (!changePress)
            {
                if (curretPowerSelect == 0)
                {
                    if (!(curretPowerSelect + 1 == allPowerGet.Count))
                    {
                        curretPowerSelect = allPowerGet.Count - 1;
                        ChangePower(allPowerGet[curretPowerSelect]);
                    }
                }
                else
                {
                    curretPowerSelect--;
                    ChangePower(allPowerGet[curretPowerSelect]);
                }
            }
            changePress = true;
        }
        if (changePowerInput == 0)
        {
            changePress = false;
        }
    }

    private void CounterDecrease()
    {
        coolDownAtack -= Time.deltaTime;
        changeCooldown -= Time.deltaTime;
        inmuneTime -= Time.deltaTime;
        flyDamageTime -= Time.deltaTime;
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
            Physics2D.IgnoreLayerCollision(10, 11, false);
            moveInput = Input.GetAxisRaw("Horizontal");
            AnimatedWalk();
            atackInput = Input.GetAxisRaw("Atack");
            if (Input.GetAxisRaw("Run") > 0)
            {
                Run(speed * 2);
            }
            else
            {
                Run(speed);
            }
            changePowerInput = Input.GetAxisRaw("Accion3");
            if (!isAtack)
            {
                //cambio de poder
                ChangePowerButton();
                Move();
            }
            else
            {
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
        Jump();
        CounterDecrease();
    }

    public void Run(float runSpeed)
    {
        actualSpeed = runSpeed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(feetPosLeft.position, checkRadius);
        Gizmos.DrawWireSphere(feetPosRigth.position, checkRadius);
    }
}
