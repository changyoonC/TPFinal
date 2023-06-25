using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;


    public Transform groundChkFront;  // 바닥 체크 position 
    public Transform groundChkBack;   // 바닥 체크 position 
    public Transform WallChk;
    public float wallchkDistance;
    public LayerMask w_Layer;
    public bool isJump;
    public float dashSpeed;
    public bool isSword;


    //기본 공격용 변수
    public int maxHP;  // 플레이어의 최대 체력
    public int currentHP;  // 플레이어의 현재 체력

    public float attackRange=1;  // 플레이어의 공격 범위
    public LayerMask enemyLayer;  // 적 캐릭터를 포함하는 레이어
    private bool canAttack = true;//공격 쿨다운 끝나고 가능
    public float cooldownTime = 1;//공격 쿨다운용

    public int attackDamage;  // 플레이어의 공격 데미지
    public static bool hasWeaponSword;

    //대쉬 체크용 변수들
    private bool isdash;
    public float defaultTime;
    private float defaultSpeed;
    private float dashTime;

    //총알 생성용 변수
    public GameObject bullet;
    public Transform firepos;
    private float cooltimebullet;
    public float bulletDelay;

    public float runSpeed;  // 기본 이동 속도
    float isRight = 1;  // 바라보는 방향 1 = 오른쪽 , -1 = 왼쪽

    float input_x; 
    bool isGround;
    public float chkDistance;
    public float jumpPower = 1;
    public float slidingSpeed=1;
    public LayerMask g_Layer;
    public float wallJumpPower;
    bool isWall;
    public bool isWallJump;

    void Start()
    {
        defaultSpeed = runSpeed;
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHP = maxHP;
    }

    void Update()
    {
        input_x = Input.GetAxis("Horizontal");
        if (isSword == true)
        {
            hasWeaponSword = true;
        }
        if(hasWeaponSword == true)
        {
            isSword = true;
        }

        // 캐릭터의 앞쪽과 뒤쪽의 바닥 체크를 진행
        bool ground_front = Physics2D.Raycast(groundChkFront.position, Vector2.down, chkDistance, g_Layer);
        bool ground_back = Physics2D.Raycast(groundChkBack.position, Vector2.down, chkDistance, g_Layer);

        // 점프 상태에서 앞 또는 뒤쪽에 바닥이 감지되면 바닥에 붙어서 이동하게 변경
        if (!isGround && (ground_front || ground_back))
            rigid.velocity = new Vector2(rigid.velocity.x, 0);

        // 앞 또는 뒤쪽의 바닥이 감지되면 isGround 변수를 참으로!
        if (ground_front || ground_back)
            isGround = true;
        else
            isGround = false;

        anim.SetBool("isGround", isGround);

        isWall=Physics2D.Raycast(WallChk.position,Vector2.right*isRight,wallchkDistance,w_Layer);
        anim.SetBool("isSliding", isWall);//캐릭터가 벽에있을때 slide  하는 조건

        // 공격
        if (Input.GetKeyDown(KeyCode.Z) && canAttack&& hasWeaponSword&&hasWeaponSword)
        {
            Attack();
        }


        //총알 투사체 발사
        if (cooltimebullet <= 0)
        {
            if (Input.GetKey(KeyCode.X))
            {
                Instantiate(bullet, firepos.position, transform.rotation);
            }
            cooltimebullet = bulletDelay;
        }
        cooltimebullet -= Time.deltaTime;



        //캐릭터 대쉬
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isdash = true;
        }
        if (dashTime <= 0)
        {
            defaultSpeed = runSpeed;
            if (isdash)
                dashTime = defaultTime;
        }
        else
        {
            dashTime -= Time.deltaTime;
            defaultSpeed = dashSpeed;
        }
        isdash = false;
        
        
        
        // 스페이스바가 눌리면 점프 애니메이션을 동작
        if (Input.GetAxis("Jump") != 0)
        {
            anim.SetTrigger("jump");
        }

        // 방향키가 눌리는 방향과 캐릭터가 바라보는 방향이 다르다면 캐릭터의 방향을 전환.
        if(!isWallJump)
        if ((input_x > 0 && isRight < 0) || (input_x < 0 && isRight > 0))
        {
            FlipPlayer();
            anim.SetBool("run", true);
        }
        else if (input_x == 0)
        {
            anim.SetBool("run", false);
        }
        // 애니메이션 레이어 변경
        if (hasWeaponSword)
        {
            anim.SetLayerWeight(0, 0); // 첫 번째 레이어 비활성화
            anim.SetLayerWeight(1, 1); // 두 번째 레이어 활성화
        }
        else
        {
            anim.SetLayerWeight(0, 1); // 첫 번째 레이어 활성화
            anim.SetLayerWeight(1, 0); // 두 번째 레이어 비활성화
        }
    }

    private void FixedUpdate()
    {
        // 캐릭터 이동
        if(!isWallJump)
        rigid.velocity = (new Vector2((input_x) * defaultSpeed , rigid.velocity.y));

        if (isGround == true)
        {
            // 캐릭터 점프
            if (Input.GetAxis("Jump") != 0)
            {
                rigid.velocity = Vector2.up * jumpPower;
            }
        }
        if (isWall)
        {
            isWallJump = false;
            rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y * slidingSpeed);
            if (Input.GetAxis("Jump") != 0)
            {
                isWallJump = true;
                Invoke("FreezeX", 0.3f);
                rigid.velocity = new Vector2(-isRight * wallJumpPower, 0.9f * wallJumpPower);
                FlipPlayer();
            }
        }
    }
    void FreezeX()
    {
        isWallJump = false;
    }
    void Attack()
    {
        // 공격을 수행
        anim.SetTrigger("attack");
        canAttack = false;

        // 공격 범위 내에 있는 적을 감지
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);

        // 적에게 데미지를 입힘
        foreach (Collider2D enemy in hitEnemies)
        {
            // 적이 Enemy라는 스크립트를 가지고 있다고 가정하고 TakeDamage() 메서드를 호출합니다.
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
        // 공격이 끝나면 일정 시간 후에 다시 공격할 수 있도록 코루틴을 시작
        StartCoroutine(ResetAttack());
    }
    IEnumerator ResetAttack()
    {
        // 일정 시간 동안 대기합니다.
        yield return new WaitForSeconds(cooldownTime);

        // 공격 가능 상태로 변경합니다.
        canAttack = true;
    }


    public void TakeDamage(int damage)
    {
        currentHP -= damage;  // 플레이어의 체력을 데미지만큼 감소

        // 플레이어가 패배했는지 확인
        if (currentHP <= 0)
        {
            // 플레이어가 패배한 경우, 게임 오버 로직을 처리합니다.
            GameOver();
        }
    }
    void GameOver()
    {
        // 게임 오버 로직을 처리합니다. 예를 들어 게임 오버 화면을 표시하거나 레벨을 재시작할 수 있습니다.
        SceneManager.LoadScene("EndingScene");
    }
    private void OnDrawGizmosSelected()//공격범위 표시
    { 
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }



void FlipPlayer()
    {
        // 방향을 전환.
        transform.eulerAngles = new Vector3(0, Mathf.Abs(transform.eulerAngles.y - 180), 0);
        isRight = isRight * -1;
    }

    // 바닥 체크 Ray를 씬화면에 표시
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(groundChkFront.position, Vector2.down * chkDistance);
        Gizmos.DrawRay(groundChkBack.position, Vector2.down * chkDistance);
    }
}
