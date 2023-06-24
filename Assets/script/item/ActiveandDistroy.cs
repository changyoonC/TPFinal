using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveandDistroy : MonoBehaviour
{
    public GameObject objectToShow;
    private bool isCollisionDetected = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �浹�� �߻��ϸ� ������Ʈ�� Ȱ��ȭ�ϰ� Ÿ�̸Ӹ� �����մϴ�.
        if (!isCollisionDetected && collision.gameObject.CompareTag("Player"))
        {
            isCollisionDetected = true;
            objectToShow.SetActive(true);
            Invoke("DeactivateObject", 5f); // 5�� �Ŀ� DeactivateObject �Լ��� ȣ���մϴ�.
            this.gameObject.SetActive(false);
        }
    }

    private void DeactivateObject()
    {
        // Ÿ�̸Ӱ� �Ϸ�Ǹ� ������Ʈ�� ��Ȱ��ȭ�մϴ�.
        objectToShow.SetActive(false);
    }
}