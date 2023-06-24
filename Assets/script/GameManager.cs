using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.instance.PlayBGM("MainBgm");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
