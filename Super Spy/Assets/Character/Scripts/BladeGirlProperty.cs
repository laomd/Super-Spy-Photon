using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeGirlProperty : HeroProperty {
	public override void OnEnableSkill (bool isEnable)
	{
		base.OnEnableSkill (isEnable);
		base.SetEnable<BladeGirlSkillController> (isEnable);
	}
}
