using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeGirlSkillController : SkillController {
	[PunRPC]
	protected override void OnShowSkill (SkillType skillID, Vector3 skillPos)
	{
		if (skillID == SkillType.BianShen) {
			base.OnShowSkill (skillID, skillPos);
		} else {
			Skill skill = GetSkill ((int)skillID);
			GameObject skill_effect = null, insSkill = skill.effect;
			float life_time = skill.lifeTime;
			LifeControl life_ctrl;

			switch (skillID) {
			case SkillType.Skill1:
				skill_effect = GameObject.Instantiate (insSkill, transform.position, Quaternion.identity);
				break;
			case SkillType.Skill2:
				transform.LookAt (skillPos);
				skill_effect = GameObject.Instantiate (insSkill, 
					transform.position, 
					transform.rotation);
				skillPos = skillPos + (skillPos - transform.position) * 5;
				skillPos.y = 3;
				life_ctrl = skill_effect.GetComponent<LifeControl> ();
				life_ctrl.target = skillPos;
				life_ctrl.move = true;
				life_ctrl.rotate = false;
				break;
			case SkillType.Skill3:
				transform.LookAt (skillPos);
				skillPos = skillPos + (skillPos - transform.position) * 5;
				skillPos.y = 0.7f;
				skill_effect = GameObject.Instantiate (insSkill, 
					skillPos, insSkill.transform.rotation);
				life_ctrl = skill_effect.GetComponent<LifeControl> ();
				life_ctrl.target = transform.position;
				life_ctrl.move = false;
				life_ctrl.rotate = true;
				break;
			case SkillType.Skill4:
				transform.LookAt (skillPos);
				skillPos.y = insSkill.transform.position.y;
				skill_effect = GameObject.Instantiate (insSkill, skillPos, insSkill.transform.rotation);
				break;
			default:
				break;
			}
			if (skill_effect) {
				Destroy (skill_effect, life_time);
			}
		}
	}
}
