using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeWarriorSkillController : SkillController {
	[PunRPC]
	protected override void OnShowSkill (SkillType skillID, Vector3 skillPos)
	{
		if (skillID == SkillType.BianShen) {
			base.OnShowSkill (skillID, skillPos);
		} else {
			Skill skill = GetSkill ((int)skillID);
			GameObject skill_effect = null, insSkill = skill.effect;
			float life_time = skill.lifeTime;
			float x, z;
			LifeControl life_ctrl;

			switch (skillID) {
			case SkillType.Skill1:
				skill_effect = GameObject.Instantiate (insSkill, transform);
				break;
			case SkillType.Skill2:
				transform.LookAt (skillPos);

				x = skillPos.x - transform.position.x;
				z = skillPos.z - transform.position.z;
				skill_effect = GameObject.Instantiate (insSkill, 
					transform.position, 
					transform.rotation);
				skillPos.x += x * 5;
				skillPos.y = 3;
				skillPos.z += z * 5;
				life_ctrl = skill_effect.GetComponent<LifeControl> ();
				life_ctrl.target = skillPos;
				life_ctrl.move = true;
				life_ctrl.rotate = false;
				break;
			case SkillType.Skill3:
				skillPos = transform.position;
				skillPos.y = 0.7f;
				skill_effect = GameObject.Instantiate (insSkill, 
					skillPos, insSkill.transform.rotation);
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
