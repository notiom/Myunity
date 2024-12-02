using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBattleState : EnemyState
{
	private Transform player;
	private Enemy_Slime enemy;


	public SlimeBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Slime _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
	{
		this.enemy = _enemy;
	}

	public override void Enter()
	{
		base.Enter();
		player = PlayerManager.instance.player.transform;
		if (player.GetComponent<CharacterStats>().isDead)
		{
			stateMachine.ChangeState(enemy.moveState);
		}
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void Update()
	{
		base.Update();
		if (enemy.IsPlayerDetected() && !player.GetComponent<CharacterStats>().isDead)
		{
			stateTimer = enemy.battleTimer;
			if (enemy.IsPlayerDetected().distance < enemy.attackDistance && canAttack())
			{
				stateMachine.ChangeState(enemy.attackState);
				return;
			}
			else if (enemy.IsPlayerDetected().distance < enemy.attackDistance && !canAttack())
			{
				return;
			}
		}
		else
		{
			// 战斗时间过了或者玩家距离敌人太远或者玩家死亡
			if (stateTimer < 0 || Vector2.Distance(player.position, enemy.transform.position) > 7)
			{
				stateMachine.ChangeState(enemy.idleState);
			}
		}
		if (player.position.x > enemy.transform.position.x)
		{
			moveDir = FacingDir.Right;
		}
		else if (player.position.x < enemy.transform.position.x)
		{
			moveDir = FacingDir.Left;
		}
		enemy.SetVelocity(enemy.moveSpeed * (int)moveDir, rb.velocity.y);
	}

	private bool canAttack()
	{
		if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
		{
			enemy.attackCooldown = Random.Range(enemy.minAttackCooldown, enemy.maxAttackCooldown);
			enemy.lastTimeAttacked = Time.time;
			return true;
		}
		return false;
	}
}
