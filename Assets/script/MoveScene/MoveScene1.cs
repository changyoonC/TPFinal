using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScene1 : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player"&&SceneManager.GetActiveScene().name=="Stage1")
        {
            SceneManager.LoadScene("Stage2");
        }else if(collision.collider.tag == "Player" && SceneManager.GetActiveScene().name == "Stage2")
        {
            SceneManager.LoadScene("LastStage");
        }
    }
}
