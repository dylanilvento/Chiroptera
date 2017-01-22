using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderScript : MonoBehaviour
{
    GameObject spider, bat;

    bool attacking = false;

    void Awake()
    {
        spider = GameObject.Find("Spider");
        bat = GameObject.Find("\"Bat\"");
    }

    void Update()
    {
        //if player within x units from center of web, spider move towards player, stop if within y range and start dealing damage
        if (attacking)
            bat.transform.GetComponent<BatMovement>().health -= 0.1f;
    }

    void OnTriggerEnter2D()
    {
        bat.transform.GetComponent<BatMovement>().moveSpeed = 0.4f;
        attacking = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        bat.transform.GetComponent<BatMovement>().moveSpeed = 2f;
        attacking = false;
    }
}
