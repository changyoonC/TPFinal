using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveandDistroy : MonoBehaviour
{
    public GameObject objectToShow;
    private bool isCollisionDetected = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 충돌이 발생하면 오브젝트를 활성화하고 타이머를 시작합니다.
        if (!isCollisionDetected && collision.gameObject.CompareTag("Player"))
        {
            isCollisionDetected = true;
            objectToShow.SetActive(true);
            Invoke("DeactivateObject", 5f); // 5초 후에 DeactivateObject 함수를 호출합니다.
            this.gameObject.SetActive(false);
        }
    }

    private void DeactivateObject()
    {
        // 타이머가 완료되면 오브젝트를 비활성화합니다.
        objectToShow.SetActive(false);
    }
}