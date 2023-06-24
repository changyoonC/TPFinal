using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    public Transform player;
    public float followDistance = 5f;
    public float returnDistance = 10f;
    public float moveSpeed = 3f;

    private Vector3 startingPosition;

    private void Start()
    {
        startingPosition = transform.position;
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        float distanceToStartingPos = Vector3.Distance(transform.position, startingPosition);

        if (distanceToPlayer <= followDistance)
        {
            // 플레이어 따라가기
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
        else if (distanceToStartingPos > returnDistance)
        {
            // 돌아가기
            Vector3 direction = (startingPosition - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }
}
