using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
	private SpriteRenderer sr;
	private Animator anim;
	[SerializeField] private float colorLoosingSpeed;
	private float cloneTimer;

	[SerializeField] private Transform attackCheck;
	[SerializeField] private float attackCheckRadius = 0.8f;
	private Transform closestEnemy;
	private bool canDuplicateClone;
	private int facingDir = 1;
	private float chanceToDuplicate;
	private Player player;
	private float attackMultiplier;
	private void Awake()
	{
		sr = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();
		player = PlayerManager.instance.player;
	}
	private void Update()
	{
		cloneTimer -= Time.deltaTime;
		if(cloneTimer < 0)
		{
			sr.color = new Color(1,1,1,sr.color.a - (Time.deltaTime * colorLoosingSpeed));
			if(sr.color.a <= 0)
			{
				Destroy(gameObject);
			}
		}
	}

	public void SetupClone(Transform _newTransform,float _cloneDuration,bool _canAttack, Vector3 _offset,Transform _closestEnemy,bool _canDuplicateClone,float _chanceToDuplicate,float _attackMultiplier)
	{
		if(_canAttack)
		{
			anim.SetInteger("AttackNumber", Random.Range(1, 3));
		}
		attackMultiplier = _attackMultiplier;
		transform.position = _newTransform.position + _offset;
		cloneTimer = _cloneDuration;
		closestEnemy = _closestEnemy;
		canDuplicateClone = _canDuplicateClone;
		chanceToDuplicate = _chanceToDuplicate;

		FaceCloseTarget();
	}

	public void AnimationTrigger()
	{
		cloneTimer = -.1f;
	}

	private void AttackTrigger()
	{
		Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);
		foreach (var hit in colliders)
		{
			if (hit.GetComponent<Enemy>() != null)
			{
				// 有敌人在攻击范围内
				hit.GetComponent<Entity>().SetupKnockbackDir(transform);
				PlayerStats playerStats = player.stats as PlayerStats;
				EnemyStats enemyStats = hit.GetComponent<EnemyStats>();

				playerStats.CloneDoDamge(enemyStats, attackMultiplier);

				// player.stats.DoDamage(hit.GetComponent<CharacterStats>());

				
				if(player.skill.clone.canApplyOnHitEffect)
				{
					Inventory.instance.GetEquipment(EquipmentType.Weapon)?.Effect(hit.transform);
				}
				

				if(canDuplicateClone)
				{
					if(Random.Range(0,100) < chanceToDuplicate)
					{
						SkillManager.instance.clone.CreateClone(hit.transform, new Vector3(0.7f * facingDir, 0));
					}
				}
			}
		}
	}

	private void FaceCloseTarget()
	{

		if(closestEnemy != null) 
		{
			if(transform.position.x > closestEnemy.position.x) 
			{
				// 如果物体在左边,就转向
				facingDir = -facingDir;
				transform.Rotate(0, 180, 0);
			}
		}
	}

}
