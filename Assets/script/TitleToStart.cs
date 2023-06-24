using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleToStart : MonoBehaviour
{
    public void loadScence()
    {
        SceneManager.LoadScene("Stage1");
    }

    public void loadTitle()
    {
        SceneManager.LoadScene("Title");
    }
}
