using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlay : MonoBehaviour
{
    public AudioSource audio;

	void Start ()
    {
        AudioSource audio = gameObject.GetComponent<AudioSource>();
	}
	
	void Update ()
    {

        audio.Play();
    }
}
