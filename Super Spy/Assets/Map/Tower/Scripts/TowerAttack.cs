using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

public class TowerAttack : AutoAttack {
	GameObject bullet;
	SpriteRenderer show;
	bool toShow;

	public override void Start ()
	{
		base.Start ();
		bullet = (gameObject.PropertyComponent() as TowerProperty).bullet;
		OnTeamChanged (PunTeams.Team.none);
		OnShow (false);
	}

	public override void OnTeamChanged (PunTeams.Team newTeam)
	{
		PunTeams.Team oldTeam = team;
		base.OnTeamChanged (newTeam);
		if (newTeam != PunTeams.Team.none) {
			if ((gameObject.PropertyComponent() as TowerProperty).isChoosed) {
				GameSceneManager.instance.OnTowerTeamChanged (name, oldTeam, newTeam);
			}
		}
	}

	public override bool CanBeAttacked (GameObject enemy)
	{
		return base.CanBeAttacked (enemy) && enemy.GetComponent<AnimatorControllerBase>().normalAttacking;
	}

	public override void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
	{
		base.OnPhotonSerializeView (stream, info);
		if (stream.isWriting) {
			stream.SendNext (toShow);
		} else {
			OnShow ((bool)stream.ReceiveNext ());
		}
	}

	public override void ChangeMaterial (PunTeams.Team newTeam)
	{
		base.ChangeMaterial (newTeam);
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
		string[] str = {"Banners", "Crystal"};
		foreach (var s in str) {
			transform.Find(s).GetComponent<MeshRenderer> ().material.color = newColor;
		}
	}
	public override void AutoHit(GameObject enemy) {
		base.AutoHit (enemy);
		OnShow (enemy != null);
		if (enemy) {
			photonView.RPC ("Hit", PhotonTargets.All, enemy.GetPhotonView().viewID);
		}
	}

	[PunRPC]
	void Hit(int viewId) {
		GameObject n = Instantiate (bullet, transform);
		GameObject lastHit = null;
		foreach (var item in PhotonNetwork.FindGameObjectsWithComponent(typeof(PhotonView))) {
			if (item.GetPhotonView().viewID == viewId) {
				lastHit = item;
				break;
			}
		}
		n.GetComponent<Bullet>().InitData(lastHit, 8, GetBall().color);
	}

	public void OnShow(bool value) {
		if (show == null) {
			show = transform.Find ("rang").GetComponent<SpriteRenderer> ();
		}
		show.enabled = toShow = value;
	}
}
