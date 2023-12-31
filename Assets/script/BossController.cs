using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float followRange = 10f;
    public float chargeSpeed = 1f;
    public float DashSpeed = 15f;
    private float warmUpTime = 0.5f;
    private float chargeTime;
    private float isRight=1;
    private float currentChargeTime;

    // 점프 기능 구현
    public LayerMask g_Layer;
    bool isJumping=false;
    public float landingDashSpeed = 10f;//대쉬 속도
    public float landingSpeed = -10f;//점프 아래로 가는 속도
    public float floatingSpeed = 3f;//뜨는속도

    public Transform player;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float projectileSpeed = 10f;
    public float shootDelay = 1f;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 currentDirection;
    private float timeSinceLastShoot;
    private float patternDuration;
    private float idleTime;
    private float idleDuration;

    public float airMovementSpeed = 5f; // 공중에서의 이동 속도
    public float gravityScale = 2f; // 아래로의 중력 스케일

    public float patternChangeCooldown = 3f; // 패턴 전환 쿨타임

    private float patternChangeTimer=3; // 패턴 전환 타이머

    private enum BossState
    {
        NormalChase,
        Idle,
        AttackPattern1,
        AttackPattern2,
        AttackPattern3
    }

    private BossState currentState;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentDirection = Vector2.right;
        timeSinceLastShoot = shootDelay;
        patternDuration = Random.Range(2f, 5f);
        idleDuration = Random.Range(1f, 3f);

        currentState = BossState.Idle;
    }

    private void FixedUpdate()
    {
        timeSinceLastShoot += Time.fixedDeltaTime;

        switch (currentState)
        {
            case BossState.NormalChase:
                if (Mathf.Abs(transform.position.x-player.position.x) <= followRange)
                {
                    ChangeDirection();
                    Move();
                    // 패턴 전환 쿨타임이 지나지 않았으면 리턴하여 패턴 전환을 막음
                    if (patternChangeTimer > 0f)
                    {
                        patternChangeTimer -= Time.fixedDeltaTime;
                        return;
                    }
                    else
                    {

                        int randomPattern = Random.Range(1, 4); // 패턴 3 추가

                        if (randomPattern == 1)
                        {
                            currentState = BossState.AttackPattern1;
                            patternDuration = 2;
                            idleDuration = 3;
                        }
                        else if (randomPattern == 2)
                        {
                            currentState = BossState.AttackPattern2;
                            patternDuration = 2;
                            chargeTime = 1.5f;
                            idleDuration = 3;
                        }
                        else if (randomPattern == 3) // 패턴 3 추가
                        {
                            currentState = BossState.AttackPattern3;
                            isJumping = true;
                            patternDuration = 6;
                          
                            idleDuration = 3;
                        }


                        // 패턴 전환 후 쿨타임 초기화
                        patternChangeTimer = patternChangeCooldown;
                    }
                }
                else
                {
                    //ChangeDirection();
                    //Move();
                }
                break;
            case BossState.Idle:
                Debug.Log("idle");
                idleTime += Time.fixedDeltaTime;
                if (idleTime >= idleDuration)
                {
                    currentState = BossState.NormalChase;
                    idleTime = 0f;
                }
                else
                {
                    StopMoving();
                    ChangeDirection();
                }
                break;
            case BossState.AttackPattern1:
                Debug.Log("1");
                StopMoving();
                if (timeSinceLastShoot >= shootDelay)
                {
                    ShootProjectile();
                    timeSinceLastShoot = 0f;
                }
                if (patternDuration <= 0f)
                {
                    currentState = BossState.Idle;
                }
                break;
            case BossState.AttackPattern2:
                Debug.Log("2");
                if (chargeTime <= 0f)
                {
                    currentState = BossState.Idle;
                }
                else
                {
                    ChargeAttack();
                }
                if (patternDuration <= 0f)
                {
                    currentState = BossState.Idle;
                }
                break;
            case BossState.AttackPattern3://점프이동
                Debug.Log("3");
                if (isJumping)
                {
                    JumpBackwards();
                }
                
                if (patternDuration <= 0f)
                {
                    currentState = BossState.Idle;
                }
                break;

        }
        rb.velocity += new Vector2(0f, Physics2D.gravity.y * gravityScale * Time.fixedDeltaTime);//중력적용




        patternDuration -= Time.fixedDeltaTime;
        chargeTime -= Time.fixedDeltaTime;
    }



    private void ChangeDirection()
    {
        Vector2 targetDirection = player.position - transform.position;
        targetDirection.y = 0f; //
        targetDirection.Normalize();

        if (currentDirection != targetDirection)
        {
            currentDirection = targetDirection;

            if (currentDirection.x > 0)
            {
                transform.rotation = Quaternion.LookRotation(Vector3.back);
            }
            else if (currentDirection.x < 0)
            {
                transform.rotation = Quaternion.LookRotation(Vector3.forward);
            }
        }
    }


    private void Move()
    {
        rb.velocity = currentDirection * moveSpeed;
    }

    private void StopMoving()
    {
        rb.velocity = Vector2.zero;
    }

    private void ShootProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);

        BulletControllerBoss bulletController = projectile.GetComponent<BulletControllerBoss>();
        if (bulletController != null)
        {
            Vector2 bulletDirection = (transform.rotation * Vector3.right).normalized;
            bulletController.SetDirection(bulletDirection);
        }

        Destroy(projectile, 5f);
    }

    private void ChargeAttack()
    {
        // 플레이어 방향 측정
        Vector2 targetDirection = player.position - transform.position;
        targetDirection.y = 0f;
        targetDirection.Normalize();

        if (currentDirection != targetDirection)
        {
            currentDirection = targetDirection;

            if (currentDirection.x > 0)
            {
                transform.rotation = Quaternion.LookRotation(Vector3.back);
            }
            else if (currentDirection.x < 0)
            {
                transform.rotation = Quaternion.LookRotation(Vector3.forward);
            }
        }

        // 대기시간 전
        if (currentChargeTime < warmUpTime)
        {
            rb.velocity = -currentDirection * chargeSpeed;
        }
        // 돌진
        else if (currentChargeTime < warmUpTime + chargeTime)
        {
            rb.velocity = currentDirection * DashSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        currentChargeTime += Time.fixedDeltaTime;

        
        if (currentChargeTime >= warmUpTime + chargeTime)
        {
            currentState = BossState.Idle;
            currentChargeTime = 0;
        }
    }
    private void JumpBackwards()
    {
        //점프속도계산
        Vector2 jumpVelocity = Vector2.up * floatingSpeed;

        // 점프 속도넣기
        rb.velocity = jumpVelocity;

        //뜨기 1초 기다리기
        StartCoroutine(LandAfterDelay());
    }

    private IEnumerator LandAfterDelay()
    {
        yield return new WaitForSeconds(1f);

        Vector2 directionToPlayer = (player.position - transform.position).normalized;

        //방향 미리정해두기
        currentDirection = directionToPlayer;

       
        if (currentDirection.x > 0)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.back);
        }
        else if (currentDirection.x < 0)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward);
        }

       //전체 벡터계산
        Vector2 landingVelocity = new Vector2(currentDirection.x * landingDashSpeed, landingSpeed); // Adjust the landing speed as needed

        rb.velocity = landingVelocity;

        isJumping = false;

        //점프 체크하고 다시 idle 로 보내기

        bool isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1f, g_Layer);

        if (isGrounded)
        {
            StopMoving(); // Activate StopMoving when grounded
            currentState = BossState.Idle;
        }
    }

}