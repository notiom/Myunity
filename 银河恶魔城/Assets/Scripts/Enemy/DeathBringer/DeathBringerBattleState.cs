using UnityEngine;

public class DeathBringerBattleState : EnemyState
{
	private Transform player;
	private Enemy_DeathBringer enemy;


	public DeathBringerBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
	{
		this.enemy = _enemy;
	}

	public override void Enter()
	{
		base.Enter();
		player = PlayerManager.instance.player.transform;

		//if (player.GetComponent<CharacterStats>().isDead)
		//{
		//	stateMachine.ChangeState(enemy.moveState);
		//}
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
			if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
			{
				if (canAttack()) stateMachine.ChangeState(enemy.attackState);
				else stateMachine.ChangeState(enemy.idleState);
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

		if (enemy.IsPlayerDetected().distance < enemy.attackDistance && !canAttack())
		{
			return;
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
