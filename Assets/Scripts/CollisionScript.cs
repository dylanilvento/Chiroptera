using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionScript : MonoBehaviour
{
    Collision2D collider;
    AudioSource audio;
    public AudioClip pulse;

	void Awake ()
    {
        audio = GetComponent<AudioSource>();
	}
	
	void Update ()
    {
		
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        collider = col;
        transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        transform.GetChild(0).GetComponent<Light>().intensity = 1;
        transform.GetChild(0).GetComponent<Light>().range = 10;
        audio.PlayOneShot(pulse, 0.5f);
        StartCoroutine(Burnout(0.5f));
    }

    IEnumerator Burnout(float time)
    {
        transform.GetChild(0).GetComponent<Light>().intensity -= 0.01f;
        transform.GetChild(0).GetComponent<Light>().range -= 0.1f;
        yield return new WaitForSeconds(time);
        transform.position = (Quaternion.AngleAxis(int.Parse(transform.name.Split('.')[0]), Vector3.forward) * Vector3.right) * 100f;
        transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        transform.GetChild(0).GetComponent<Light>().intensity = 0;
        transform.GetChild(0).GetComponent<Light>().range = 0;
    }
}
