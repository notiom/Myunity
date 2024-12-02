using UnityEngine;

public class ArcherGroundedState : EnemyState
{
	protected Enemy_Archer enemy;
	protected Transform player;

	public ArcherGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Archer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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
		// ²»ÄÜ¼ì²édistance
		if (enemy.IsPlayerDetected() || (Mathf.Abs(player.position.x - enemy.transform.position.x) < 7 && Mathf.Abs(player.position.y - enemy.transform.position.y) < 2))
		{
			// Debug.Log("I see" + enemy.IsPlayerDetected().collider.gameObject.name);
			stateMachine.ChangeState(enemy.battleState);
		}
	}

}

