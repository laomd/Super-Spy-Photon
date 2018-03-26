using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TowerProperty : AIProperty {
	bool m_isChoosed;
	public bool isChoosed {
		get {
			return m_isChoosed;
		}
	}
	public GameObject bullet;
	public GameObject soldier;
	public GameObject magician;
	public override void Awake ()
	{
		base.Awake ();
		Choosed (false);
	}

	public override void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
	{
		base.OnPhotonSerializeView (stream, info);
		if (stream.isWriting) {
			stream.SendNext (isChoosed);
		} else {
			Choosed((bool)stream.ReceiveNext());
		}
	}
		
	public override void OnEnaleAttack (bool isEnable)
	{
		base.SetEnable<TowerHP> (isEnable);
		base.SetEnable<TowerAttack> (isEnable);
		base.SetEnable<GenerateSoldiers> (isEnable);
	}

	void Choosed(bool flag) {
		m_isChoosed = flag;
		GetComponentInChildren<Rotate> ().enabled = !flag;
	}

	public void OnChoosed(bool flag) {
		Choosed (flag);
	}
}
