using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HeroAttack : AttackBase {
	float m_recovery = 0;
	public int recover_speed;

	public override void Attack (GameObject enemy)
	{
		base.Attack (enemy);
		recover_speed = 1;
		if (isLocalPlayer && enemy && enemy.GetComponent<AttackBase>().team != team) {
			Vector3 look = enemy.transform.position;
			look.y = transform.position.y;
			transform.LookAt (look);
		}
	}

	public override void BeAttacked (GameObject enemy, int power)
	{
		if ((gameObject.PropertyComponent() as HeroProperty).symbol == 3)
            power = -System.Math.Abs(power);
        else
            power = System.Math.Abs(power);
		base.BeAttacked (enemy, power);
		if (isLocalPlayer) {
			gameObject.HPComponent().TakeDamage (enemy.GetComponent<AttackBase>().team, power);
			m_recovery = Time.time;
		}
	}

	void Update ()
	{
		if (isLocalPlayer) {
			if (Time.time - m_recovery > 1.0f) {
				m_recovery = Time.time;
				gameObject.HPComponent().TakeDamage (PunTeams.Team.none, -1);
			}
		}
	}
}
