using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    public GameObject item;
    Rigidbody2D itemRB;
    GameObject target;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        itemRB = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "player")
        {
            target.GetComponent<Player>().TakeDamage(-1);
            Destroy(gameObject);

        }
    }
}
