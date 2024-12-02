public class ArcherMoveState : ArcherGroundedState
{
	public ArcherMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Archer _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
	{
	}

	public override void Enter()
	{
		base.Enter();
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void Update()
	{
		base.Update();

		enemy.SetVelocity(enemy.moveSpeed * (int)enemy.facingDir, rb.velocity.y);
		if (!enemy.IsGroundDetected() || enemy.IsWallDetected())
		{
			enemy.Flip();
			stateMachine.ChangeState(enemy.idleState);
		}
	}
}
