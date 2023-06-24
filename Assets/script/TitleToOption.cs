using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleToOption : MonoBehaviour
{
    // Start is called before the first frame update
    public void loadScence()
    {
        SceneManager.LoadScene("Option");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
