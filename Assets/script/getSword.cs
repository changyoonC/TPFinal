using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getSword : MonoBehaviour
{
    private bool isCollisionDetected = false;
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 충돌이 발생하면 오브젝트를 활성화하고 타이머를 시작합니다.
        if (!isCollisionDetected && collision.gameObject.CompareTag("Player"))
        {
            Player.hasWeaponSword = true;
        }
    }
}
