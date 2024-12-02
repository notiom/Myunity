using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
	public PlayerAirState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
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
		if (player.IsGroundDetected())
		{
			// ����ı�����ȼ�Ҫ���ڻ�ǽ
			stateMachine.ChangeState(player.idleState);
			return;
		}

		if (player.IsWallDetected()) 
		{
			stateMachine.ChangeState(player.wallSlideState);
			return;
		}

		if (jumpCount == 1 && Input.GetKeyDown(KeyCode.K))
		{
			jumpCount++;
			stateMachine.ChangeState(player.jumpState);
		}

		if (xinput != 0)
		{
			// �ڿ���Ӧ���ƶ�����΢��һ��
			player.SetVelocity(player.moveSpeed * .8f * xinput, rb.velocity.y);
		}
	}
}
