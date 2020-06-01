using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CrabAI2 : MonoBehaviour {

	public GameObject player;
	public Text debugtext;
	public float sideSpeed;
	public float forwardSpeed;
	float angle;
	float crabSpeed;
	public float escapeSpeed;
	GameControl gameControl; 
	public float rotateSpeed;
	public AudioSource ljud;
	bool endingLife;
	float volumeDecrease;

	
	private Vector3 velocity = Vector3.zero;


	// Use this for initialization
	void Start () {
		rotateSpeed = 0.5f;
		GameObject other  = GameObject.FindGameObjectWithTag ("GameController");
		gameControl = other.GetComponent<GameControl>();
		sideSpeed = 6f;
		player = GameObject.FindGameObjectWithTag ("Player");
		if (gameControl.hardMode == true) {
			forwardSpeed = 1.0f;
			sideSpeed = 8f;
		} else {
			forwardSpeed = 0.5f;
			sideSpeed = 5f;
		}
		ljud = GetComponent<AudioSource> ();
		endingLife = false;
	}



	// Update is called once per frame
//	void FixedUpdate () {
//		if (stage == 1) {
//			Vector3 offset = player.transform.position - transform.position;
//			float forwardDistance = Vector3.Dot(transform.up, offset);
//			float sideDistance = Vector3.Dot(transform.right, offset);
//			Vector3 crabTargetPos = transform.position + transform.right * sideDistance;
//		}
//	}

	void FixedUpdate () {

		Vector3 offset = player.transform.position - transform.position;
		float sideDistance = Vector3.Dot(transform.right, offset);
		Vector3 crabTargetPos = transform.position + transform.right * sideDistance + transform.up * forwardSpeed;
		transform.position = Vector3.SmoothDamp(transform.position, crabTargetPos, ref velocity, 0.3f, sideSpeed);
		transform.Rotate(0,0,Mathf.Sign(sideDistance) * rotateSpeed );


		if (transform.position.y > 6 && endingLife == false){ // Checks if player is above the screen, and changes the stage.
			gameControl.crabLeft = Time.time;
			gameControl.crabExists = false;
			Destroy (gameObject,1);
			endingLife = true;
			volumeDecrease = ljud.volume;

		}

		if (endingLife == true) {
			ljud.volume = ljud.volume - Time.deltaTime*volumeDecrease;
		}


	}
}
