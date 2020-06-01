using UnityEngine;
using System.Collections;

public class GlasskuleBehaviour : MonoBehaviour {

	
	public float size;
	public GameControl gameControl;
	public float meltspeed;
	public Vector3 screenBounds;
	public Vector3 resolution;

	// Use this for initialization
	void Start () {
		resolution = new Vector3(Screen.width, Screen.height, 0);
		screenBounds = 	Camera.main.ScreenToWorldPoint (resolution);
		size = 1f;
		meltspeed = 0.2f;
		//GameObject other  = GameObject.FindGameObjectWithTag ("GameController");
		//gameControl = other.GetComponent<GameControl>();
        gameControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
    }
	
	// Update is called once per frame
	void Update () {
			float finalsize = size * 2;
			float kulExtents = 0.4f * size;
			transform.localScale = new Vector3 (finalsize, finalsize, finalsize);
		if (transform.position.x < -screenBounds.x+kulExtents )  {
			transform.position = new Vector3(-screenBounds.x+kulExtents , transform.position.y, 0);
		}
		else if (transform.position.x > screenBounds.x-kulExtents ) {
			transform.position = new Vector3(screenBounds.x-kulExtents , transform.position.y, 0);
		}
		if (transform.position.y < -screenBounds.y+kulExtents )  {
			transform.position = new Vector3(transform.position.x, -screenBounds.y+kulExtents , 0);
		}
		else if (transform.position.y > screenBounds.y-kulExtents ) {
			transform.position = new Vector3(transform.position.x, screenBounds.y-kulExtents , 0);
		}
	}
}
