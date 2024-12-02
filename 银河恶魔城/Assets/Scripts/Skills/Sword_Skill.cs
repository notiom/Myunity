using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwordType
{
	Regular,
	Bounce,
	Pierce,
	Spin
}

public class Sword_Skill : Skill
{
	public SwordType swordType = SwordType.Regular;
	[Header("Bounce Info")]
	[SerializeField] private UI_SkillTreeSlot bounceUnlockButton;

	[SerializeField] private int bounceAmount;
	[SerializeField] private float bounceGravity;
	[SerializeField] private float bounceSpeed;
	[Header("Pierce info")]
	[SerializeField] private UI_SkillTreeSlot pierceUnlockButton;
	[SerializeField] private int pierceAmount;
	[SerializeField] private float pierceGravity;
	[Header("Spin info")]
	[SerializeField] private UI_SkillTreeSlot spinUnlockButton;
	[SerializeField] private float hitCooldown = .35f;
	[SerializeField] private float maxTravelDistance = 5;
	[SerializeField] private float spinDuration = 2;
	[SerializeField] private float spinGravity = 1;

	[Header("skill info")]
	[SerializeField] private UI_SkillTreeSlot swordUnlockButton;
	public bool swordUnlocked { get; private set; }
	[SerializeField] private GameObject swordPrefab;
	[SerializeField] private Vector2 launchDir;
	[SerializeField] private float swordGravity;
	[SerializeField] private float freezeTimeDuration = .7f;
	[SerializeField] private float returnSpeed;
	public float accumlateTimer;
	private Vector2 currentDirection; //用于使用wasd控制瞄准方向
	public bool canEnterO = true; // 在回收剑的过程中不可以按下O

	[Header("Passive skills")]
	[SerializeField] private UI_SkillTreeSlot timeStopUnlockButton;
	public bool timestopUnlocked {get ; private set;}
	[SerializeField] private UI_SkillTreeSlot volnurableUnlockButton;
	public bool volnurableUnlocked { get; private set; }


	private Vector2 finalDir;
	[Header("Aim dots")]
	[SerializeField] private int numberOfDots;
	[SerializeField] private float spaceBetweenDots;
	[SerializeField] private GameObject dotPrefab;
	[SerializeField] private Transform dotsParent;
	[SerializeField] private Transform upWallCheck;

	private GameObject[] dots; 
	public void createSword()
	{
		GameObject newSword = Instantiate(swordPrefab,player.transform.position,transform.rotation);

		Sword_Skill_Controller newSwordScript = newSword.GetComponent<Sword_Skill_Controller>();

		if(swordType == SwordType.Bounce)
		{
			newSwordScript.SetupBounce(true, bounceAmount,bounceSpeed);
		}
		else if(swordType == SwordType.Pierce)
		{
			newSwordScript.SetupPierce(pierceAmount);
		}
		else if(swordType == SwordType.Spin)
		{
			newSwordScript.SetupSpin(true, maxTravelDistance, spinDuration, hitCooldown);
		}
		newSwordScript.SetupSword(finalDir, swordGravity,freezeTimeDuration,returnSpeed);
		player.AssignNewSword(newSword);
		DotsActive(false);
	}

	protected override void Start()
	{
		base.Start();
		GenereateDots();
		currentDirection = player.transform.position;
		SetUpGraivty();
	}

	private void SetUpGraivty()
	{
		if (swordType == SwordType.Bounce)
			swordGravity = bounceGravity;
		else if (swordType == SwordType.Pierce)
			swordGravity = pierceGravity;
		else if (swordType == SwordType.Spin)
			swordGravity = spinGravity;
	}

