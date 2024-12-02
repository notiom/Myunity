using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill : Skill
{
	// 设置克隆体
	[SerializeField] private UI_SkillTreeSlot blackholeUnlockButton;
	public bool blackholeUnlocked { get; private set; }
	[SerializeField] private int amountOfAttacks;
	[SerializeField] private float cloneCoolddown;
	[Space]
	// 设置黑洞
	[SerializeField] private float blackholeDuration;
	[SerializeField] private GameObject blackHolePrefab;
	[SerializeField] private float maxSize;
	[SerializeField] private float growSpeed;
	[SerializeField] private float shrinkSpeed;

	Blackhole_Skill_Controller currentBlackhole;

	public void UnlockBlackHole()
	{
		if(blackholeUnlockButton.unlocked) 
		{
			blackholeUnlocked = true;
		}
	}
	public override bool CanUseSkill()
	{
		return base.CanUseSkill();
	}

	public override void UseSkill()
	{
		base.UseSkill();

		GameObject newBlackHole = Instantiate(blackHolePrefab, player.transform.position, Quaternion.identity);

		currentBlackhole = newBlackHole.GetComponent<Blackhole_Skill_Controller>();

		currentBlackhole.SetupBlackhole(maxSize, growSpeed, shrinkSpeed, amountOfAttacks, cloneCoolddown,blackholeDuration);
	}

	protected override void Start()
	{
		base.Start();
	}

	protected override void Update()
	{
		base.Update();
	}

	public bool BlackholeFinished()
	{
		if (!currentBlackhole) return false;

		if (currentBlackhole.playerCanExitState)
		{
			currentBlackhole = null;
			return true;

		}
		return false;
	}

	protected override void CheckUnlock()
	{
		UnlockBlackHole();
	}
}
