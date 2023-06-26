using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControllerBoss : MonoBehaviour
{
    public float speed = 10f;
    private Vector2 direction;
    private Vector3 initialScale;

    // Set the bullet's direction
    public void SetDirection(Vector2 dir)
    {
        direction = -dir; // Reverse the provided direction vector
    }

    private void Start()
    {
        // Store the initial scale of the object
        initialScale = transform.localScale;
    }

    private void Update()
    {
        // Move the bullet in the specified direction
        transform.Translate(direction * speed * Time.deltaTime);

        // Flip the sprite if the direction is negative along the x-axis
        if (direction.x < 0)
        {
            // Set the local scale of the transform to flip the image
            transform.localScale = new Vector3(-initialScale.x, initialScale.y, initialScale.z);
        }
        else if (direction.x > 0)
        {
            // Reset the local scale if the direction is positive along the x-axis
            transform.localScale = initialScale;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Handle collision logic here
    }
}