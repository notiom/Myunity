using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Crystal_Skill : Skill
{
	[SerializeField] private float crystalDuration;
	[SerializeField] private GameObject crystalPrefab;
	private GameObject currentCrystal;

	[Header("Crystal mirage")]
	[SerializeField] private UI_SkillTreeSlot unlockCloneInsteadCrystalButton;
	private bool cloneInsteadOfCrystal;

	[Header("Crystal simple")]
	[SerializeField] private UI_SkillTreeSlot unlockCrystalButton;
	public bool crystalUnlocked { get;private set; }

	[Header("Explosive crystal")]
	[SerializeField] private UI_SkillTreeSlot unlockExplosiveButton;
	 private bool canExplode;

	[Header("Moving crystal")]
	[SerializeField] private UI_SkillTreeSlot unlockMovingCrystalButton;
	private bool canMoveToEnemy;
	[SerializeField] private float moveSpeed;

	[Header("Multi stacking crystal")]
	[SerializeField] private UI_SkillTreeSlot unlockMultiStackButton;
	private bool canUseMultiStacks;
	[SerializeField] private int amountOfStacks;
	[SerializeField] private float multiStackCooldown;
	[SerializeField] private float useTimeWindow;
	[SerializeField] private List<GameObject> crystalLeft = new List<GameObject>();

	protected override void Start()
	{
		base.Start();
	}

	#region Unlock skill region
	public void UnlockCloneInsteadCrystal()
	{
		if(unlockCloneInsteadCrystalButton.unlocked) 
		{
			cloneInsteadOfCrystal = true;
		}
	}

	public void UnlockCrystal()
	{
		if(unlockCrystalButton.unlocked)
		{
			crystalUnlocked = true;
		}
	}

	public void UnlockExplosiveCrystal()
	{
		if(unlockExplosiveButton.unlocked) 
		{
			canExplode = true;
		}
	}

	public void UnlockMovingCrystal()
	{
		if(unlockMovingCrystalButton.unlocked) 
		{
			canMoveToEnemy = true;
		}
	}

	public void UnlockMultiStack()
	{
		if(unlockMultiStackButton.unlocked)
		{
			canUseMultiStacks = true;
		}
	}
	#endregion

	public override void UseSkill()
	{
		base.UseSkill();

		if(CanUseMultiCrystal()) 
		{
			return;
		}
		if(currentCrystal == null) 
		{
			CreateCrystal();
		}
		else
		{
			// 如果想使其移动，那么就不能在此和水晶换位置
			if (canMoveToEnemy)
				return;
			
			Vector2 playerPos = player.transform.position;

			player.transform.position = currentCrystal.transform.position;

			currentCrystal.transform.position = playerPos;

			if(cloneInsteadOfCrystal)
			{
				SkillManager.instance.clone.CreateClone(currentCrystal.transform, Vector3.zero);
				Destroy(currentCrystal);
			}
			else
			{
				currentCrystal.GetComponent<Crystal_Skill_Controller>()?.FinishCrystal();
			}
			
		}
	}

	private bool CanUseMultiCrystal()
	{
		if(canUseMultiStacks)
		{
			if (crystalLeft.Count > 0) 
			{
				if(crystalLeft.Count == amountOfStacks) 
				{
					Invoke("ResetAbility", useTimeWindow); //自动装填
				}
				cooldown = 0;
				GameObject crystalToSpawn = crystalLeft[crystalLeft.Count - 1];
				GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);

				crystalLeft.Remove(crystalToSpawn);
				newCrystal.GetComponent<Crystal_Skill_Controller>().
					SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(newCrystal.transform));

				if(crystalLeft.Count <= 0)
				{
					cooldown = multiStackCooldown;
					RefilCrystal();
				}
				return true;
			}

		}
		return false;
	}

	private void RefilCrystal()
	{
		int amountToAdd = amountOfStacks - crystalLeft.Count; // 添加量
		for(int i = 0;i < amountToAdd; i++) 
		{
			crystalLeft.Add(crystalPrefab);
		}
	}

	private void ResetAbility()
	{
		if(cooldown > 0) { return; }
		cooldownTimer = multiStackCooldown;
		RefilCrystal();
	}

	public void CreateCrystal()
	{
		currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
		Crystal_Skill_Controller currentCrystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();

		currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(currentCrystal.transform));
	}

	public void CurrentCrystalChooseRandomTarget() => currentCrystal.GetComponent<Crystal_Skill_Controller>().ChooseRandomEnemy();

	public void SetAttackToEnemy(Transform _transform)
	{
		currentCrystal.GetComponent<Crystal_Skill_Controller>().SetcCoseseTarget(_transform);
	}

	protected override void CheckUnlock()
	{
		UnlockCrystal();
		UnlockCloneInsteadCrystal();
		UnlockExplosiveCrystal();
		UnlockMovingCrystal();
		UnlockMultiStack();
	}

}