	protected override void Update()
	{
		// 不能使用技能就直接return
		base.Update();
		accumlateTimer += Time.deltaTime;
		// 增加蓄力时长对敌人的伤害和距离
		launchDir = accumlateTimer > 1 ? new Vector2(15,20) : new Vector2(accumlateTimer * 15, accumlateTimer * 20);
		if (player.sword)
		{
			return;
		}

		if (Input.GetKeyUp(KeyCode.O)) 
		{
			finalDir = new Vector2(AimDirection().normalized.x * launchDir.x, AimDirection().normalized.y * launchDir.y);
			return;
		}

		if (Input.GetKey(KeyCode.O) && swordUnlocked)
		{	
			for (int i = 0; i < dots.Length; i++)
			{
				dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
				if (dots[i].transform.position.y < upWallCheck.position.y)
				{
					dots[i].SetActive(false);
				}
				else 
				{
					dots[i].SetActive(true);
				}
			}
			
		}
	}

	#region unlock region
	public void UnlockTimeStop()
	{
		if(timeStopUnlockButton.unlocked) 
		{
			timestopUnlocked = true;
		}
	}

	public void UnlockVulnurable()
	{
		if(volnurableUnlockButton.unlocked)
		{
			volnurableUnlocked = true;
		}
	}

	public void UnlockSword()
	{
		if(swordUnlockButton.unlocked)
		{
			swordType = SwordType.Regular;
			swordUnlocked = true;
		}
	}

	public void UnlockBounceSword()
	{
		if(bounceUnlockButton.unlocked)
		{
			swordType = SwordType.Bounce;
		}
	}

	public void UnlockPierceSword()
	{
		if (pierceUnlockButton.unlocked)
		{
			swordType = SwordType.Pierce;
		}
	}

	public void UnlockSpinSword()
	{
		if (spinUnlockButton.unlocked)
		{
			swordType = SwordType.Spin;
		}
	}
	#endregion

	#region Aim region
	/*
	public Vector2 AimDirection()
	{
		Vector2 playerPosition = player.transform.position;
		Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 direction = mousePosition - playerPosition;

		return direction;
	}
	*/

	public Vector2 AimDirection()
	{
		// 根据按键改变方向，按键按下时更新方向
		if (Input.GetKey(KeyCode.W))
		{
			currentDirection += Vector2.up; // 向上
		}
		else if (Input.GetKey(KeyCode.S))
		{
			currentDirection += Vector2.down; // 向下
		}
		else if (Input.GetKey(KeyCode.A))
		{
			if(player.facingDir == FacingDir.Right)
				player.Flip();
			currentDirection += Vector2.left; // 向左
		}
		else if (Input.GetKey(KeyCode.D))
		{
			if (player.facingDir == FacingDir.Left)
				player.Flip();
			currentDirection += Vector2.right; // 向右
		}

		return currentDirection;
	}


	public void DotsActive(bool _isActive)
	{
		for(int i = 0;i < dots.Length;i++)
		{
			dots[i].SetActive(_isActive);
		}
	}
	private void GenereateDots()
	{
		dots = new GameObject[numberOfDots];
		for(int i = 0;i < numberOfDots;i++) 
		{
			dots[i] = Instantiate(dotPrefab, player.transform.position,Quaternion.identity,dotsParent);
			dots[i].SetActive(false);
		}
	}

	private Vector2 DotsPosition(float t)
	{
		// 该块的抛物线就应该为初始位移 + 抛物线位移
		Vector2 position = (Vector2)player.transform.position + new Vector2(
			AimDirection().normalized.x * launchDir.x, 
			// v0 * t + 1 / 2 * g * t * t 
			AimDirection().normalized.y * launchDir.y) * t + .5f * (Physics2D.gravity * swordGravity) * (t * t);
		return position;
	}

	public void ClearAllDots()
	{
		foreach (var dot in dots) dot.SetActive(false);
	}
	// 设计开发蓄力时间越久,伤害越高，飞的越远
	#endregion

	protected override void CheckUnlock()
	{
		UnlockSword();
		UnlockBounceSword();
		UnlockSpinSword();
		UnlockPierceSword();
		UnlockTimeStop();
		UnlockVulnurable();
	}

}
