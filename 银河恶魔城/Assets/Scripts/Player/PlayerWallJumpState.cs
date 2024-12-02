using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerState
{
	public PlayerWallJumpState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();
		stateTimer = .4f;
		player.SetVelocity(5 * -(int)player.facingDir, player.jumpForce);
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void Update()
	{
		base.Update();
		if (player.IsGroundDetected())
		{
			stateMachine.ChangeState(player.idleState);
			return;
		}

		if (stateTimer < 0) 
		{
			stateMachine.ChangeState(player.airState);
			return;
		}


	}
}
