using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Clone_Skill : Skill
{
	[Header("Clone info")]
	private float attackMultiplier;
	[SerializeField] private float cloneDuration;
	[SerializeField] private GameObject clonePrefab;
	[Space]
	[Header("Clone attack")]
	[SerializeField] private UI_SkillTreeSlot cloneAttackUnlockButton;
	[SerializeField] private float cloneAttackMultiplier;
	[SerializeField] private bool canAttack;

	[Header("Aggrisive clone")]
	[SerializeField] private UI_SkillTreeSlot aggresiveCloneUnlockButton;
	[SerializeField] private float aggresivecloneAttackMultiplier;
	[SerializeField] public bool canApplyOnHitEffect { get; private set; }

	[Header("Clone can duplicate")]
	[SerializeField] private UI_SkillTreeSlot multipleUnlockButton;
	[SerializeField] private bool canDuplicateClone;
	[SerializeField] private float chanceToDuplicate;
	[SerializeField] private float multiConeAttackMultiplier;

	[Header("Crystal instead of clone")]
	[SerializeField] private UI_SkillTreeSlot crystalInsteadUnlockButton;
	public bool crystalInsteadOfClone;

	#region Unlock region
	public void UnlockCloneAttack()
	{
		if(cloneAttackUnlockButton.unlocked)
		{
			canAttack = true;
			attackMultiplier = cloneAttackMultiplier;
		}
	}

	public void UnlockAggresiveClone()
	{
		if(aggresiveCloneUnlockButton.unlocked)
		{
			canApplyOnHitEffect = true;
			attackMultiplier = aggresivecloneAttackMultiplier;
		}
	}

	public void UnlockMultiClone()
	{
		if(multipleUnlockButton.unlocked)
		{
			canDuplicateClone = true;
			attackMultiplier = multiConeAttackMultiplier;
		}
	}

	public void UnlockCrystalInsteadClone()
	{
		if(crystalInsteadUnlockButton.unlocked)
		{
			crystalInsteadOfClone = true;
		}
	}
	#endregion
	public void CreateClone(Transform _clonePosition, Vector3 _offset)
	{
		if(crystalInsteadOfClone)
		{
			player.skill.crystal.CreateCrystal();
			// player.skill.crystal.CurrentCrystalChooseRandomTarget();
			return;
		}

		GameObject newClone = Instantiate(clonePrefab);
		newClone.GetComponent<Clone_Skill_Controller>().SetupClone(_clonePosition, cloneDuration, canAttack, _offset + new Vector3(0,0.15f), FindClosestEnemy(newClone.transform),canDuplicateClone,chanceToDuplicate,attackMultiplier);
	}

	public void CreateCloneonDashBegun(bool createCloneOnDashStart)
	{
		if (createCloneOnDashStart)
		{
			CreateClone(player.transform, Vector3.zero);
		}
	}

	public void CreateCloneOnDashFinish(bool createCloneOnDashOver)
	{
		if(createCloneOnDashOver) 
		{
			CreateClone(player.transform, Vector3.zero);
		}
	}

	public void CreateCloneOnCounterAttack(Transform _transform)
	{
		StartCoroutine(CreateCloneWithDelay(_transform, new Vector3(0.7f * (int)player.facingDir, 0)));
	}

	private IEnumerator CreateCloneWithDelay(Transform _transform,Vector3 _offset)
	{
		yield return new WaitForSeconds(.4f);
		CreateClone(_transform, _offset);
	}

	protected override void CheckUnlock()
	{
		UnlockCloneAttack();
		UnlockAggresiveClone();
		UnlockMultiClone();
		UnlockCrystalInsteadClone();
	}
}