using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class XiaoBinAttack : AutoAttack {
	public Collider weapon;
	NavMeshAgent navMeshAgent;
	Vector3 originDestination;
	float life_time;
	// Use this for initialization
	public override void Start ()
	{
		base.Start ();
		navMeshAgent = GetComponent<NavMeshAgent> ();
		originDestination = (gameObject.PropertyComponent () as SoldierProperty).originDestination;
		life_time = 0;
	}
		
	public override void OnTeamChanged (PunTeams.Team newTeam)
	{
		base.OnTeamChanged (newTeam);
		ChangeCloth ();
	}
		
	public override void AutoHit (GameObject enemy)
	{
		base.AutoHit (enemy);
		if (enemy) {
			Vector3 look = enemy.transform.position;
			look.y = transform.position.y;
			transform.LookAt (look);
			if (Vector3.Distance(transform.position, enemy.transform.position) <= gameObject.PropertyComponent().attackDistance) {
				SetTarget (transform.position);
				GetComponent<AnimatorControllerBase> ().PlayAnimation ("attack");
			} else {
				SetTarget (enemy.transform.position);
			}
		} else {
			SetTarget ((gameObject.PropertyComponent() as SoldierProperty).originDestination);
		}
	}

	public override bool CanAttack ()
	{
		return base.CanAttack () && navMeshAgent.isOnNavMesh;
	}

	public override void Update ()
	{
		life_time += Time.deltaTime;
		if (life_time > 50f || Vector3.Distance(transform.position, originDestination) < 2.0f) {
			if (isLocalPlayer) {
				PhotonNetwork.Destroy (gameObject);
			}
		}
		base.Update ();
	}
		
	public void SetTarget(Vector3 target) {
		target.y = transform.position.y;
		navMeshAgent.SetDestination (target);
	}

	void ChangeCloth() {
		SoldierProperty proper = (gameObject.PropertyComponent () as SoldierProperty);
		Material m = null;
		if (team != PunTeams.Team.none) {
			if (team == PhotonNetwork.player.GetTeam()) {
				m = proper.soldier_blue;
			} else {
				m = proper.soldier_red;
			}
			transform.GetChild (1).GetComponent<SkinnedMeshRenderer> ().material = m;
		}
	}
}