using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackholeState : PlayerState
{
	private float flyTime = .25f;
	private bool skillUsed;
	private float defaultGravity;
	public PlayerBlackholeState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
	{

	}

	public override void Enter()
	{
		base.Enter();
		defaultGravity = rb.gravityScale;
		player.gameObject.layer = LayerMask.NameToLayer("InvinciblePlayer");
		skillUsed = false;
		stateTimer = flyTime;
		rb.gravityScale = 0;
	}

	public override void Exit()
	{
		player.gameObject.layer = LayerMask.NameToLayer("Player");
		rb.gravityScale = defaultGravity;
		player.fx.MakeTransparent(false);
		base.Exit();

	}

	public override void Update()
	{
		base.Update();
		if(stateTimer > 0) 
		{
			rb.velocity = new Vector2(0, 10);
		}
		else
		{
			rb.velocity = new Vector2 (0, -.1f);
			if(!skillUsed)
			{
				if(player.skill.blackhole.CanUseSkill())
					skillUsed = true;
			}
		}
		/////////////////////////////////
		// We Exit state in blackhole skills controller whel all of the attacks are over
		////////////////////////////////
		/// 预防空中使用冲刺技能
		 
		if (player.skill.blackhole.BlackholeFinished())
			if(player.IsGroundDetected())
				stateMachine.ChangeState(player.idleState);
			else
				stateMachine.ChangeState(player.airState);
	}

	public override void AnimationFinishTrigger()
	{
		base.AnimationFinishTrigger();
	}
}
