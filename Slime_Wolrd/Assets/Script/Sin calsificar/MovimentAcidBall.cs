using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentAcidBall : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool first;
    private int direction;
    public float speed = 10f;
    public float height = 5f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        first = true;
        speed = 20f;
        height = 10f;
}

    private void FixedUpdate()
    {
        if (first)
        {
            rb.velocity = new Vector2(GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerController>().direction * speed, height);
            first = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Wall"))
        {
            Destroy(gameObject);
            Debug.Log(collision.name);
        }
    }
}
