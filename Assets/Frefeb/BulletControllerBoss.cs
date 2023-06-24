using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControllerBoss : MonoBehaviour
{
    public float speed = 10f;
    private Vector2 direction;

    // Set the bullet's direction
    public void SetDirection(Vector2 dir)
    {
        direction = dir;
    }

    private void Update()
    {
        // Move the bullet in the specified direction
        transform.Translate(direction*-1 * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}