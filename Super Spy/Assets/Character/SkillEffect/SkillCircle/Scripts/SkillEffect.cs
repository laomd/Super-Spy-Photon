using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SkillEffect : Effect {
	public void SetSkillImage(string playerName) {
		if (skill != SkillType.Attack) {
			GetComponent<Image>().sprite = Resources.Load<Sprite>(playerName + skillName[(int)skill]);
		}
	}

	protected override void PlayEffect()
	{
		base.PlayEffect ();
		if (player) {
			AnimatorControllerBase ani_ctrl = player.GetComponent<AnimatorControllerBase> ();
			SkillController skill_ctrl = player.GetComponent<SkillController> ();

			Vector3 pos = Vector3.zero;
			switch (areaType) {
			case SkillAreaType.OuterCircle_InnerCircle:
				pos = GetCirclePosition (outerRadius);
				break;
			case SkillAreaType.OuterCircle_InnerCube:
			case SkillAreaType.OuterCircle_InnerSector:
				pos = GetCubeSectorLookAt ();
				break;
			default:
				break;
			}
			ani_ctrl.PlayAnimation (skillName[(int)skill]);
			if (skill != SkillType.Attack) {
				skill_ctrl.ShowEffect (skill, pos);
			}
		}
	}

}