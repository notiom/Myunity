using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
	public PlayerGroundedState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();
		rb.velocity = new Vector2(0, 0);
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void Update()
	{
		base.Update();
		if(Input.GetKeyDown(KeyCode.J)) 
		{
			stateMachine.ChangeState(player.primaryAttack);
			return;
		}
		if(Input.GetKeyDown(KeyCode.K) && player.IsGroundDetected())
		{
			jumpCount++;
			stateMachine.ChangeState(player.jumpState);
			return;
		}
		if(Input.GetKeyDown(KeyCode.I) && player.skill.parry.parryUnlocked)
		{
			stateMachine.ChangeState(player.counterAttack);
		}

		if(Input.GetKeyDown(KeyCode.O) && HashNoSword() && player.skill.sword.canEnterO && player.skill.sword.swordUnlocked)
		{
			// 这里的处理可以直接省略销毁键的过程,当第二次再次按下时
			// 会自动触发销毁剑并收回
			// 右键触发技能瞄准剑
			stateMachine.ChangeState(player.aimSword);
		}

		if(Input.GetKeyDown(KeyCode.R) && player.skill.blackhole.blackholeUnlocked)
		{
			if (player.skill.blackhole.cooldownTimer < 0)
			{
				// 转为黑洞状态
				stateMachine.ChangeState(player.blackholeState);
			}
			else
			{
				player.fx.CreatePopUpText("Skill is on cooldown");
			}
		}

		if(player.IsGroundDetected())
		{
			jumpCount = 0;
		}

	}

	private bool HashNoSword()
	{
		if(!player.sword)
		{
			return true;
		}
		player.sword.GetComponent<Sword_Skill_Controller>().ReturnSword();
		return false;
	}
}
