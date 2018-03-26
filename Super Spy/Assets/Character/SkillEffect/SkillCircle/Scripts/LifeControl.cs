using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LifeControl : NetworkBehaviour {
	public bool rotate, move;
	public Vector3 target;

	/*void CmdDestroy() {
		Destroy (gameObject);
	}
	[ClientRpc]
	void RpcDestroy() {
		Destroy (gameObject);
	}*/
	// Update is called once per frame
	void Update () {
		//if (isLocalPlayer) {
			
			if (rotate) {
				transform.RotateAround (target, Vector3.up, 10);
			}
			if (move) {
				transform.position = Vector3.Lerp (transform.position, target, 0.01f);
			}

		//}
	}
}