using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerGroundedState
{
	public PlayerWallSlideState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void Update()
	{
		base.Update();

		if(!player.IsWallDetected()) 
		{
			// 如果没有检测到墙体
			stateMachine.ChangeState(player.airState);
		}
		if(Input.GetKeyDown(KeyCode.K)) 
		{
			stateMachine.ChangeState(player.wallJump);
			return;
		}
		if (Input.GetKeyDown(KeyCode.L))
		{
			stateMachine.ChangeState(player.dashState);
			return;
		}
		// 使玩家控制下落速度变快
		if (yinput < 0)
		{
			rb.velocity = new Vector2(0, rb.velocity.y);
		}
		else
		{
			// 在墙上下落的速度变慢
			rb.velocity = new Vector2(0, rb.velocity.y * .7f);
		}
		if((int)player.facingDir * xinput < 0 || player.IsGroundDetected())
		{
			// 从墙上下来的条件
			// 1.在墙上有反方向的移动
			// 2.已经落地
			stateMachine.ChangeState(player.idleState);
			return;
		}
	}
}
