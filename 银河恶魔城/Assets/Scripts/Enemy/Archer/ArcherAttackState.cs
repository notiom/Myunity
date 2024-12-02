using UnityEngine;

public class ArcherAttackState : EnemyState
{
	private Enemy_Archer enemy;

	public ArcherAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Archer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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
