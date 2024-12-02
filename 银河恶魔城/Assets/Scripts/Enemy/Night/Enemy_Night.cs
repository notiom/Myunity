using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Night : Enemy
{
	#region States
	public NightIdleState idleState { get; private set; }
	public NightMoveState moveState { get; private set; }
	public NightBattleState battleState { get; private set; }
	public NightAttackState attackState { get; private set; }
	public NightStunnedState stunnedState { get; private set; }
	public NightDeadState deadState { get; private set; }

	#endregion

	#region Awake
	protected override void Awake()
	{
		base.Awake();
		idleState = new NightIdleState(this, stateMachine, "Idle", this);
		moveState = new NightMoveState(this, stateMachine, "Move", this);
		battleState = new NightBattleState(this, stateMachine, "Move", this);
		attackState = new NightAttackState(this, stateMachine, "Attack", this);
		stunnedState = new NightStunnedState(this, stateMachine, "Stunned", this);
		deadState = new NightDeadState(this, stateMachine, "Dead", this);
	}
	#endregion

	protected override void Start()
	{
		base.Start();
		stateMachine.Initialize(idleState);
	}

	protected override void Update()
	{
		base.Update();
		/*
		if(Input.GetKeyDown(KeyCode.U))
		{
			stateMachine.ChangeState(stunnedState);
		}
		*/
	}

	public override bool CanBeStunned()
	{
		if (base.CanBeStunned())
		{
			stateMachine.ChangeState(stunnedState);
			return true;
		}
		return false;
	}

	public override void Die()
	{
		base.Die();
		stateMachine.ChangeState(deadState);
	}

	public void Explode() => deadState.isExplosion = true;

}
