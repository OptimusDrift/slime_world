using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : MonoBehaviour
{
    //Nombre y efecto del poder
    protected string namePower, efect;
    //Daño del ataque si no tiene el valor es 0
    public float damage = 0f;
    //Velocidad o movimiento en el eje X
    public float speed = 0f;
    //Velocidad o movimiento en el eye Y
    public float height = 0f;
    //Tiepo en que tarda en volver a tirar el poder
    public float coolDownAtackTime = 0f;
    //Tiempo inicial
    public float initialTimeAtack = 0f;

    //El poder que usa el jugador
    virtual
    public void UsePowerPlayer(Rigidbody2D rb, float direction)
    {

    }

    //Poder que usa el enemigo
    virtual
    public float UsePowerEnemy(Rigidbody2D rb, float direction)
    {
        return 0f;
    }

    public string GetName()
    {
        return namePower;
    }
}
