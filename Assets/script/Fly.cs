using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed = 3f; 
    public float maxDistance = 10f; 
    public Transform player; 
    private Rigidbody2D rb; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
        rb.gravityScale = 0f; 
    }

    void FixedUpdate()
    {

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

  
        if (distanceToPlayer <= maxDistance)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.AddForce(direction * moveSpeed, ForceMode2D.Force);
        }
    }
}
