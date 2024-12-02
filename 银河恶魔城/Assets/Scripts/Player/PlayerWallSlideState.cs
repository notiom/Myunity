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
			// ���û�м�⵽ǽ��
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
		// ʹ��ҿ��������ٶȱ��
		if (yinput < 0)
		{
			rb.velocity = new Vector2(0, rb.velocity.y);
		}
		else
		{
			// ��ǽ��������ٶȱ���
			rb.velocity = new Vector2(0, rb.velocity.y * .7f);
		}
		if((int)player.facingDir * xinput < 0 || player.IsGroundDetected())
		{
			// ��ǽ������������
			// 1.��ǽ���з�������ƶ�
			// 2.�Ѿ����
			stateMachine.ChangeState(player.idleState);
			return;
		}
	}
}
