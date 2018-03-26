using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAttack : AttackBase {
	float _time;
	// Use this for initialization
	public override void Start () {
		base.Start ();
		_time = 0;
	}

	public virtual bool CanAttack() {
		return _time >= gameObject.PropertyComponent().attackCd;
	}

	public virtual void AutoHit(GameObject enemy) {
		if (enemy) {
			_time = 0;
		}
	}
	
	// Update is called once per frame
	public virtual void Update () {
		if (isLocalPlayer) {
			if (CanAttack()) {
				AutoHit (Check.FindObjectAroundthePoint (transform.position, (gameObject.PropertyComponent() as AIProperty).checkDistance, team));
			} else {
				_time += Time.deltaTime;
			}
		}
	}

	public override void BeAttacked (GameObject enemy, int power)
	{
		base.BeAttacked (enemy, power);
		if (isLocalPlayer) {
			gameObject.HPComponent().TakeDamage (enemy.GetComponent<AttackBase>().team, power);
		}
	}
}
