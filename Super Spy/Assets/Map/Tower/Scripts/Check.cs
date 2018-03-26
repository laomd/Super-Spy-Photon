using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Check : MonoBehaviour {
	public static GameObject GetRootParent(Transform t) {
		AttackBase root = GetRootAttackBase (t);
		if (root) {
			return root.gameObject;
		} else {
			return null;
		}
	}
	static AttackBase GetRootAttackBase(Transform t) {
		AttackBase root = t.GetComponent<AttackBase> ();
		if (root == null) {
			root = t.GetComponentInParent<AttackBase> ();
		}
		return root;
	}
	public static GameObject FindObjectAroundthePoint(Vector3 point, float radius, PunTeams.Team myTeam){ 
		Collider[] res = Physics.OverlapSphere (point, radius);
		foreach (var item in res) {
			if (item.gameObject.layer == LayerMask.NameToLayer("Checkable")) {
				AttackBase root = GetRootAttackBase (item.transform);
				if (root && root.team != myTeam) {
					return root.gameObject;
				}
			}
		}
		return null; 
	}
}
