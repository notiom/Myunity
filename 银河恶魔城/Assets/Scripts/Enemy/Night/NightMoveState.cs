using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightMoveState : NightGroundedState
{
	public NightMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Night _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
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

		enemy.SetVelocity(enemy.moveSpeed * (int)enemy.facingDir, rb.velocity.y);
		if (!enemy.IsGroundDetected() || enemy.IsWallDetected())
		{
			enemy.Flip();
			stateMachine.ChangeState(enemy.idleState);
		}
	}
}
