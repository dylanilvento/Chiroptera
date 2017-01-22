using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonarPulse : MonoBehaviour
{

    float scaleMin = 0.1f, scaleMax = 1f, scaleIncrement = 0.02f, curScale = 0.1f;

    void Awake ()
    {
        transform.localScale = new Vector2(scaleMin, scaleMin);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -9);
    }
	
	void Update ()
    {
        curScale += scaleIncrement;
        transform.localScale = new Vector2(transform.localScale.x + scaleIncrement, transform.localScale.y + scaleIncrement);
        if (curScale > 1)
            Destroy(gameObject);
    }
}
