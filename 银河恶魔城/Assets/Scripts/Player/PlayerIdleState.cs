using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
	public PlayerIdleState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine,_player,_animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();
		player.SetVelocity(0, 0);
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void Update()
	{
		base.Update();
		if(player.IsWallDetected() && xinput * (int)player.facingDir > 0)
		{
			// ��⵽ǽ�����˶�����������һ��
			return;
		}
		if(xinput != 0 && !player.isBusy) 
		{
			stateMachine.ChangeState(player.moveState);
		}
	}
}
