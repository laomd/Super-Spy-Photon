using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prototype.NetworkLobby;

[System.Serializable]
public class Skill
{
	public GameObject effect;
	public float lifeTime;
	public float CD;
}

[System.Serializable]
public class MagicSkill : Skill
{
	public float magicPowerNeeded;
}

public class HeroPropertyBase : AIProperty
{
	public Collider weaponCollider;

	public virtual void OnEnableAnimator(bool isEnable) {
		GetComponent<Animator> ().enabled = isEnable;
	}

	public virtual void OnEnableSkill(bool isEnable) {
		
	}
}

public class HeroProperty : HeroPropertyBase {
	[Space]
	[Header("Born Properties")]
	public int speed = 6;
	public int money;
	public int symbol;
	public bool isSpy;
	[Header("Skill Properties")]
	public MagicSkill[] skills;

	public override void OnEnable ()
	{
		base.OnEnable ();
		OnEnableAnimator (true);
		OnEnableSkill (true);
		if (isLocalPlayer) {
			OnEnableExplore (true);
		}
	}
		
	public override void OnDisable ()
	{
		base.OnDisable ();
		OnEnableAnimator (false);
		OnEnableSkill (false);
		if (isLocalPlayer) {
			OnEnableExplore (false);
		}
	}
	public override void Start ()
	{
		if (isLocalPlayer) {
			PunTeams.Team myTeam = PhotonNetwork.player.GetTeam();
			if (myTeam == PunTeams.Team.red) {
				transform.localPosition = red_quanshui;
			} else if (myTeam == PunTeams.Team.blue) {
				transform.localPosition = blue_quanshui;
			}
			transform.LookAt ((blue_quanshui + red_quanshui) / 2);
			isSpy = PhotonNetwork.player.GetScore () != 0;
			GameSceneManager.instance.spyKind.SetActive (isSpy);
		}
	}

	public virtual void OnSpeedChanged(int newSpeed) {
		speed = newSpeed;
	}

	public override void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
	{
		base.OnPhotonSerializeView (stream, info);
		if (stream.isWriting) {
			stream.SendNext (speed);
			stream.SendNext (isSpy);
		} else {
			speed = (int)stream.ReceiveNext ();
			isSpy = (bool)stream.ReceiveNext ();
		}
	}

	public override void OnEnaleAttack (bool isEnable)
	{
		base.SetEnable<HeroHP> (isEnable);
		base.SetEnable<HeroAttack> (isEnable);
	}

	public override void OnEnableCheck (bool flag)
	{
		base.OnEnableCheck (flag);
		bodyCollider.enabled = flag;
	}

	public override void OnEnableAnimator (bool isEnable)
	{
		base.OnEnableAnimator (isEnable);
		base.SetEnable<NetworkAnimatorController> (isEnable);
	}


	public virtual void OnEnableExplore(bool isEnable) {
		FogOfWarExplorer explorer = base.SetEnable<FogOfWarExplorer> (isEnable);
		explorer.radius = 10;
		DayNightController.instance.onDayNightChanged.AddListener (delegate(bool arg0) {
			explorer.enabled = arg0;
		});
	}

}
