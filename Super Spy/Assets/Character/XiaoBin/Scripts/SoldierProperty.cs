using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierProperty : HeroPropertyBase {
	[HideInInspector]
	public Vector3 originDestination;

	public Material soldier_red, soldier_blue;

	public override void OnEnable ()
	{
		base.OnEnable ();
		OnEnableAnimator (true);
	}

	public override void OnDisable ()
	{
		base.OnDisable ();
		OnEnableAnimator (false);
		GetComponent<UnityEngine.AI.NavMeshAgent> ().enabled = false;
	}

	public override void OnEnaleAttack (bool isEnable)
	{
		base.OnEnaleAttack (isEnable);
		base.SetEnable<XiaoBinAttack> (isEnable);
	}

	public override void OnEnableAnimator (bool isEnable)
	{
		base.OnEnableAnimator (isEnable);
		base.SetEnable<AnimatorControllerBase> (isEnable);
	}
}
