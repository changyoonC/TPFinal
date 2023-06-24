using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHP;  // ���� �ִ� ü��
    private int currentHP;  // ���� ���� ü��

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


    void Start()
    {
        currentHP = maxHP;
        startingPosition = transform.position;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        float distanceToStartingPos = Vector3.Distance(transform.position, startingPosition);

        if (distanceToPlayer <= followDistance && distanceToPlayer > shootingRange)
        {
            // �÷��̾� ���󰡱�
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;

            // ������ �÷��̾� �������� ������
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
            // ���ư���
            Vector3 direction = (startingPosition - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;

            // ������ ���� ��ġ �������� ������
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


    public void TakeDamage(int damage)
    {
        currentHP -= damage;  // ���� ü���� ��������ŭ ����

        // ���� �й��ߴ��� Ȯ��
        if (currentHP <= 0)
        {
            // ���� �й��� ���, ���� ��� ������ ó��
            Die();
        }
    }

    void Die()
    {
        // ���� ��� ������ ó��
        Destroy(gameObject);  // �� ������Ʈ�� ����
    }
}