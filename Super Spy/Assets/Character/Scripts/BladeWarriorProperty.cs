using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeWarriorProperty : HeroProperty {
	public override void OnEnableSkill (bool isEnable)
	{
		base.OnEnableSkill (isEnable);
		base.SetEnable<BladeWarriorSkillController> (isEnable);
	}
}
