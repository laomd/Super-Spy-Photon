using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateSoldiers : Photon.NetworkBehaviour {
	GameObject soldier;
	GameObject magician;
	Vector3 red_quanshui, blue_quanshui;

	float last_time, cur_time;
	static int total = 0;

	// Use this for initialization
	void Start() {
		cur_time = last_time = 0;
		TowerProperty init = gameObject.PropertyComponent() as TowerProperty;
		soldier = init.soldier;
		magician = init.magician;
		red_quanshui = init.red_quanshui;
		blue_quanshui = init.blue_quanshui;
		if (isLocalPlayer) {
			Generate ();
		}
	}

	// Update is called once per frame
	void Update () {
		cur_time = Time.time;
		if (cur_time - last_time >= 60.0f) {
			last_time = cur_time;
			if (isLocalPlayer) {
				Generate ();
			}
		}
	}

	/*public override void OnSend (NanoBuffers.NanoWriter writer)
	{
		base.OnSend (writer);
		writer.put (targetPosition);
	}

	public override void OnReceived (NanoBuffers.NanoReader reader)
	{
		base.OnReceived (reader);
		reader.get (out targetPosition);
		Generate ();
	}*/

	void Generate() {
		PunTeams.Team myTeam = gameObject.PropertyComponent().myTeam;
		Vector3 targetPosition = GetEnermyQuanShui (myTeam);
		Vector3 pos = transform.position;
		pos.y = 3;
		GameObject ins = null;
		int rd = Random.Range (0, 2);
		if (rd < 1) {
			ins = magician;
		} else {
			ins = soldier;
		}
		total++;
		GameObject obj = PhotonNetwork.InstantiateSceneObject (ins.name, pos, Quaternion.identity, 0, null);
		Property p = obj.PropertyComponent ();
		p.myTeam = myTeam;
		(p as SoldierProperty).originDestination = targetPosition;
	}

	Vector3 GetEnermyQuanShui(PunTeams.Team myTeam) {
		if (myTeam == PunTeams.Team.red) {
			return blue_quanshui;
		} else if (myTeam == PunTeams.Team.blue) {
			return red_quanshui;
		} else {
			int i = Random.Range (0, 2);
			if (i == 0) {
				return red_quanshui;
			} else {
				return blue_quanshui;
			}
		}
	}
}
