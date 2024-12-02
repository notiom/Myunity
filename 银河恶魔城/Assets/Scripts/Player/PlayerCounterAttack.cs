using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounterAttack : PlayerState
{
	private bool canCreateClone;
	public PlayerCounterAttack(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();
		if(player.skill.parry.parryWithMirageUnlocked)
		{
			// 仅仅可以反击生成一次
			canCreateClone = true;
		}
		stateTimer = player.counterAttackDuration;
		player.anim.SetBool("SuccessfulCounterAttack", false);
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void Update()
	{
		base.Update();
		player.SetVelocity(0, 0); // 保证在反击时不会移动
		Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
		foreach (var hit in colliders)
		{
			if(hit.GetComponent<Arrow_Controller>() != null)
			{
				hit.GetComponent<Arrow_Controller>().FlipArrow();
				SuccessfulCounterAttack();
			}

			if (hit.GetComponent<Enemy>() != null)
			{
				// 有敌人在攻击范围内 
				if(hit.GetComponent<Enemy>().CanBeStunned())
				{
					SuccessfulCounterAttack();

					player.skill.parry.UseSkill(); // goint to use to restore health on parry

					if (canCreateClone)
					{
						canCreateClone = false;
						player.skill.clone.CreateCloneOnCounterAttack(hit.transform);
					}
				}
			}
		}

		if(stateTimer < 0 || triggerCalled)
		{
			stateMachine.ChangeState(player.idleState);
		}
	}

	private void SuccessfulCounterAttack()
	{
		stateTimer = 10; /// 大于1的任何值
		player.anim.SetBool("SuccessfulCounterAttack", true);
	}
}
