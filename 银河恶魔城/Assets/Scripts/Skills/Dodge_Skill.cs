using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodge_Skill : Skill
{
    [Header("Dodge")]
    [SerializeField] private UI_SkillTreeSlot unlockDodgeButton;
    [SerializeField] private int evasionAmount;
    public bool dodgeUnlocked;

    [Header("Mirage dodge")]
    [SerializeField] private UI_SkillTreeSlot unlockMirageDodgeButton;
    public bool mirageDodgeUnlocked;

	protected override void Start()
	{
		base.Start();

	}

    public void UnlockDodge()
    {
        if(unlockDodgeButton.unlocked) 
        {
            // 增加10点闪避值
            player.stats.evasion.AddModifier(evasionAmount);
            Inventory.instance.UpdateSlotUI();
            dodgeUnlocked = true; 
        }
    }

	protected override void CheckUnlock()
	{
        UnlockDodge();
        UnlockMirageDodge();
	}

	public void UnlockMirageDodge()
    {
        if(unlockMirageDodgeButton.unlocked)
        {
            mirageDodgeUnlocked= true;
        }
    }

    public void CreateMirageOnDodge()
    {
        if(mirageDodgeUnlocked) 
        {
            SkillManager.instance.clone.CreateClone(player.transform, (int)player.facingDir * new Vector3(1,0)); // 出现在怪物身后
        }
    }
}
