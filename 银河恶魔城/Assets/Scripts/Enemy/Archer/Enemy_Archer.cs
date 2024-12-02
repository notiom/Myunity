using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Archer : Enemy
{
	[Header("Archer spisfc info")]
	[SerializeField] public Vector2 jumpVelocity;
	[SerializeField] public GameObject arrowPrefab;

	public float jumpCooldown;
	public float safeDistance; // 玩家距离多近时触发跳跃
	[HideInInspector] public float lastTimeJump;

	[Header("Arrow settings info")]
	[SerializeField] private float arrowSpeed;

	#region States
	public ArcherIdleState idleState { get; private set; }
	public ArcherMoveState moveState { get; private set; }
	public ArcherBattleState battleState { get; private set; }
	public ArcherAttackState attackState { get; private set; }
	public ArcherStunnedState stunnedState { get; private set; }
	public ArcherDeadState deadState { get; private set; }

	public ArcherJumpState jumpState { get; private set; }

	#endregion

	#region Awake
	protected override void Awake()
	{
		base.Awake();
		idleState = new ArcherIdleState(this, stateMachine, "Idle", this);
		moveState = new ArcherMoveState(this, stateMachine, "Move", this);
		battleState = new ArcherBattleState(this, stateMachine, "Idle", this);
		attackState = new ArcherAttackState(this, stateMachine, "Attack", this);
		stunnedState = new ArcherStunnedState(this, stateMachine, "Stunned", this);
		deadState = new ArcherDeadState(this, stateMachine, "Dead", this);
		jumpState = new ArcherJumpState(this, stateMachine, "Jump", this);
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

	public override void AnimationSpecialAttackTrigger()
	{
		GameObject newArrow = Instantiate(arrowPrefab, attackCheck.transform.position, Quaternion.identity);

		if (facingDir != FacingDir.Left) newArrow.transform.Rotate(0, 180, 0);

		newArrow.GetComponent<Arrow_Controller>().SetUpArrow(arrowSpeed * (int)facingDir, stats);
	}

}
