using UnityEngine;

public class DeathBringerIdleState : EnemyState
{
	private Enemy_DeathBringer enemy;
	private Transform player => PlayerManager.instance.player.transform;
	public DeathBringerIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
	{
		this.enemy = _enemy;
	}

	public override void Enter()
	{
		base.Enter();
		stateTimer = enemy.idleTime;
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void Update()
	{
		base.Update();
		if (Vector2.Distance(player.position, enemy.transform.position) < 7)
			enemy.bossbegunBattle = true;

		if (stateTimer < 0 && enemy.bossbegunBattle)
		{
			stateMachine.ChangeState(enemy.battleState);
		}
	}
}
