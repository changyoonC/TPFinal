using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getSword : MonoBehaviour
{
    private bool isCollisionDetected = false;
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �浹�� �߻��ϸ� ������Ʈ�� Ȱ��ȭ�ϰ� Ÿ�̸Ӹ� �����մϴ�.
        if (!isCollisionDetected && collision.gameObject.CompareTag("Player"))
        {
            Player.hasWeaponSword = true;
        }
    }
}
