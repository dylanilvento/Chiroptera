using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Echolocate : MonoBehaviour
{
    public GameObject lightPrefab, sonarCirclePrefab;
    GameObject batlight;
    GameObject[] lightProjectiles = new GameObject[16];

    Vector3 spawnPos = new Vector3(0, 0, -3.75f);
    float magnitude = 20f, dist;
    BatMovement batClass;

    public AudioClip sonar;
    AudioSource audio;

	void Awake ()
    {
        audio = GetComponent<AudioSource>();
        batClass = GameObject.Find("\"Bat\"").GetComponent<BatMovement>();
        for (int i = 0; i < lightProjectiles.Length; i++)
        {
            spawnPos = (Quaternion.AngleAxis(22.5f * i, Vector3.forward) * Vector3.right)*100f;
            lightProjectiles[i] = Instantiate(lightPrefab, spawnPos, Quaternion.identity) as GameObject;
            lightProjectiles[i].name = (22.5f * i).ToString();
            lightProjectiles[i].transform.GetChild(0).GetComponent<Light>().intensity = 0;
            lightProjectiles[i].transform.GetChild(0).GetComponent<Light>().range = 0;
        }
        Pulse();
	}
	
	void Update ()
    {
        if (Input.GetKeyUp(KeyCode.Space) && batClass.energy > 20f)
            Pulse();

        for (int i = 0; i < lightProjectiles.Length; i++)
        {
            dist = Vector2.Distance(lightProjectiles[i].transform.position, transform.position);
            if (dist > 25
                && dist < 50)
            {
                lightProjectiles[i].transform.position = (Quaternion.AngleAxis(22.5f * i, Vector3.forward) * Vector3.right) * 100f;
                lightProjectiles[i].transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                lightProjectiles[i].transform.GetChild(0).GetComponent<Light>().intensity = 0;
                lightProjectiles[i].transform.GetChild(0).GetComponent<Light>().range = 0;
            }
        }
	}

    void Pulse()
    {
        batClass.energy -= 20f;
        audio.PlayOneShot(sonar, 1f);
        Instantiate(sonarCirclePrefab, gameObject.transform.position, Quaternion.identity);
        for (int i = 0; i < lightProjectiles.Length; i++)
        {
            lightProjectiles[i].transform.GetChild(0).GetComponent<Light>().intensity = 0;
            lightProjectiles[i].transform.GetChild(0).GetComponent<Light>().range = 0;
            lightProjectiles[i].transform.GetComponent<Rigidbody2D>().position = transform.position + (Quaternion.AngleAxis(22.5f * i, Vector3.forward) * Vector3.right)*1.5f;
            print(lightProjectiles[i].transform.name);
            lightProjectiles[i].transform.GetComponent<Rigidbody2D>().velocity = (Quaternion.AngleAxis(int.Parse(lightProjectiles[i].transform.name.Split('.')[0]), Vector3.forward) * Vector3.right) * magnitude;
        }
    }
}
