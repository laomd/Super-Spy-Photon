using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBase : Photon.NetworkBehaviour {
	public PunTeams.Team team {
		get {
			return gameObject.PropertyComponent().myTeam;
		}
		set {
			gameObject.PropertyComponent ().myTeam = value;
		}
	}

	public virtual void Start () {
		if (isLocalPlayer) {
			OnTeamChanged(team);
		}
	}

	public virtual bool CanBeAttacked(GameObject enemy) {
		return enemy && enemy != gameObject && gameObject.PropertyComponent().isVisual;
	}

	public virtual void OnTriggerEnter(Collider enemy) {
		if (enemy.GetType() != typeof(CharacterController)) {
			GameObject root = Check.GetRootParent (enemy.transform);
			if (CanBeAttacked(root)) {
				root.GetComponent<AttackBase> ().Attack (gameObject);
			}
		}
	}

	public override void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
	{
		base.OnPhotonSerializeView (stream, info);
		if (stream.isWriting) {
			stream.SendNext (team);
		} else {
			OnTeamChanged ((PunTeams.Team)stream.ReceiveNext());
		}
	}

	public virtual void OnTeamChanged(PunTeams.Team newTeam) {
		team = newTeam;
		gameObject.HPComponent().UpdateColor (newTeam);
		ChangeMaterial (newTeam);
	}

	public virtual void ChangeMaterial(PunTeams.Team newTeam) {
		Color newColor;
		if (newTeam == PunTeams.Team.none) {
			newColor = PunTeams.TeamColor [(int)newTeam];
		} else {
			if (newTeam == PhotonNetwork.player.GetTeam()) {
				newColor = Color.blue;
			} else {
				newColor = Color.red;
			}
		}
		GetBall().color = newColor;
	}

	protected Material GetBall() {
		return gameObject.PropertyComponent().characterBall.GetComponent<MeshRenderer> ().material;
	}
		
	public virtual void Attack(GameObject enemy) {
		if (enemy != null) {
			AttackBase attack_base = enemy.GetComponent<AttackBase> ();
			if (!attack_base) {
				attack_base = enemy.GetComponentInParent<AttackBase> ();
			}
			attack_base.BeAttacked (gameObject, gameObject.PropertyComponent().attackPower);
		}
	}

	public virtual void BeAttacked(GameObject enemy, int power) {
		
	}
}
