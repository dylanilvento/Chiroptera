using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{

	void Awake ()
    {
		
	}
	
	void Update ()
    {
        if (Input.GetKeyUp(KeyCode.Space))
            SceneManager.LoadScene("Main");
	}
}
