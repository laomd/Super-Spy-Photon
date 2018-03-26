
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prototype.NetworkLobby;

public class TowerHP : HP {
	PunTeams.Team bloodTeam;

	protected override void Update ()
	{
		base.Update ();
		UpdateColor (bloodTeam);
	}

	public override void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
	{
		base.OnPhotonSerializeView (stream, info);
		if (stream.isWriting) {
			stream.SendNext (bloodTeam);
		} else {
			bloodTeam = (PunTeams.Team)stream.ReceiveNext ();
		}
	}

	public override void TakeDamage (PunTeams.Team enemyTeam, int power)
	{
		int newBlood;
		if (bloodTeam != enemyTeam) {
			newBlood = curBlood - power;
			if (newBlood <= 0) {
				newBlood *= -1;
				bloodTeam = (enemyTeam);
			}
		} else {
			newBlood = curBlood + power;
			if (newBlood >= originBlood) {
				newBlood = originBlood;
				attackBase.OnTeamChanged (bloodTeam);
			}
		}
		if (newBlood != curBlood) {
			OnHealthChanged (newBlood);
		}
	}
}
