using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControllerBoss : MonoBehaviour
{
    public float speed = 10f;
    private Vector2 direction;
    private Vector3 initialScale;
    GameObject target;
    Rigidbody2D bulletRB;
    public GameObject player;

    // Set the bullet's direction
    public void SetDirection(Vector2 dir)
    {
        direction = -dir; // Reverse the provided direction vector
    }

    private void Start()
    {
        bulletRB = GetComponent<Rigidbody2D>();
        initialScale = transform.localScale;
        target = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        
        transform.Translate(direction * speed * Time.deltaTime);

        
        if (direction.x < 0)
        {
            
            transform.localScale = new Vector3(-initialScale.x, initialScale.y, initialScale.z);
        }
        else if (direction.x > 0)
        {   
            transform.localScale = initialScale;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "player")
        {
            target.GetComponent<Player>().TakeDamage(1);
            Destroy(gameObject);
        }
    }
}