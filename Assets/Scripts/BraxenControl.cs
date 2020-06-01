using UnityEngine;
using System.Collections;

public class BraxenControl : MonoBehaviour {
	public float individualSpeed;
	public float rotation;
	public GameControl gameControl;
	public float birthTime;
	public float fishWidth;
	public float animationSpeed;

	// Use this for initialization
	void Start () {
		GameObject other  = GameObject.FindGameObjectWithTag ("GameController");
		gameControl = other.GetComponent<GameControl>();
		fishWidth = GetComponent<Renderer> ().bounds.extents.x;
		birthTime = Time.time;
		individualSpeed = Random.Range (0.06f,0.12f);	
		float randomRotMagnitude = 0.3f;
		rotation = Random.Range (-randomRotMagnitude, randomRotMagnitude);
		float braxsize = Random.Range (0.8f, 1.2f);
		transform.localScale = new Vector3(-braxsize, braxsize, 1);

		gameObject.GetComponent<Animator> ().speed = Random.Range (0.9f, 1.1f);
	}
	

	void FixedUpdate () {
		transform.Translate (individualSpeed * animationSpeed * gameControl.braxenSpeedMultiplier, 0, 0);
		transform.Rotate (0, 0, rotation);
		if (Mathf.Abs (transform.position.x) > gameControl.levelbounds.x + fishWidth || Mathf.Abs (transform.position.y) > gameControl.levelbounds.y + fishWidth) {
			if (Time.time > birthTime + 2){
				Destroy(gameObject);
				gameControl.braxCount -= 1;
			}
		}

	}

	void OnCollisionEnter(Collision collisionInfo) {
//		if (collisionInfo.gameObject.tag == "Food") {
			collisionInfo.rigidbody.AddForce(10,0,0);
			Debug.Log("Glass har fastnat pa fisk!");
//		}
	}
}
