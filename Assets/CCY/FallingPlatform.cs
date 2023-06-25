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
        // ��鸲 ������ �����ϼ���. ���� ���, �÷����� �����ϰ� �̵���Ű�� ������� ������ �� �ֽ��ϴ�.

        // ��鸲�� ������ �ð��� üũ�ϰ�, ��鸲�� ������ �� ���������� ���¸� �����ϼ���.
        if (Time.time >= shakeDuration)
        {
            isShaking = false;
            isFalling = true;
        }
    }

    private void FallPlatform()
    {
        // �÷����� �Ʒ��� �̵���Ű�� ������ �����ϼ���. ���� ���, Rigidbody2D�� ����Ͽ� �߷¿� ���� ���������� �� �� �ֽ��ϴ�.
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 1f;  // �߷��� Ȱ��ȭ�Ͽ� �÷����� �Ʒ��� ���������� �մϴ�.

        // ���� �ð��� ������ �÷����� �ٽ� �����ϱ� ���� ���¸� �����ϼ���.
        if (Time.time >= shakeDuration + fallDuration)
        {
            rb.gravityScale = 0f;  // �߷��� ��Ȱ��ȭ�Ͽ� �÷����� ���ߵ��� �մϴ�.
            rb.velocity = Vector2.zero;  // �ӵ��� 0���� �����Ͽ� �÷����� �����ϵ��� �մϴ�.
            isFalling = false;
        }
    }

    private void RespawnPlatform()
    {
        // �÷����� �ʱ� ��ġ�� �̵���Ű�� ���¸� �缳���ϼ���.
        transform.position = initialPosition;
        isShaking = false;
        isFalling = false;

        // ���� �ð��� ������ �÷����� �ٽ� ���� ���� ���¸� �����ϼ���.
        if (Time.time >= shakeDuration + fallDuration + respawnDelay)
        {
            isShaking = true;
        }
    }
}