using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHP;  // 적의 최대 체력
    private int currentHP;  // 적의 현재 체력

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
    public float isFlying = 0;

    private Vector3 startingPosition;



    public float maxDistance = 10f;
    private Rigidbody2D rb;

    void Start()
    {
        currentHP = maxHP;
        startingPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        
    }

    void Update()
    {
        if (isFlying == 0)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            float distanceToStartingPos = Vector3.Distance(transform.position, startingPosition);

            if (distanceToPlayer <= followDistance && distanceToPlayer > shootingRange)
            {
                // 플레이어 따라가기
                Vector3 direction = (player.position - transform.position).normalized;
                transform.position += direction * moveSpeed * Time.deltaTime;

                // 방향을 플레이어 방향으로 뒤집기
                if (player.position.x < transform.position.x)
                    transform.localScale = new Vector3(1f, 1f, 1f);
                else
                    transform.localScale = new Vector3(-1f, 1f, 1f);

                anim.SetBool("isWalk", true);
                anim.SetBool("Attack", false);
            }
            else if (distanceToPlayer <= shootingRange && nextFireTime < Time.time)
            {
                Instantiate(bullet, bulletParent.transform.position, Quaternion.identity);
                nextFireTime = Time.time + fireRate;
                anim.SetBool("Attack", true);
                anim.SetBool("isWalk", false);
            }
            else if (distanceToStartingPos > returnDistance)
            {
                // 돌아가기
                Vector3 direction = (startingPosition - transform.position).normalized;
                transform.position += direction * moveSpeed * Time.deltaTime;

                // 방향을 시작 위치 방향으로 뒤집기
                if (startingPosition.x < transform.position.x)
                    transform.localScale = new Vector3(1f, 1f, 1f);
                else
                    transform.localScale = new Vector3(-1f, 1f, 1f);

                anim.SetBool("isWalk", true);
                anim.SetBool("Attack", false);
            }
            else
            {
                anim.SetBool("isWalk", false);
            }
        }

        if(isFlying == 1)
        {
            
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);


            if (distanceToPlayer <= maxDistance)
            {
                Vector2 direction = (player.position - transform.position).normalized;
                rb.AddForce(direction * moveSpeed, ForceMode2D.Force);

                if (player.position.x < transform.position.x)
                    transform.localScale = new Vector3(1f, 1f, 1f);
                else
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                anim.SetBool("isMove", true);

            }
        }

        if (isFlying == 2)
        {

            float distanceToPlayer = Vector2.Distance(transform.position, player.position);


            if (distanceToPlayer <= followDistance && distanceToPlayer > shootingRange)
            {
                Vector3 direction = (player.position - transform.position).normalized;
                transform.position += direction * moveSpeed * Time.deltaTime;

                if (player.position.x < transform.position.x)
                    transform.localScale = new Vector3(1f, 1f, 1f);
                else
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                anim.SetBool("isMove", true);
                anim.SetBool("isAttack", false);

            }
            else if (distanceToPlayer <= shootingRange && nextFireTime < Time.time)
            {
                Instantiate(bullet, bulletParent.transform.position, Quaternion.identity);
                nextFireTime = Time.time + fireRate;
                anim.SetBool("isMove", false);
                anim.SetBool("isAttack", true);
            }
        }
    }


    public void TakeDamage(int damage)
    {
        currentHP -= damage;  // 적의 체력을 데미지만큼 감소

        // 적이 패배했는지 확인
        if (currentHP <= 0)
        {
            // 적이 패배한 경우, 적의 사망 로직을 처리
            Die();
        }
    }

    void Die()
    {
        // 적의 사망 로직을 처리
        Destroy(gameObject);  // 적 오브젝트를 제거
    }
}