using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJump : Power
{
    [Header ("Player")]
    //Distancia del personaje a la pared
    public float distanceCollition = 0.01f;
    //Tranform desde donde salta
    public Transform posWallJump;
    //Objeto que dispara el pj
    public GameObject shot;
    //Desde donde dispara el pj
    public Transform shotSpawn;
    //Cada cuanto sale un disparo
    private float shotTime;
    //Tiempo inicial de disparo
    public float initialShotTime = 0.3f;
    [Header ("Enemy Jump")]
    //Valor minimo de los saltos enemigos
    public float minValueJumpUp;
    //Valor maximo de los saltos enemigos
    public float maxValueJumpUp;
    //Valor minimo de los saltos enemigos
    public float minValueJump;
    //Valor maximo de los saltos enemigos
    public float maxValueJump;
    public Transform feetPos;
    public Transform headPos;
    public Transform rigthPos;
    public Transform leftPos;
    public LayerMask whatIsWall;
    public float checkRadius;


    // Start is called before the first frame update
    void Start()
    {
        namePower = "wallJump";
        damage = 0f;
        speed = 30f;
        height = 10f;
        shotTime = 0f;
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
            if (shotTime <= 0)
            {
                Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
                shotTime = initialShotTime;
            }
            else
            {
                shotTime -= Time.fixedDeltaTime;
            }
        }
    }

    private bool IsGroundedFeet()
    {
        //Revisa el objeto de feetPos y revisa si esta colicionando con el tag "Ground"
        return Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsWall);
    }

        private bool IsGroundedRigth()
    {
        //Revisa el objeto de feetPos y revisa si esta colicionando con el tag "Ground"
        return Physics2D.OverlapCircle(rigthPos.position, checkRadius, whatIsWall);
    }
    private bool IsGroundedLeft()
    {
        //Revisa el objeto de feetPos y revisa si esta colicionando con el tag "Ground"
        return Physics2D.OverlapCircle(leftPos.position, checkRadius, whatIsWall);
    }
    private bool IsGroundedHead()
    {
        //Revisa el objeto de feetPos y revisa si esta colicionando con el tag "Ground"
        return Physics2D.OverlapCircle(headPos.position, checkRadius, whatIsWall);
    }

    private float Randomicer(float min, float max)
    {
        float x = min;
        if (min < 0)
        {
            x = min * -1;
        }
        else if (min == 0)
        {
            x = max / 2;
        }
        if (Random.Range(min, max) >= (max - x))
        {
            return max;
        }
        return min;
    }

    private float jump(float directionV, Rigidbody2D rb, float direction, bool h)
    {
        float direc = direction;
        if (coolDownAtackTime <= 0)
        {
            direc = Random.Range(-1, 1);
            if (direc >= 0)
            {
                direc = 1;
            }
            else if (direc < 0)
            {
                direc = -1;
            }
            if ((direc == direction) && h)
            {
                direc *= -1;
            }
            rb.gravityScale = 3f;
            rb.velocity = new Vector2(direc * Random.Range(minValueJump, maxValueJump), Random.Range(minValueJumpUp * directionV, maxValueJumpUp * directionV));
            coolDownAtackTime = Random.Range(0.5f,2f);
        }
        else
        {
            rb.gravityScale = 0f;
            rb.velocity = Vector2.zero;
            coolDownAtackTime -= Time.fixedDeltaTime;
        }
        return direc;
    }
    override
        public float UsePowerEnemy(Rigidbody2D rb, float direction)
    {
        float direc = direction;
        if (IsGroundedFeet())
        {
            direc = jump(1f, rb, direction, false);
        }
        else if (IsGroundedLeft())
        {
            direc = jump(Randomicer(-1f, 1f), rb, direction, true);
        }
        else if(IsGroundedRigth())
        {
            direc = jump(Randomicer(-1f, 1f), rb, direction, true);
        }
        else if(IsGroundedHead())
        {
            direc = jump(-1f, rb, direction, false);
        }
        return direc;
    }
}
