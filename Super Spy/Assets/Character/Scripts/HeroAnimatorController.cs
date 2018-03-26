using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkAnimatorController : AnimatorControllerBase {
	MeleeWeaponTrail weaponTrail;
	//CharacterController ctrller;
	public override void Awake ()
	{
		base.Awake ();
		weaponTrail = GetComponentInChildren<MeleeWeaponTrail> ();
		//ctrller = GetComponent<CharacterController> ();
	}

	/*public override void Start ()
	{
		base.Start ();
		foreach (var item in anim.runtimeAnimatorController.animationClips) {
			if (item.name.ToLower().Contains("walk") || item.name.ToLower().Contains("run")) {
				AddEvent (item, "OnWalkStart", 0);
				break;
			}
		}
	}

	void OnWalkStart() {
		ctrller.enabled = true;
	}*/

	protected virtual void Update() {

		if (PhotonNetwork.connected) {
			if (isLocalPlayer) {
				anim.SetBool ("run", ETCInput.GetAxis ("Vertical") != 0 ||
					ETCInput.GetAxis ("Horizontal") != 0);
			}
		} else {
			anim.SetBool ("run", false);
		}
	}

	protected override void OnAnimationStart() {
		base.OnAnimationStart ();
		weaponTrail.Emit = true;
		//ctrller.enabled = false;
	}

	protected override void OnAnimationEnd() {
		base.OnAnimationEnd ();
		weaponTrail.Emit = false;
		//ctrller.enabled = true;
	}
}