using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {
	// Update is called once per frame
	public float speed = 0;
	void Update () {
		transform.Rotate (Vector3.down * speed,Space.World);
	}
}
