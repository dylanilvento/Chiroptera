using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BatMovement : MonoBehaviour {

	public float jumpSpeed = 5f;
	public float moveSpeed = 2f;
    Animator flyAnim;
    RectTransform healthPool, energyPool;
    GameObject web;

    public AudioClip impact;
    AudioSource ImpactAudio;

    GameObject cam, goal;

	public float damageSpeed = 5f;

	bool pauseControl = false;

    public float health = 100f, energy = 100f;

	//faces right by default
	int directionScale = 1;

	Rigidbody2D rb;
	CircleCollider2D collider;
	SpriteRenderer sr;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		collider = GetComponent<CircleCollider2D>();
		sr = GetComponent<SpriteRenderer>();
        flyAnim = GameObject.Find("bat_spritesheet_0").GetComponent<Animator>();
        healthPool = GameObject.Find("HealthBar").GetComponent<RectTransform>() as RectTransform;
        energyPool = GameObject.Find("EnergyBar").GetComponent<RectTransform>() as RectTransform;
        web = GameObject.Find("Spiderweb");
        cam = GameObject.Find("Main Camera");
        goal = GameObject.FindGameObjectWithTag("Finish");

        ImpactAudio = gameObject.AddComponent<AudioSource>();
        ImpactAudio.playOnAwake = false;
        ImpactAudio.clip = impact;

        //reset light and position
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z+9f);
    }
	
	// Update is called once per frame
	void Update () {
        flyAnim.speed = (1/moveSpeed);

        if (health < 0)
        {
            transform.Find("Light").GetComponent<Light>().intensity = 2f;
            transform.Find("Light").transform.localPosition = new Vector3(transform.Find("Light").transform.localPosition.x, transform.Find("Light").transform.localPosition.y, 0f);
            flyAnim.SetBool("Dead", true);
            GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -1f);
            GetComponent<CircleCollider2D>().enabled = false;
            transform.position = new Vector3(transform.position.x, transform.position.y, -9f);
            GameObject.Find("Main Camera").GetComponent<CameraFollow>().enabled = false;
            if (Mathf.Abs(transform.position.y - cam.transform.position.y) > 10)
                SceneManager.LoadScene("Start");
        }

        if (Vector2.Distance(transform.position, goal.transform.position) < 0.8f)
            SceneManager.LoadScene("Main");
    }

	void FixedUpdate () {

		if (!pauseControl) {

			float xAxis = Input.GetAxis("Horizontal");

			if (Input.GetKey(KeyCode.UpArrow)) {
				rb.velocity = new Vector3(moveSpeed * directionScale, (jumpSpeed/1.5f) * moveSpeed, 0f);
                energy -= 3f;
			}

			// if (Input.GetKeyDown(KeyCode.LeftArrow)) {
			if (xAxis < 0) {
				directionScale = -1;
				transform.localScale = new Vector3(-1, 1, 1);
			}

			// if (Input.GetKeyDown(KeyCode.RightArrow)) {
			if (xAxis > 0) {
				directionScale = 1;
				transform.localScale = new Vector3(1, 1, 1);
			}
		}
        energy = Mathf.Clamp(energy + 1, 0, 100);
        energyPool.sizeDelta = new Vector2(energy, energyPool.sizeDelta.y);
        healthPool.sizeDelta = new Vector2(health, healthPool.sizeDelta.y);
	}

	void BounceBack (Vector3 direction) {
		print("bouncing");
		print("==========");

        ImpactAudio.PlayOneShot(impact, 1f);
        health -= 20f;
		// print("X: " + (direction.x * damageSpeed * -1) + ", Y: " + (direction.y * damageSpeed * -1));
		rb.velocity = new Vector3(direction.x * damageSpeed * -1, direction.y * damageSpeed * -1, 0f);

		//StartCoroutine(PauseControl(0.25f));
		StartCoroutine("BlinkInjury");
	}

	void OnCollisionEnter2D (Collision2D other) {
		

		RaycastHit2D hit;

		Vector3[] directions = {
			Vector3.up,
			Vector3.right,
			Vector3.down,
			Vector3.left,
			new Vector3 (1, 1, 0),
			new Vector3 (1, -1, 0),
			new Vector3 (-1, 1, 0),
			new Vector3 (-1, -1, 0)
		};

		RaycastHit[] rayHitGroup = new RaycastHit[directions.Length];

		Vector3 hitDirection = Vector3.zero;

		foreach (Vector2 dir in directions) {
			Debug.DrawRay(new Vector2(transform.position.x, transform.position.y) + dir * (collider.bounds.size.magnitude/2), dir * 5, Color.green, 100f);
		}

		print("shooting rays");

		for (int ii = 0; ii < rayHitGroup.Length; ii++) {
			// Physics.Raycast(transform.position, directions[ii], out rayHitGroup[ii], 5f);
			hit = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z) + directions[ii] * (collider.bounds.size.magnitude/2), directions[ii], 5f);
			// print(hit == null);


			//Printing for ID
			// if (hit.collider != null) print(hit.transform.gameObject.GetInstanceID());
			// else print("null");
			// print(other.gameObject.GetInstanceID());

			//Printing for name
			if (hit.collider != null) print(hit.transform.gameObject.name);
			else print("null");
			print(other.gameObject.name);
			
			
			if (hit.collider != null && hit.transform.gameObject.name.Equals(other.gameObject.name)) {
			// if (hit.collider != null && hit.transform.gameObject.GetInstanceID() == other.gameObject.GetInstanceID()) {
				hitDirection = directions[ii];
				BounceBack(hitDirection);
				// print(hitDirection);
				break;
			}
			print("==========");
			// print(hit);

		}

		
	}

	IEnumerator PauseControl(float t) {
		pauseControl = true;
		yield return new WaitForSeconds(t);
		pauseControl = false;
	}

	IEnumerator BlinkInjury() {
		for (int ii = 0; ii < 5; ii++) {
			sr.color = Color.red;
			yield return new WaitForSeconds(0.5f);
			sr.color = Color.white;
			yield return new WaitForSeconds(0.5f);
		}
	}
}
