using UnityEngine;
using System.Collections;

public class OpenableDoor : MonoBehaviour {
	public int doorOpenAngle = -90;
	private bool playerEnter, doorOpen;
	public float openSpeed = 2;
	private Vector3 closedRotation, openRotation;
	public GameObject monsterPrefab;
	public int monsterRotation = 90;
	public AudioClip doorCreak1;
	public AudioClip doorCreak2;
	public AudioClip monsterGrowl;
	private int counter = 0;
	private AudioSource source;
	private float volLowRange = .5f;
	private float volHighRange = 1.0f;

	void Start () {
		// Obtains the default x, y, z rotational values for a closed door
		closedRotation = transform.eulerAngles;
		// Sets the rotation on the door when opening
		openRotation = new Vector3 (closedRotation.x, closedRotation.y + doorOpenAngle, closedRotation.z);
		source = GetComponent<AudioSource>(); 
	}
	
	// Update is called once per frame
	void Update () {
		// If the door flag is true, open the door
		if (doorOpen) {
			transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, openRotation, Time.deltaTime * openSpeed);
			Destroy (GameObject.FindGameObjectWithTag("Respawn"), (float).6);
		} 
		// If the door flag is false, close the door
		else {
			transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, closedRotation, Time.deltaTime * openSpeed);
		}
		// Toggle door state if user is within the collision box
		if (Input.GetKeyDown (KeyCode.E) && playerEnter) {
			doorOpen = !doorOpen;
			source.PlayOneShot (monsterGrowl, volHighRange);

			float vol = Random.Range(volLowRange, volHighRange);
			int doorSoundSelect = Random.Range(1, 3);

			if (doorSoundSelect == 1)
				source.PlayOneShot(doorCreak1, vol);
			else 
				source.PlayOneShot(doorCreak2, vol);
		}
	}
	 

	// Toggle entry flag when user within close proximity of the door
	void OnTriggerEnter(Collider player) {
		if (player.gameObject.tag == "Player")
			playerEnter = true;
		if (counter < 1) {
			GameObject monster = Instantiate (monsterPrefab, new Vector3 (transform.position.x - (float)1.1, transform.position.y - 2, transform.position.z + 2), Quaternion.Euler (0, monsterRotation, 0)) as GameObject;
			monster.tag = "Respawn";
			if (doorOpen)
				source.PlayOneShot (monsterGrowl, volHighRange);

		}


		counter++;

	}
	
	void OnTriggerExit(Collider player) {
		if (player.gameObject.tag == "Player") {
			playerEnter = false;

		}
	}
	
}
