using System.Collections;
using UnityEngine;

// 121集
public class Player : Entity
{


	#region States
	public PlayerStateMachine StateMachine { get; private set; }

	public PlayerIdleState idleState { get; private set; }

	public PlayerMoveState moveState { get; private set; }

	public PlayerJumpState jumpState { get; private set; }

	public PlayerAirState airState { get; private set; }

	public PlayerDashState dashState { get; private set; }

	public PlayerWallSlideState wallSlideState { get; private set; }

	public PlayerWallJumpState wallJump { get; private set; }

	public PlayerPrimaryAttack primaryAttack { get; private set; }

	public PlayerCounterAttack counterAttack { get; private set; }

	public PlayerDeadState deadState { get; private set; }

	public PlayerAimSwordState aimSword { get; private set; }

	public PlayerCatchSwordState catchSword { get; private set; }

	public PlayerBlackholeState blackholeState { get; private set; }

	public SkillManager skill { get; private set; }
	public GameObject sword { get; private set; }
	#endregion

	public bool isBusy { get; private set; }
	[Header("Attack details")]
	public Vector2[] attackMovement;
	public float counterAttackDuration = .2f;
	[Header("Move info")]
	public float moveSpeed;
	private float defaultMoveSpeed;
	public float swordReturnImpact; // 剑的击退力量
	[Header("Jump info")]
	public float jumpForce;
	private float defaultjumpForce;


	[Header("Dash info")]
	// 冷却时间
	[SerializeField] public float dashCoolDown;
	[SerializeField] private float dashUsageTimer;
	public float dashSpeed;
	public float dashDuration;

	private float defaultDashSpeed;

	public FacingDir dashDir { get; set; } = FacingDir.Right;
	// [Header("Attack info")] 

	public bool isNotRestrict;

	#region Awake
	protected override void Awake()
	{
		base.Awake();
		StateMachine = new PlayerStateMachine();
		idleState = new PlayerIdleState(StateMachine, this, "Idle");
		moveState = new PlayerMoveState(StateMachine, this, "Move");
		jumpState = new PlayerJumpState(StateMachine, this, "Jump");
		airState = new PlayerAirState(StateMachine, this, "Jump");
		dashState = new PlayerDashState(StateMachine, this, "Dash");
		wallSlideState = new PlayerWallSlideState(StateMachine, this, "WallSlide");
		wallJump = new PlayerWallJumpState(StateMachine, this, "Jump");
		primaryAttack = new PlayerPrimaryAttack(StateMachine, this, "Attack");
		counterAttack = new PlayerCounterAttack(StateMachine, this, "CounterAttack");
		aimSword = new PlayerAimSwordState(StateMachine, this, "AimSword");
		catchSword = new PlayerCatchSwordState(StateMachine, this, "CatchSword");
		blackholeState = new PlayerBlackholeState(StateMachine, this, "Jump");
		deadState = new PlayerDeadState(StateMachine, this, "Dead");
	}
	#endregion
	protected override void Start()
	{
		base.Start();
		skill = SkillManager.instance;
		// 子类也可以被传递
		groundCheckDistance = 0.2f;
		wallCheckDistance = 0.2f;
		jumpForce = 10f;
		moveSpeed = 5f;
		defaultMoveSpeed = moveSpeed;
		defaultjumpForce = jumpForce;
		defaultDashSpeed = dashSpeed;
		attackMovement = new Vector2[3] { new Vector2(3, 2), new Vector2(1, 3), new Vector2(4, 2) };
		StateMachine.Initialize(idleState);
	}

	protected override void Update()
	{
		transform.position = new Vector3(transform.position.x, transform.position.y, 0);
		if (Time.timeScale == 0)
			return;
		base.Update();
		StateMachine.currentState.Update();
		CheckForDashInput();
		// 清除剑的dots
		if (!sword && !Input.GetKey(KeyCode.O))
		{
			SkillManager.instance.sword.ClearAllDots();
		}

		if (Input.GetKeyDown(KeyCode.Space) && skill.crystal.crystalUnlocked)
		{
			// 空格释放水晶
			skill.crystal.CanUseSkill();
		}

		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			Inventory.instance.UseFlask();
		}
		RestrictPosition(PlayerManager.instance.rightBound);
	}

	protected override void RestrictPosition(int rightIndex)
	{
		if(isNotRestrict)
		{
			return;
		}
		base.RestrictPosition(rightIndex);
	}

	public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
	{
		moveSpeed = moveSpeed * (1 - _slowPercentage);
		jumpForce = jumpForce * (1 - _slowPercentage);
		dashSpeed = dashSpeed * (1 - _slowPercentage);
		anim.speed = anim.speed * (1 - _slowPercentage);

		Invoke("ReturnDefalutSpeed", _slowDuration);
	}

	protected override void ReturnDefalutSpeed()
	{
		base.ReturnDefalutSpeed();

		moveSpeed = defaultMoveSpeed;
		jumpForce = defaultjumpForce;
		dashSpeed = defaultDashSpeed;
	}

	private void CheckForDashInput()
	{
		if (IsWallDetected()) return;
		if (Input.GetKeyDown(KeyCode.L) && SkillManager.instance.dash.CanUseSkill())
		{
			// 按下L键进行冲刺
			float dashDirection = Input.GetAxisRaw("Horizontal");
			if (dashDirection == 0)
				dashDir = facingDir;
			else
			{
				dashDir = dashDirection > 0 ? FacingDir.Right : FacingDir.Left;

			}
			StateMachine.ChangeState(dashState);
		}
	}

	public void AssignNewSword(GameObject _newSword)
	{
		sword = _newSword;
	}

	public void CatchTheSword()
	{
		StateMachine.ChangeState(catchSword);
		Destroy(sword);
	}

	public void ExitBlackHoleAbility()
	{
		StateMachine.ChangeState(airState);
	}

	// 协程
	public IEnumerator BusyFor(float _seconds)
	{
		isBusy = true;
		Debug.Log("IS BUSY");
		yield return new WaitForSeconds(_seconds);
		Debug.Log("NOT BUSY");

		isBusy = false;
	}

	public virtual void AnimationTrigger() => StateMachine.currentState.AnimationFinishTrigger();

	public override void Die()
	{
		base.Die();
		StateMachine.ChangeState(deadState);
	}

}
