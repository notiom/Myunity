using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
	private Transform sword;
	public PlayerCatchSwordState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
	{

	}

	public override void Enter()
	{
		base.Enter();
		sword = player.sword.transform;
		player.fx.PlayDustFX();
		if(player.transform.position.x > sword.position.x && player.facingDir == FacingDir.Right)
		{
			player.Flip();
		}
		else if(player.transform.position.x < sword.position.x && player.facingDir == FacingDir.Left)
		{
			player.Flip();
		}
		rb.velocity = new Vector2 (player.swordReturnImpact * -(int)player.facingDir, rb.velocity.y);
	}

	public override void Exit()
	{
		base.Exit();
		player.StartCoroutine("BusyFor", .1f);
	}

	public override void Update()
	{
		base.Update();
		if(triggerCalled)
		{
			stateMachine.ChangeState(player.idleState);
		}
	}
}
