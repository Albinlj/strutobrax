using UnityEngine;
using System.Collections;

public class Popup : MonoBehaviour {
	public AnimationClip animationx;
	// Use this for initialization
	void Start () {
		StartCoroutine ("Die");
	}

	private IEnumerator Die()
	{
		yield return new WaitForSeconds(animationx.length-0.1f);
		Destroy(gameObject);
	}	
}
