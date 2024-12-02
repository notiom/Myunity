using UnityEngine;

public class DeathBringerAttackState : EnemyState
{
	private Enemy_DeathBringer enemy;

	public DeathBringerAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
	{
		this.enemy = _enemy;
	}

	public override void Enter()
	{
		base.Enter();
		enemy.isBusy = true;
		if (enemy.fireOrIce)
		{
			enemy.stats.fireDamage.AddModifier(1);
		}
		else
		{
			enemy.stats.iceDamage.AddModifier(1);
		}
	}

	public override void Exit()
	{
		base.Exit();
		enemy.lastTimeAttacked = Time.time;
		enemy.isBusy = false;
		if (enemy.fireOrIce)
		{
			enemy.stats.fireDamage.RemoveModifier(1);
		}
		else
		{
			enemy.stats.iceDamage.RemoveModifier(1);
		}
		enemy.fireOrIce = !enemy.fireOrIce;
	}

	public override void Update()
	{
		base.Update();
		enemy.SetVelocity(0, 0);

		if (triggerCalled)
		{
			if (PlayerManager.instance.player.GetComponent<CharacterStats>().isDead)
			{
				stateMachine.ChangeState(enemy.idleState);
				return;
			}

			stateMachine.ChangeState(enemy.battleState);

		}
	}
}
