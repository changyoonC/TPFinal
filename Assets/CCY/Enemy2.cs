using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    public Transform player;
    public float followDistance = 5f;
    public float returnDistance = 10f;
    public float shootingRange;
    public float fireRate = 1f;
    private float nextFireTime;
    public GameObject bullet;
    public GameObject bulletParent;
    public float moveSpeed = 3f;
    public Animator anim;

    private Vector3 startingPosition;

    private void Start()
    {
        startingPosition = transform.position;
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        float distanceToStartingPos = Vector3.Distance(transform.position, startingPosition);

        if (distanceToPlayer <= followDistance && distanceToPlayer>shootingRange)
        {
            // 플레이어 따라가기
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
            anim.SetBool("isWalk", true);
            anim.SetBool("Attack", false);
        }
        else if (distanceToPlayer <= shootingRange && nextFireTime<Time.time)
        {
            Instantiate(bullet, bulletParent.transform.position, Quaternion.identity);
            nextFireTime = Time.time+fireRate;
            anim.SetBool("Attack", true);
            anim.SetBool("isWalk", false);
        }
        else if (distanceToStartingPos > returnDistance)
        {
            // 돌아가기
            Vector3 direction = (startingPosition - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
            anim.SetBool("isWalk", true);
            anim.SetBool("Attack", false);
        }
        else
        {
            anim.SetBool("isWalk", false);
        }
    }
}
