using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SkillType
{
	BianShen,
	Skill1,
	Skill2,
	Skill3,
	Skill4,
	Attack
}

[RequireComponent(typeof(SkillJoystick))]
public class Effect : SkillArea {
	protected static string[] skillName = { "bianshen", "skill1", "skill2", "skill3", "skill4", "attack" };

	public SkillType skill;
	HeroProperty init {
		get {
			if (GameSceneManager.localHero) {
				return GameSceneManager.localHero.GetComponent<HeroProperty> ();
			} else {
				return null;
			}
		}
	}
	float cooling_time {
		get {
			if (init) {
				if (skill == SkillType.Attack) {
					return init.attackCd;
				} else {
					return init.skills [(int)skill].CD;
				}
			} else {
				return 0;
			}

		}
	}
	protected Image mask;
	protected Text time_text;
	protected float cur_time;
	// Use this for initialization
	public override void Start () {
		base.Start ();
		cur_time = 0;
		var m_mask = transform.parent.Find (
			transform.gameObject.name.Replace ("Skill", "mask"));
		if (m_mask) {
			mask = m_mask.GetComponent<Image>();
			mask.gameObject.SetActive (false);
			time_text = m_mask.GetComponentInChildren<Text>();
			time_text.text = "";
		}
	}

	protected override void OnJoystickUpEvent ()
	{
		base.OnJoystickUpEvent ();
		PlayEffect ();
	}
	
	// Update is called once per frame
	public virtual void Update () {
		if (mask != null) {
			if (mask.fillAmount == 0) {
				time_text.text = "";
				mask.gameObject.SetActive (false);
			} else {
				cur_time += Time.deltaTime;
				float remaining_time = cooling_time - cur_time;
				string t;
				if (remaining_time < 10f) {
					t = string.Format ("{0:F}", remaining_time);
				} else {
					t = ((int)remaining_time).ToString();
				}
				time_text.text = t;
				mask.fillAmount = remaining_time / cooling_time;
			}
		}
	}

	protected virtual void PlayEffect() {
		if (mask) {
			mask.gameObject.SetActive (true);
			mask.fillAmount = 1;
			cur_time = 0;
		}
	}
}