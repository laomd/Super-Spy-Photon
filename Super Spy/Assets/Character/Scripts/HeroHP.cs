using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prototype.NetworkLobby;

public class HeroHP : HP {
	public override void UpdateColor (PunTeams.Team newTeam)
	{
		if (isLocalPlayer) {
			SetBarColor (Color.green);
		} else {
			base.UpdateColor (newTeam);
		}
	}

	public override void OnBloodEmpty (PunTeams.Team enemyTeam)
	{
		gameObject.PropertyComponent ().OnRespawn ();
		base.Start ();
	}
}
