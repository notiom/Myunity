using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dash_Skill : Skill
{
	[Header("Dash")]
	[SerializeField] private UI_SkillTreeSlot dashUnlockButton;
	public bool dashUnlocked { get; private set; }


	[Header("Clone on dash")]
	[SerializeField] private UI_SkillTreeSlot cloneOnDashUnlockButton;
	public bool createCloneOnDashStart { get; private set; }


	[Header("Cone on arrival")]
	[SerializeField] private UI_SkillTreeSlot cloneOnArrivalUnlockButton;
	public bool createCloneOnDashFinish { get; private set; }


	public override bool CanUseSkill()
	{
		if(!dashUnlocked)
		{
			return false;
		}
		return base.CanUseSkill();
	}
	public override void UseSkill()
	{
		base.UseSkill();
		// Debug.Log("Created clone behind");
	}

	public void UnlockDash()
	{
		if(dashUnlockButton.unlocked) 
		{
			dashUnlocked = true;
		}

	}

	public void UnlockCloneOnDash()
	{
		if(cloneOnDashUnlockButton.unlocked) 
		{
			createCloneOnDashStart = true;
		}
	}

	public void UnlockCloneOnArrival()
	{
		if(cloneOnArrivalUnlockButton.unlocked)
		{
			createCloneOnDashFinish = true;
		}
	}

	protected override void CheckUnlock()
	{
		UnlockDash();
		UnlockCloneOnDash();
		UnlockCloneOnArrival();
	}
}
