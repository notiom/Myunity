using UnityEngine;

public class ArcherBattleState : EnemyState
{
	private Transform player;
	private Enemy_Archer enemy;

	private bool isFlip;
	public ArcherBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Archer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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
			if (enemy.IsPlayerDetected().distance < enemy.safeDistance)
			{
				if (CanJump())
				{
					stateMachine.ChangeState(enemy.jumpState);
					return;
				}

			}

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
			// 战斗时间过了或者玩家距离弓箭手的横向坐标太远或者纵向坐标太远
			if (stateTimer < 0 || Mathf.Abs(player.position.y - enemy.transform.position.y) > 2)
			{
				stateMachine.ChangeState(enemy.idleState);
				return;
			}

			if (Mathf.Abs(player.position.x - enemy.transform.position.x) > 7)
			{
				stateMachine.ChangeState(enemy.idleState);
				return;
			}

			if (player.position.x > enemy.transform.position.x)
			{
				if (enemy.facingDir != FacingDir.Right) enemy.Flip();
			}

			else
			{
				if (enemy.facingDir != FacingDir.Left) enemy.Flip();
			}
		}
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

	private bool CanJump()
	{
		if (Time.time >= enemy.lastTimeJump + enemy.jumpCooldown)
		{
			enemy.lastTimeJump = Time.time;
			return true;
		}
		return false;
	}
}
