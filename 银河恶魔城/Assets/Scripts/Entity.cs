using System.Collections;
using System.Linq;
using UnityEngine;

public enum FacingDir
{
	Left = -1,
	Right = 1
}
public class Entity : MonoBehaviour
{
	#region Components
	public Animator anim { get; set; }

	public Rigidbody2D rb { get; set; }

	public EntityFX fx { get; set; }

	public SpriteRenderer sr { get; set; }
	public CharacterStats stats { get; set; }

	public CapsuleCollider2D cd { get; private set; }
	#endregion
	[Header("Collision info")]

	[SerializeField] protected Transform groundCheck;
	[SerializeField] protected float groundCheckDistance = 1;
	[SerializeField] protected Transform wallCheck;
	[SerializeField] protected float wallCheckDistance = 0.8f;
	[SerializeField] protected LayerMask whatIsGround;
	[Header("Knocback info")]
	[SerializeField] protected Vector2 knockbackPower = new Vector2(7,12);
	[SerializeField] protected float knockbackDuration = 0.07f;
	protected bool isKoncked;

	public Transform attackCheck;
	public float attackCheckRadius = 1.2f;

	public int knockbackDir { get; private set; }
	public FacingDir facingDir { get; set; } = FacingDir.Right;

	public System.Action onFlipped;

	protected Transform[] checkpoints;

	protected virtual void Awake()
	{
		checkpoints = PlayerManager.instance.checkpoints;
	}

	protected virtual void Start()
	{
		sr = GetComponentInChildren<SpriteRenderer>();
		anim = GetComponentInChildren<Animator>();
		rb = GetComponent<Rigidbody2D>();
		fx = GetComponent<EntityFX>();
		stats = GetComponent<CharacterStats>();
		cd = GetComponent<CapsuleCollider2D>();
		// 开始的时候默认剪切
		// RestrictPosition(CheckPosition());

	}

	protected virtual int CheckPosition()
	{
		int currentCheckpoint = -1;
		for (int i = 0; i < checkpoints.Length; i++)
		{
			if (transform.position.x < checkpoints[i].position.x)
			{
				currentCheckpoint = i;
				break;
			}
		}
		return currentCheckpoint;
	}

	protected virtual void RestrictPosition(int rightIndex)
	{
		Vector3 position = transform.position;

		float minY = float.PositiveInfinity;
		if (rightIndex < checkpoints.Length)
		{
			minY = checkpoints[rightIndex].position.x - 1.5f;
		}

		// 限制玩家位置在边界内
		// 
		float minX = float.NegativeInfinity;
		if (rightIndex != 0)
		{
			minX = checkpoints[rightIndex - 1].position.x - 1.5f; 
		}

		position.x = Mathf.Clamp(position.x, minX, minY);

		transform.position = position;
	}

	protected virtual void Update()
	{

	}
	public void SetVelocity(float _xVelocity, float _yVelocity)
	{
		if (isKoncked) return;
		rb.velocity = new Vector2(_xVelocity, _yVelocity);
		FlipController(_xVelocity);
	}

	public virtual void SlowEntityBy(float _slowPercentage, float _slowDuration)
	{

	}

	protected virtual void ReturnDefalutSpeed()
	{
		anim.speed = 1;
	}

	public virtual void DamageEffect()
	{
		fx.StartCoroutine("FlashFX");
		StartCoroutine("HitKnockback");
		// Debug.Log(gameObject.name + "was damaged!");
	}

	public virtual void SetupKnockbackDir(Transform _damageDirection)
	{
		if (_damageDirection.position.x > transform.position.x)
		{
			knockbackDir = -1;
		}
		else if (_damageDirection.position.x < transform.position.x)
		{
			knockbackDir = 1;
		}
	}

	protected virtual IEnumerator HitKnockback()
	{
		isKoncked = true;
		FacingDir deafaultDir = knockbackDir == 1 ? FacingDir.Left : FacingDir.Right;
		if (deafaultDir != facingDir)
		{
			Flip();
		}
		rb.velocity = new Vector2(knockbackPower.x * knockbackDir, knockbackPower.y);

		yield return new WaitForSeconds(knockbackDuration);

		rb.velocity = new Vector2(0, 0);
		isKoncked = false;
	}

	#region Flip
	protected void FlipController(float _xVelocity)
	{
		if (_xVelocity * (int)facingDir < 0)

		{
			Flip();
		}
	}
	internal void Flip()
	{
		// 改变人物朝向
		facingDir = facingDir == FacingDir.Left ? FacingDir.Right : FacingDir.Left;
		transform.Rotate(0, 180, 0);

		onFlipped?.Invoke();
	}
	#endregion
	#region Collision
	// 隐式转换，直接将后面的值赋值给前面
	public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
	public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * (int)facingDir, wallCheckDistance, whatIsGround);

	protected virtual void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
		Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + (int)facingDir * wallCheckDistance, wallCheck.position.y));
		Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
	}
	#endregion

	public virtual void Die()
	{
		if (GetComponent<CharacterStats>().isDead) return;
	}
}
