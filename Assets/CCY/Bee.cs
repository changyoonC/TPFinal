using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed = 3f; 
    public float maxDistance = 10f; 
    public Transform player; 
    private Rigidbody2D rb;

    public int maxHP;  // 적의 최대 체력
    private int currentHP;  // 적의 현재 체력

    void Start()
    {
        currentHP = maxHP;
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
