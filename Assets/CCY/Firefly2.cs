using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firefly2 : MonoBehaviour
{
    public float speed = 5f; //움직이는 속도
    public float width = 10f; //움직이는 구역 
    public float height = 5f;
    public float acceleration = 1f; //+ 가속도
    public float deceleration = 1f;  //- 가속도
    private Vector2 targetDirection; 
    private Vector2 currentDirection;
    private float targetSpeed;
    private float currentSpeed; 

    void Start()
    {
        currentDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        targetDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        currentSpeed = speed;
        targetSpeed = speed;
    }

    void Update()
    {
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
        transform.Translate(currentDirection * currentSpeed * Time.deltaTime);

        if (transform.position.x < -width / 2 && currentDirection.x < 0)
        {
            targetDirection.x = Random.Range(0.1f, 1f);
            targetSpeed = Random.Range(0.1f, speed);
        }
        else if (transform.position.x > width / 2 && currentDirection.x > 0)
        {
            targetDirection.x = Random.Range(-1f, -0.1f);
            targetSpeed = Random.Range(0.1f, speed);
        }
        if (transform.position.y < -height / 2 && currentDirection.y < 0)
        {
            targetDirection.y = Random.Range(0.1f, 1f);
            targetSpeed = Random.Range(0.1f, speed);
        }
        else if (transform.position.y > height / 2 && currentDirection.y > 0)
        {
            targetDirection.y = Random.Range(-1f, -0.1f);
            targetSpeed = Random.Range(0.1f, speed);
        }

        currentDirection = Vector2.Lerp(currentDirection, targetDirection, deceleration * Time.deltaTime);
    }
}

