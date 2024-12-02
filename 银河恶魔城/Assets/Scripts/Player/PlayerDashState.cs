using UnityEngine;

public class PlayerDashState : PlayerState
{
	public PlayerDashState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();
		// 冲刺时进入无敌层，无法和敌人发生碰撞
		player.skill.clone.CreateCloneonDashBegun(SkillManager.instance.dash.createCloneOnDashStart);
		player.gameObject.layer = LayerMask.NameToLayer("InvinciblePlayer");
		player.stats.MakeInvincible(true);
		stateTimer = player.dashDuration;
	}

	public override void Exit()
	{
		base.Exit();
		player.skill.clone.CreateCloneOnDashFinish(SkillManager.instance.dash.createCloneOnDashFinish);
		player.gameObject.layer = LayerMask.NameToLayer("Player");

		player.SetVelocity(0, rb.velocity.y);
		player.stats.MakeInvincible(false);
	}

	public override void Update()
	{
		base.Update();
		if (stateTimer < player.dashDuration - 0.5f)
		{
			if (!player.IsGroundDetected() && player.IsWallDetected())
			{
				// 在空中冲刺到了另一面墙上的情况
				stateMachine.ChangeState(player.wallSlideState);
				return;
			}
		}

		player.SetVelocity(player.dashSpeed * (int)player.dashDir, 0);
		if (stateTimer < 0)
		{
			if (!player.IsGroundDetected())
			{
				// 如果此时还在空中,状态就是air
				stateMachine.ChangeState(player.airState);
			}
			else
			{
				stateMachine.ChangeState(player.idleState);
			}
		}

		player.fx.CreateAfterImage();
	}
}
