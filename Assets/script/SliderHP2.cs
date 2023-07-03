using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderHP2 : MonoBehaviour
{

    GameObject target;
    public int currHP;
    public int maxHP;

    public Slider slider;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        maxHP = target.GetComponent<Player1>().maxHP;

    }

    // Update is called once per frame
    void Update()
    {
        slider.value = (float)target.GetComponent<Player1>().currentHP/maxHP;
    }
}
