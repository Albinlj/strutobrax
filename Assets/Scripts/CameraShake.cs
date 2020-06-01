using UnityEngine;
using System;
using System.Collections;




public class CameraShake : MonoBehaviour {

	public float shake;
	public float decreaseAmount;
	public Vector3 originalPos;
	// Use this for initialization
	void Start () {
		originalPos = transform.position;
		shake = 0;
		decreaseAmount = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {

		if (shake > 0) {
			
			transform.position = Vector3.Lerp (transform.position, originalPos + UnityEngine.Random.insideUnitSphere * shake, 3);
			
			shake -= Time.deltaTime * decreaseAmount;
			
			
		} else {
			shake = 0.0f;
		}
	}
}
