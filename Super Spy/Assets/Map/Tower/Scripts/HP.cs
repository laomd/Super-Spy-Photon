using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP : Photon.NetworkBehaviour {
	protected AttackBase attackBase {
		get {
			return GetComponent<AttackBase> ();
		}
	}

	public int curBlood {
		get {
			return property.blood;
		}
		set {
			property.blood = value;
		}
	}

	public int originBlood {
		get {
			return property.maxBlood;
		}
		set {
			property.maxBlood = value;
		}
	}

	GameObject bar = null;
	public override void Awake() {
		base.Awake ();
		bar = GameObject.Instantiate (property.HPBar, transform);
	}

	public virtual void Start() {
		if (isLocalPlayer) {
			OnHealthChanged (originBlood);
		}
	}

	protected virtual void Update() {
		if (bar) {
			bar.transform.LookAt (Camera.main.transform);
		}
	}

	public override void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
	{
		base.OnPhotonSerializeView (stream, info);
		if (stream.isWriting) {
			stream.SendNext (originBlood);
			stream.SendNext (curBlood);
		} else {
			originBlood = (int)stream.ReceiveNext ();
			OnHealthChanged ((int)stream.ReceiveNext ());
		}
	}

	public virtual void TakeDamage(PunTeams.Team enemyTeam, int hp) {
		if (enemyTeam != attackBase.team) {
			int newBlood = curBlood - hp;
			if (newBlood > 0) {
				if (newBlood >= originBlood) {
					newBlood = originBlood;
				}
			} else {
				newBlood = 0;
			}
			OnHealthChanged (newBlood);
			if (newBlood == 0) {
				OnBloodEmpty (enemyTeam);
			}
		}
	}

	public virtual void OnBloodEmpty(PunTeams.Team enemyTeam)
	{
		PhotonNetwork.Destroy (gameObject);
	}

	public virtual void OnHealthChanged(int newBlood) {
		curBlood = newBlood;
		UpdateBar ();
	}

	public void UpdateBar() {
		bar.GetComponentInChildren<Image> ().fillAmount = curBlood / (float)originBlood;
	}

	protected void SetBarColor(Color color) {
		bar.GetComponentInChildren<Image> ().color = color;
	}

	public virtual void UpdateColor(PunTeams.Team newTeam) {
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
		SetBarColor (newColor);
	}
}

public static class HPExtensions
{
	public static HP HPComponent(this GameObject obj) {
		HP hp = obj.GetComponent<HP> ();
		if (hp == null) {
			hp = obj.GetComponentInParent<HP> ();
		}
		return hp;
	}
}