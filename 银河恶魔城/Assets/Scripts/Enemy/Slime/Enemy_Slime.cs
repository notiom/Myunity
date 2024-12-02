using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlimeType
{
	big,
	medium,
	small
}

// 该怪物的特性是死后复制一个更小的自己
public class Enemy_Slime : Enemy
{
	[Header("Slime spesific")]
	[SerializeField] private SlimeType slimeType;
	[SerializeField] private int slimesToCreate;
	[SerializeField] private GameObject slimePrefab;
	[SerializeField] private Vector2 minCreateionVelocity;
	[SerializeField] private Vector2 maxCreateionVelocity;

	#region States
	public SlimeIdleState idleState { get; private set; }
	public SlimeMoveState moveState { get; private set; }
	public SlimeBattleState battleState { get; private set; }
	public SlimeAttackState attackState { get; private set; }
	public SlimeStunnedState stunnedState { get; private set; }
	public SlimeDeadState deadState { get; private set; }

	#endregion

	#region Awake
	protected override void Awake()
	{
		base.Awake();
		idleState = new SlimeIdleState(this, stateMachine, "Idle", this);
		moveState = new SlimeMoveState(this, stateMachine, "Move", this);
		battleState = new SlimeBattleState(this, stateMachine, "Move", this);
		attackState = new SlimeAttackState(this, stateMachine, "Attack", this);
		stunnedState = new SlimeStunnedState(this, stateMachine, "Stunned", this);
		deadState = new SlimeDeadState(this, stateMachine, "Dead", this);
	}
	#endregion

	protected override void Start()
	{
		base.Start();
		transform.Rotate(new Vector3(0, 180, 0));
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

		if (slimeType == SlimeType.small)
		{
			return;
		}
		CreateSlimes(slimesToCreate, slimePrefab);

	}

	public void CreateSlimes(int _amountOfSlime,GameObject _slimePrefab)
	{
		for(int i = 0;i < _amountOfSlime;i++) 
		{
			GameObject newSlime = Instantiate(_slimePrefab, transform.position, Quaternion.identity);
			newSlime.GetComponent<Enemy_Slime>().SetupSlime(facingDir);
			newSlime.transform.parent = EnemyManager.instance.enemiesParent;

			EnemyManager.instance.currentEnemies.Enqueue(0);
		}
	}
	public void SetupSlime(FacingDir _facingDir)
	{
		if (facingDir != _facingDir)
			Flip();
		float xVelocity = Random.Range(minCreateionVelocity.x,maxCreateionVelocity.x);
		float yVelocity = Random.Range(minCreateionVelocity.y, maxCreateionVelocity.y);

		isKoncked = true;

		GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity * (int)facingDir, yVelocity);

		Invoke("CancelKnockback", 1.5f);
	}

	private void CancelKnockback() => isKoncked = false;
}

