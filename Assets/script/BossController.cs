using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossController : MonoBehaviour
{
    public int maxHP;  // 적의 최대 체력
    private int currentHP;  // 적의 현재 체력
    public float moveSpeed = 5f;
    public float followRange = 10f;
    public float chargeSpeed = 1f;
    public float DashSpeed = 15f;
    private float warmUpTime = 0.5f;
    private float chargeTime;
    private float isRight=1;
    private float currentChargeTime;
    public int maxHP;
    public int currHP;

    // 점프 기능 구현
    public LayerMask g_Layer;
    bool isJumping=false;
    public float landingDashSpeed = 10f;//대쉬 속도
    public float landingSpeed = -10f;//점프 아래로 가는 속도
    public float floatingSpeed = 3f;//뜨는속도

    //애니메이션용 변수
    bool isMove;
    bool isP1;
    bool isP2;
    bool isP3;
    bool isIdle;

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


    //애니메이션 호출
    private readonly string animParamMove = "isMove";
    private readonly string animParamPattern1 = "isPattern1";
    private readonly string animParamPattern2 = "isPattern2";
    private readonly string animParamPattern3 = "isPattern3";
    private readonly string animParamIdle = "isIdle";


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
        currHP = maxHP;
        currentState = BossState.Idle;
    }

    private void FixedUpdate()
    {
        timeSinceLastShoot += Time.fixedDeltaTime;
        if (Input.GetKey(KeyCode.P))
        {


            HPminus();
        }
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
                        isMove = true;
                        animator.SetBool("isMove", isMove);
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
                            animator.SetTrigger("isP1");
                        }
                        else if (randomPattern == 2)
                        {
                            currentState = BossState.AttackPattern2;
                            patternDuration = 2;
                            chargeTime = 1.5f;
                            idleDuration = 3;
                            animator.SetTrigger("isP2");

                        }
                        else if (randomPattern == 3) // 패턴 3 추가
                        {
                            currentState = BossState.AttackPattern3;
                            isJumping = true;
                            patternDuration = 4;
                            animator.SetTrigger("isP3");


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
                    animator.SetBool("isMove", false);
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
        if (currHP <= 0)
        {
            SceneManager.LoadScene("Ending");
        }
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

    public void HPminus()
    {
        currHP -= 1;
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