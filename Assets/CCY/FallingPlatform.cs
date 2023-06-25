using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    private Vector3 initialPosition;
    public bool isShaking;
    private bool isFalling;
    private float shakeDuration = 3f;
    private float fallDuration = 0.5f;
    private float respawnDelay = 5f;
    

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        if (isShaking)
        {
            ShakePlatform();
        }
        else if (isFalling)
        {
            FallPlatform();
        }
        else
        {
            RespawnPlatform();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isShaking = true;
        }
    }

    private void ShakePlatform()
    {
        // 흔들림 로직을 구현하세요. 예를 들어, 플랫폼을 랜덤하게 이동시키는 방식으로 구현할 수 있습니다.

        // 흔들림이 지속한 시간을 체크하고, 흔들림이 끝났을 때 떨어지도록 상태를 변경하세요.
        if (Time.time >= shakeDuration)
        {
            isShaking = false;
            isFalling = true;
        }
    }

    private void FallPlatform()
    {
        // 플랫폼을 아래로 이동시키는 로직을 구현하세요. 예를 들어, Rigidbody2D를 사용하여 중력에 따라 떨어지도록 할 수 있습니다.
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 1f;  // 중력을 활성화하여 플랫폼이 아래로 떨어지도록 합니다.

        // 일정 시간이 지나면 플랫폼을 다시 생성하기 위해 상태를 변경하세요.
        if (Time.time >= shakeDuration + fallDuration)
        {
            rb.gravityScale = 0f;  // 중력을 비활성화하여 플랫폼이 멈추도록 합니다.
            rb.velocity = Vector2.zero;  // 속도를 0으로 설정하여 플랫폼이 정지하도록 합니다.
            isFalling = false;
        }
    }

    private void RespawnPlatform()
    {
        // 플랫폼을 초기 위치로 이동시키고 상태를 재설정하세요.
        transform.position = initialPosition;
        isShaking = false;
        isFalling = false;

        // 일정 시간이 지나면 플랫폼을 다시 흔들기 위해 상태를 변경하세요.
        if (Time.time >= shakeDuration + fallDuration + respawnDelay)
        {
            isShaking = true;
        }
    }
}