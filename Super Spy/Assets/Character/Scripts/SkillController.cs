using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prototype.NetworkLobby;

public class SkillController : Photon.NetworkBehaviour {
	Skill[] skills;

	void Start() {
		skills = (gameObject.PropertyComponent() as HeroProperty).skills;
	}

	public void ShowEffect(SkillType state, Vector3 pos) {
		if (PhotonNetwork.connected) {
			photonView.RPC("OnShowSkill", PhotonTargets.All, state, pos);
		}
	}

	protected Skill GetSkill(int idx) {
		return skills [idx];
	}

	[PunRPC]
	protected virtual void OnShowSkill(SkillType skillID, Vector3 pos) {
		if (skillID == SkillType.BianShen) {
			Skill skill = GetSkill ((int)skillID);
			GameObject skill_effect = GameObject.Instantiate (skill.effect, transform);
			SetVisual (false);
			int lastSpeed = (gameObject.PropertyComponent () as HeroProperty).speed;
			AttackBase atkb = GetComponent<AttackBase> ();
			PunTeams.Team lastTeam = atkb.team;
			if (lastTeam == PunTeams.Team.red) {
				atkb.OnTeamChanged (PunTeams.Team.blue);
			} else {
				atkb.OnTeamChanged (PunTeams.Team.red);
			}
			SetSpeed (lastSpeed + 4);
			DayNightController.instance.onDayNightChanged.AddListener(delegate(bool arg0) {
				if (!arg0) {
					Destroy(skill_effect);
					SetVisual(true);
					SetSpeed(lastSpeed);
					atkb.OnTeamChanged(lastTeam);
				}
			});
		}
	}

	void SetSpeed(int newSpeed) {
		(gameObject.PropertyComponent () as HeroProperty).OnSpeedChanged (newSpeed);
	}

	protected void SetVisual(bool is_visual) {
		gameObject.PropertyComponent().OnEnableCheck (is_visual);

		if (!isLocalPlayer) {
			GetComponentInChildren<Canvas> ().enabled = is_visual;
			foreach (var render in GetComponentsInChildren<MeshRenderer>()) {
				render.enabled = is_visual;
			}
			foreach (var render in GetComponentsInChildren<SkinnedMeshRenderer>()) {
				render.enabled = is_visual;
			}
		}
	}
}
