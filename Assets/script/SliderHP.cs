using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderHP : MonoBehaviour
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
        maxHP = target.GetComponent<Player>().maxHP;

    }

    // Update is called once per frame
    void Update()
    {
        slider.value = (float)target.GetComponent<Player>().currentHP/maxHP;
    }
}
