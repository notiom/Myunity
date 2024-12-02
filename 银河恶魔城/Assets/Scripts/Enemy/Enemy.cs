using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent (typeof(CapsuleCollider2D))]
[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(EntityFX))]
[RequireComponent(typeof(ItemDrop))]
public class Enemy : Entity
{
	[SerializeField] protected LayerMask whatIsPlayer;
	[Header("Stunned info")]
	public float stunDuration = 1;
	public Vector2 stunDirection = new Vector2(3,10);
	protected bool canBeStunned;
	[SerializeField] protected GameObject counterImage;
	[Header("Move info")]
	public float moveSpeed = 1.5f;
	public float idleTime = 2f;
	private float defaultMoveSpeed;

	[Header("Attack info")]
	public float attackDistance = 2;
	public float attackCooldown;
	public float minAttackCooldown = 1;
	public float maxAttackCooldown = 2;
	public float battleTimer = 5;
	[HideInInspector] public float lastTimeAttacked;

	public EnemyStateMachine stateMachine { get; private set; }
	public string lastAnimBoolName { get; private set; }

	protected override void Awake()
	{
		base.Awake();
		stateMachine = new EnemyStateMachine();

		defaultMoveSpeed = moveSpeed;
	}
	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
	}

	// Update is called once per frame
	protected override void Update()
	{
		base.Update();
		RestrictPosition(PlayerManager.instance.rightBound);
		stateMachine.currentState.Update();
	}

	public virtual void AssignLastAnimName(string _animBoolName) => lastAnimBoolName = _animBoolName;

	public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
	{
		moveSpeed = moveSpeed * (1 - _slowPercentage);
		anim.speed = anim.speed * (1 - _slowPercentage);

		Invoke("ReturnDefalutSpeed", _slowDuration);
	}
	protected override void ReturnDefalutSpeed()
	{
		base.ReturnDefalutSpeed();
		moveSpeed = defaultMoveSpeed;
	}
	public virtual void FreezeTime(bool _timeFrozen)
	{
		if (_timeFrozen)
		{
			moveSpeed = 0;
			anim.speed = 0;
		}
		else
		{
			moveSpeed = defaultMoveSpeed;
			anim.speed = 1;
		}
	}

	public virtual void FreezeTimeFor(float _duration) => StartCoroutine(FreezeTimeCoroutine(_duration));

	protected virtual IEnumerator FreezeTimeCoroutine(float _seconds)
	{
		FreezeTime(true);

		yield return new WaitForSeconds(_seconds);

		FreezeTime(false);
	}

	#region Counter Attack Window
	public virtual void OpenCounterAttackWindow()
	{
		canBeStunned = true;
		counterImage.SetActive(true);
	}

	public virtual void CloseCounterAttackWindow()
	{
		canBeStunned = false;
		counterImage.SetActive(false);
	}
	#endregion

	public virtual bool CanBeStunned()
	{
		if (canBeStunned)
		{
			// 这段代码的执行逻辑为当骷髅闪出红色标志时，可以使用反击技能，将怪物攻击打断
			// 至于进入的击退晕眩状态，是在之后重写函数得到的
			CloseCounterAttackWindow();
			return true;
		}

		return false;
	}
	public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(attackCheck.position,
																		Vector2.right * (int)facingDir,
																		50, whatIsPlayer);
	public virtual void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

	public virtual void AnimationSpecialAttackTrigger()
	{

	}

	public override void Die()
	{
		base.Die();
		EnemyManager.instance.currentEnemies.Dequeue();
	}

	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * (int)facingDir, transform.position.y));
	}

	public void DestroyMe() => Destroy(gameObject);

}


