using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parry_Skill : Skill
{
	[Header("Parry")]
	[SerializeField] private UI_SkillTreeSlot parryUnlockButton;
	public bool parryUnlocked { get; private set; }

	[Header("Parry restore")]
	[SerializeField] private UI_SkillTreeSlot restoreUnlockButton;
	[Range(0f, 1f)]
	[SerializeField] private float restoreHealthPercentage;
	public bool restoreUnlocked { get; private set; }

	[Header("Parry with mirage")]
	[SerializeField] private UI_SkillTreeSlot parryWithMirageUnlockButton;
	public bool parryWithMirageUnlocked { get; private set; }

	public override void UseSkill()
	{
		base.UseSkill();

		if(restoreUnlocked) 
		{
			int restoreAmount  = Mathf.RoundToInt(player.stats.GetMaxHealthValue() * restoreHealthPercentage);
			player.stats.IncreaseHealthBy(restoreAmount);
		}
	}

	public void UnlockParry()
	{
		if(parryUnlockButton.unlocked) 
		{
			parryUnlocked = true;
		}
	}

	public void UnlockParryRestore()
	{
		if (restoreUnlockButton.unlocked)
		{
			restoreUnlocked = true;
		}
	}

	public void UnlockParryWithMirage()
	{
		if (parryWithMirageUnlockButton.unlocked)
		{
			parryWithMirageUnlocked = true;
		}
	}

	protected override void CheckUnlock()
	{
		UnlockParry();
		UnlockParryRestore();
		UnlockParryWithMirage();
	}
}
