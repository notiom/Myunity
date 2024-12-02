using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
	public PlayerAimSwordState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
	{

	}

	public override void Enter()
	{
		base.Enter();
		player.skill.sword.DotsActive(true);
		player.skill.sword.accumlateTimer = 0f;
	}

	public override void Exit()
	{
		base.Exit();

		player.StartCoroutine("BusyFor", .2f);
	}

	public override void Update()
	{
		base.Update();
		rb.velocity = new Vector2(0, 0);

		if (Input.GetKeyUp(KeyCode.O))
		{
			stateMachine.ChangeState(player.idleState);
		}
	}
}
