using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
	private Animator anim;
	private Rigidbody2D rb;
	private BoxCollider2D cd;
	private Player player;
	private bool isReturing = false;

	private float returnSpeed = 12;
	private bool canRotate = true;
	[Header("Pierce info")]
	private int pierceAmount;
	[Header("Bounce info")]
	[SerializeField] private float bounceSpeed;
	private bool isBouncing = false;
	private int bounceAmount = 4;
	private List<Transform> enemyTarget;

	private int targetIndex;
	[Header("Spin info")]
	private float maxTravelDistance;
	private float spinDuration;
	private float spinTimer;
	private bool wasStopped;
	private bool isSpinning;

	private float hitTimer;
	private float hitCooldown;

	private float freezeTimeDuration;
	// 让剑可以向前移动
	private float spinDirection;
	private void Awake()
	{
		anim = GetComponentInChildren<Animator>();
		rb = GetComponent<Rigidbody2D>();
		cd = GetComponent<BoxCollider2D>();
		player = PlayerManager.instance.player;
	}

	private void DestoryMe()
	{
		Destroy(gameObject);
	}
	public void SetupSword(Vector2 _dir, float _gravityScale, float _freezeTimeDuration, float _returnSpeed)
	{
		rb.velocity = _dir;
		rb.gravityScale = _gravityScale;
		freezeTimeDuration = _freezeTimeDuration;
		returnSpeed = _returnSpeed;
		if (pierceAmount <= 0)
			anim.SetBool("Rotation", true);

		spinDirection = Mathf.Clamp(rb.velocity.x, -0.5f, 0.5f);
		Invoke("DestoryMe", 7); // 7s后自动毁灭
	}

	public void SetupBounce(bool _isBouncing, int _amountOfBounces, float _bounceSpeed)
	{
		isBouncing = _isBouncing;
		bounceAmount = _amountOfBounces;
		bounceSpeed = _bounceSpeed;

		enemyTarget = new List<Transform>();

	}

	public void SetupPierce(int _pierceAmount)
	{
		pierceAmount = _pierceAmount;
	}

	public void SetupSpin(bool _isSpinning, float _maxTravelDistance, float _spinDuration, float _hitCooldown)
	{
		isSpinning = _isSpinning;
		maxTravelDistance = _maxTravelDistance;
		spinDuration = _spinDuration;
		hitCooldown = _hitCooldown;
	}

	private void Update()
	{
		if (canRotate)
		{
			transform.right = rb.velocity;
		}

		if (isReturing)
		{
			isBouncing = false;
			player.skill.sword.canEnterO = false;
			transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);
			if (Vector2.Distance(transform.position, player.transform.position) < 1)
			{
				// 重置冷却时间
				// SkillManager.instance.sword.cooldownTimer = SkillManager.instance.sword.cooldown;
				anim.SetBool("Rotation", false);
				player.CatchTheSword();
				player.skill.sword.canEnterO = true;
			}

			BounceLogic();
			SpinLogic();

		}
	}

	private void SpinLogic()
	{
		if (isSpinning)
		{
			if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
			{
				StopWhenSpinning();
			}

			if (wasStopped)
			{
				spinTimer -= Time.deltaTime;
				// 让这把剑可以朝前移动
				transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f * Time.deltaTime);
				if (spinTimer < 0)
				{
					isReturing = true;
					wasStopped = false;
					isSpinning = false;
				}
				hitTimer -= Time.deltaTime;
				if (hitTimer < 0)
				{
					hitTimer = hitCooldown;
					Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);
					foreach (var hit in colliders)
					{
						if (hit.GetComponent<Enemy>() != null)
						{
							SwordSkillDamage(hit.GetComponent<Enemy>());
						}
					}
				}
			}
		}
	}

	private void StopWhenSpinning()
	{
		wasStopped = true;
		rb.constraints = RigidbodyConstraints2D.FreezePosition;
		spinTimer = spinDuration;
	}

	private void BounceLogic()
	{
		if (isBouncing && enemyTarget.Count > 0)
		{
			transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);
			if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < .1f)
			{
				SwordSkillDamage(enemyTarget[targetIndex].GetComponent<Enemy>());
				targetIndex++;
				bounceAmount--;

				if (bounceAmount <= 0)
				{
					isBouncing = false;
					cd.enabled = true;
					isReturing = true;
				}

				if (targetIndex >= enemyTarget.Count) targetIndex = 0;
			}
		}
	}

	public void ReturnSword()
	{
		rb.constraints = RigidbodyConstraints2D.FreezeAll;
		transform.parent = null;
		cd.enabled = true; // 重新开启碰撞器
		isReturing = true;
		anim.SetBool("Rotation", true);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// 撞到东西停止旋转
		// 后续可以开发回旋镖扣血的功能

		if (isReturing)
		{
			if (collision.GetComponent<Enemy>() != null)
			{
				Enemy enemy = collision.GetComponent<Enemy>();
				SwordSkillDamage(enemy);
				// player.stats.DoDamage(collision.GetComponent<Enemy>().GetComponent<CharacterStats>());
			}
			return;
		}
		if (collision.GetComponent<Enemy>() != null)
		{
			Enemy enemy = collision.GetComponent<Enemy>();
			SwordSkillDamage(enemy);

		}
		SetupTargetsForBounce(collision);

		StuckInto(collision);
	}

	private void SwordSkillDamage(Enemy enemy)
	{
		bool flag = true;
		EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();
		player.stats.DoDamage(enemyStats);
		if(player.skill.sword.timestopUnlocked)
		{
			enemy.FreezeTimeFor(freezeTimeDuration);
		}
		
		if(player.skill.sword.volnurableUnlocked) 
		{
			enemyStats.MakeVurableFor(freezeTimeDuration);
		}
		EquipmentData equipedAmulet = Inventory.instance.GetEquipment(EquipmentType.Amulet);
		if (equipedAmulet != null && flag)
		{
			equipedAmulet.Effect(enemy.transform);
			flag = false;
		}
	}

	private void SetupTargetsForBounce(Collider2D collision)
	{
		if (collision.GetComponent<Enemy>() != null)
		{
			if (isBouncing && enemyTarget.Count <= 0)
			{
				Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);
				foreach (var hit in colliders)
				{
					if (hit.GetComponent<Enemy>() != null)
					{
						enemyTarget.Add(hit.transform);
					}
				}
			}
		}
	}

	private void StuckInto(Collider2D collision)
	{
		if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
		{
			pierceAmount--;
			return;
		}

		if (isSpinning)
		{
			if (collision.GetComponent<Enemy>() != null)
			{
				StopWhenSpinning();
			}
			return;
		}
		canRotate = false;
		cd.enabled = false; // 关闭碰撞器

		rb.isKinematic = true;
		rb.constraints = RigidbodyConstraints2D.FreezeAll;
		if (isBouncing && enemyTarget.Count > 0) return;
		anim.SetBool("Rotation", false);

		transform.parent = collision.transform;
	}
}
