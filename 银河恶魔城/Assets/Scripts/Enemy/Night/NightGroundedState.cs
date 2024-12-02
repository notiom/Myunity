using UnityEngine;

public class NightGroundedState : EnemyState
{
	protected Enemy_Night enemy;
	protected Transform player;

	public NightGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Night _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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
		if (enemy.IsPlayerDetected() || Vector2.Distance(player.position, enemy.transform.position) < 3)
		{
			// Debug.Log("I see" + enemy.IsPlayerDetected().collider.gameObject.name);
			stateMachine.ChangeState(enemy.battleState);
		}
	}
}
