using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {


	public float playerSpeed;
	public Vector3 resolution;
	public Vector3 screenBounds;
	public Camera cam;

	public GameControl gameControl;
	public float targetAngle;
	public float oldAngle;
	public float lerpedAngle;
	public float angleSmoothing;
	public Sprite[] sprites;
	public int arrayIndex;
	public int comboCount;
	public float comboTimer;
	public AudioSource[] ljud;

	float comboTime;


	// Use this for initialization
	void Start () {
		if (cam == null) {
			cam = Camera.main;
		}
		angleSmoothing = 20;
		playerSpeed = 35;
		comboCount = 0;
		comboTimer = -10;

		resolution = new Vector3(Screen.width, Screen.height, 0);
		screenBounds = cam.ScreenToWorldPoint (resolution);
//		sprites = Resources.LoadAll<Sprite>(@"IMG/brax_seq/brax3");
		GetComponent<SpriteRenderer> ().sprite = sprites [8];
		ljud = GetComponents<AudioSource> ();


	}




	void FixedUpdate () {
		if (gameControl.livesRemaining > 0) {
			if (Input.acceleration.magnitude > 0.1f){
				float strutExtents = 0.6f;



				Vector3 clampedinput = Vector3.ClampMagnitude(Input.acceleration, 0.5f);
				transform.Translate (clampedinput.x * Time.deltaTime * playerSpeed, clampedinput.y * Time.deltaTime * playerSpeed, 0, Space.World);
				if (transform.position.x < -screenBounds.x+strutExtents)  {
					transform.position = new Vector3(-screenBounds.x+strutExtents, transform.position.y, 0);
				}
				else if (transform.position.x > screenBounds.x-strutExtents) {
					transform.position = new Vector3(screenBounds.x-strutExtents, transform.position.y, 0);
				}
				if (transform.position.y < -screenBounds.y+strutExtents)  {
					transform.position = new Vector3(transform.position.x, -screenBounds.y+strutExtents, 0);
				}
				else if (transform.position.y > screenBounds.y-strutExtents) {
					transform.position = new Vector3(transform.position.x, screenBounds.y-strutExtents, 0);
				}
			

			targetAngle = Mathf.Atan2(clampedinput.y, clampedinput.x) * Mathf.Rad2Deg - 90;
			lerpedAngle = Mathf.LerpAngle(oldAngle, targetAngle, angleSmoothing * Time.deltaTime);
			arrayIndex = Mathf.RoundToInt ((targetAngle + 270)/22.5f); 
			GetComponent<SpriteRenderer> ().sprite = sprites [arrayIndex];
			transform.rotation = Quaternion.AngleAxis(lerpedAngle, Vector3.forward);
			oldAngle = lerpedAngle;
			}
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Enemy") {
			ljud[2].panStereo = transform.position.x.Remap(-screenBounds.x,screenBounds.x,-1,1);
			gameControl.LoseLife();


		}
		if (coll.gameObject.tag == "Food") {
			ljud[0].panStereo = transform.position.x.Remap(-screenBounds.x,screenBounds.x,-1,1);
			int scoreaddition = Mathf.RoundToInt (coll.gameObject.GetComponent<GlasskuleBehaviour>().size * 100);
			if (Time.time < comboTimer + gameControl.comboTime){
				comboCount += 1;
			}
			else{
				comboCount = 0;
			}
			comboTimer = Time.time;
			gameControl.UpdateScore (scoreaddition, comboCount);
			gameControl.SpawnPopup(coll.gameObject.transform.position, scoreaddition, comboCount);
			Destroy (coll.gameObject);
			gameControl.SpawnGlasskula (0);

			ljud[0].pitch = 1 + (comboCount*0.2f);
			ljud[0].Play();

			if (comboCount == 2){
				ljud[1].Play ();
			}

		}

	}

	void OnTriggerEnter2D(Collider2D coll) {


	}

	



}