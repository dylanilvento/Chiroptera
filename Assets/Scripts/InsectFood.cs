using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsectFood : MonoBehaviour
{

	void Awake ()
    {
		
	}
	
	void Update ()
    {
		
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<BatMovement>().health = Mathf.Clamp(col.gameObject.GetComponent<BatMovement>().health+10, 0, 100f);
            Destroy(gameObject);
        }
    }
}
