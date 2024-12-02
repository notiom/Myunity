using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
	public PlayerJumpState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();
		rb.velocity = new Vector2(rb.velocity.x, player.jumpForce);
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void Update()
	{
		base.Update();
		if(jumpCount == 1 && Input.GetKeyDown(KeyCode.K)) 
		{
			jumpCount++;
			stateMachine.ChangeState(player.jumpState);
		}
		if(rb.velocity.y < 0)
		{
			stateMachine.ChangeState(player.airState);
		}
	}
}
