using UnityEngine;
using System.Collections;

public class DestroyTimed : MonoBehaviour {
	

	void Awake()
	{
		Destroy(gameObject, 3);
	}

}
