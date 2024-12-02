using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGroundedState : EnemyState
{
	protected Enemy_Skeleton enemy;
	protected Transform player;

	public SkeletonGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
	{
		this.enemy = _enemy;
	}

	public override void Enter()
	{
		base.Enter();
		player = PlayerManager.instance.player.transform;
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void Update()
	{
		base.Update();
		if (enemy.facingDir != moveDir)
		{
			moveDir = moveDir == FacingDir.Right ? FacingDir.Left : FacingDir.Right;
		}

		if (player.GetComponent<CharacterStats>().isDead)
		{
			stateMachine.ChangeState(enemy.moveState);
			return;
		}
		if (enemy.IsPlayerDetected() || Vector2.Distance(player.position, enemy.transform.position) < 2)
		{
			// Debug.Log("I see" + enemy.IsPlayerDetected().collider.gameObject.name);
			stateMachine.ChangeState(enemy.battleState);
		}
	}
}
