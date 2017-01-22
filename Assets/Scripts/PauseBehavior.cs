using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseBehavior : MonoBehaviour
{
    GameObject pauseBackground;

	void Start ()
    {
        pauseBackground = GameObject.Find("PauseBackground");
        pauseBackground.SetActive(false);
	}

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            if (Time.timeScale == 1f)
            {
                //Stop time
                Time.timeScale = 0f;
                //Display Pause UI element
                pauseBackground.SetActive(true);
            }

        if (Time.timeScale == 0f)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(0);
                Time.timeScale = 1f;
            }
                if (Input.GetKeyUp(KeyCode.Space))
            {
                //Return to normal time
                Time.timeScale = 1f;
                //Hide Pause UI element
                pauseBackground.SetActive(false);
            }
        }
    }
}
