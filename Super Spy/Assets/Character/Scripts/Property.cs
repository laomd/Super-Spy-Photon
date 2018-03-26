using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Property : Photon.NetworkBehaviour {
	[HideInInspector]
	public int blood;
	[HideInInspector]
	public bool isVisual;
	[Space]
	[Header("Attack Properties")]
	public Vector3 red_quanshui;
	public Vector3 blue_quanshui;
	public Collider bodyCollider;
	public GameObject characterBall;
	public int maxBlood;
	public GameObject HPBar;
	public int attackDistance;
	public float attackCd;
	public int attackPower;
	[HideInInspector]
	public PunTeams.Team myTeam;

	public override void Awake() {
		base.Awake ();
		bodyCollider.gameObject.layer = LayerMask.NameToLayer ("Checkable");
		characterBall.layer = LayerMask.NameToLayer ("CharacterBall");
	}

	public virtual void OnEnable() {
		OnEnaleAttack (true);
		OnEnableCheck (true);
	}

	public virtual void Start() {
		
	}

	public virtual void OnDisable()
	{
		OnEnaleAttack (false);
		OnEnableCheck (false);
	}
		
	protected T SetEnable<T>(bool isEnable) where T : MonoBehaviour {
		T comp = GetComponent<T> ();
		if (comp == null) {
			comp = gameObject.AddComponent<T> ();
		}
		comp.enabled = isEnable;
		return comp;
	}

	public virtual void OnEnaleAttack(bool flag) {
		SetEnable<HP> (flag);
	}

	public virtual void OnEnableCheck(bool flag) 
	{
		isVisual = flag;
	}
}

public static class PropertyExtensions
{
	public static Property PropertyComponent(this GameObject m) {
		Property prop = m.GetComponent<Property> ();
		if (prop == null) {
			prop = m.GetComponentInParent<Property> ();
		}
		return prop;
	}
}

public class AIProperty : Property {
	public int checkDistance;
	/*public override void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
	{
		base.OnPhotonSerializeView (stream, info);
		if (stream.isWriting) {
			stream.SendNext (checkDistance);
		} else {
			checkDistance = (int)stream.ReceiveNext ();
		}
	}*/
}