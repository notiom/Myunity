using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerState
{
	public PlayerDeadState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
	{
	}

	public override void AnimationFinishTrigger()
	{
		base.AnimationFinishTrigger();
	}

	public override void Enter()
	{
		base.Enter();
		Debug.Log(GameObject.Find("Canvas").GetComponent<UI>() == null);
		GameObject.Find("Canvas").GetComponent<UI>().SwitchOnEndScreen(false);
		// 死亡无法与敌人碰撞
		player.gameObject.layer = LayerMask.NameToLayer("InvinciblePlayer");
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void Update()
	{
		base.Update();
		player.SetVelocity(0, 0);
	}
}
