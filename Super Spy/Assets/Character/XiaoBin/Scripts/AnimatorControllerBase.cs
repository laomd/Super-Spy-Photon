using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorControllerBase : Photon.NetworkBehaviour
{
	protected Animator anim;
	Collider weapon;
	bool isNormal;

	public virtual bool normalAttacking {
		get {
			return isNormal;
		}
	}

	public override void Awake ()
	{
		base.Awake ();
		anim = GetComponent<Animator> ();
		weapon = (gameObject.PropertyComponent() as HeroPropertyBase).weaponCollider;
	}

	public virtual void Start() {
		foreach (var item in anim.runtimeAnimatorController.animationClips) {
			if (!Filter(item.name)) {
				AddEvent (item, "OnAnimationStart", 0);
				AddEvent (item, "OnAnimationEnd", item.length);
				if (isNormalAtkClip(item)) {
					AddEvent (item, "OnNormalAttackStart", 0);
					AddEvent (item, "OnNormalAttackEnd", item.length);
				}
			}
		}
		OnNormalAttackEnd ();
		OnAnimationEnd ();
	}

	bool isNormalAtkClip(AnimationClip clip) {
		return clip.name.ToLower ().EndsWith ("attack");
	}

	bool Filter(string clipName) {
		string[] disable = { "idle", "run", "walk" };
		foreach (var item in disable) {
			if (clipName.ToLower().Contains(item)) {
				return true;
			}
		}
		return false;
	}

	protected virtual void OnAnimationStart() {
		weapon.enabled = true;
	}

	protected virtual void OnAnimationEnd() {
		weapon.enabled = false;
	}

	protected virtual void OnNormalAttackStart() {
		isNormal = true;
	}

	protected virtual void OnNormalAttackEnd() {
		isNormal = false;
	}

	public void PlayAnimation(string state) {
		if (PhotonNetwork.connected) {
			isNormal = state == "attack";
			anim.SetTrigger (state);
		}
	}

	AnimationClip GetClip(string clipName) {
		foreach (var item in anim.runtimeAnimatorController.animationClips) {
			if (item.name == clipName) {
				return item;
			}
		}
		return null;
	}

	AnimationEvent MakeEvent(string functionName, float time) {
		AnimationEvent evt = new AnimationEvent ();
		evt.functionName = functionName;
		evt.time = time;
		return evt;
	}

	protected void AddEvent(AnimationClip clip, string functionName, float time) {
		AnimationEvent evt = MakeEvent (functionName, time);
		clip.AddEvent (evt);
	}
}