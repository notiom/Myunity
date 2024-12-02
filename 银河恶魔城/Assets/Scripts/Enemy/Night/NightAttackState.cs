using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightAttackState : EnemyState
{
	private Enemy_Night enemy;

	public NightAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Night _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
	{
		this.enemy = _enemy;
	}

	public override void Enter()
	{
		base.Enter();
	}

	public override void Exit()
	{
		base.Exit();
		enemy.lastTimeAttacked = Time.time;
	}

	public override void Update()
	{
		base.Update();
		enemy.SetVelocity(0, 0);

		if (triggerCalled)
		{
			if (PlayerManager.instance.player.GetComponent<CharacterStats>().isDead)
			{
				stateMachine.ChangeState(enemy.moveState);
				return;
			}
			stateMachine.ChangeState(enemy.battleState);
		}
	}
}
