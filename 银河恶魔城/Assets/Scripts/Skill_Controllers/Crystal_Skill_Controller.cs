using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
	private Animator anim => GetComponent<Animator>();
	private CircleCollider2D cd => GetComponent<CircleCollider2D>();
	private float crystalExistTimer;

	private bool canExplode;
	private bool canMove;
	private float moveSpeed;

	private bool canGrow;
	[SerializeField] private float growSpeed;

	private Transform closeseTarget;
	[SerializeField] private LayerMask whatisEnemy;

	private Player player;

	public void SetupCrystal(float _crystalDuration,bool _canExplode,bool _canMove,float _moveSpeed,Transform _closestTarget)
	{
		crystalExistTimer = _crystalDuration;
		canExplode = _canExplode; 
		canMove = _canMove;
		moveSpeed = _moveSpeed;
		closeseTarget = _closestTarget;
	}

	public void ChooseRandomEnemy()
	{
		// cloneTimer
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25,whatisEnemy);
		if(colliders.Length > 0)
			closeseTarget = colliders[Random.Range(0,colliders.Length)].transform;
	
	}
	private void Update()
	{
		crystalExistTimer -= Time.deltaTime;
		if(crystalExistTimer < 0)
		{
			FinishCrystal();
		}

		if(canMove)
		{
			if (closeseTarget == null) return;
			transform.position = Vector2.MoveTowards(transform.position, closeseTarget.position, moveSpeed * Time.deltaTime);

			if(Vector2.Distance(transform.position, closeseTarget.position) < .5f)
			{
				FinishCrystal();
				canMove = false;
			}
		}

		if(canGrow)
		{
			transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(1.5f, 1.5f), growSpeed * Time.deltaTime);
		}
	}

	private void AnimationExplodeEvent()
	{
		// cloneTimer
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);
		foreach (var hit in colliders)
		{
			if (hit.GetComponent<Enemy>() != null)
			{
				// 有敌人在攻击范围内
				hit.GetComponent<Entity>().SetupKnockbackDir(transform);
				player = PlayerManager.instance.player;
				// 水晶造成魔法伤害
				player.stats.DoMagicalDamage(hit.GetComponent<CharacterStats>());

				EquipmentData equipedAmulet = Inventory.instance.GetEquipment(EquipmentType.Amulet);
				if(equipedAmulet != null)
				{
					equipedAmulet.Effect(hit.transform);
				}
			}
		}
	}

	public void FinishCrystal()
	{
		if (canExplode)
		{
			canGrow = true;
			anim.SetBool("Explode",true);
		}
		else
		{
			SelfDestory();
		}
	}

	public void SelfDestory() => Destroy(gameObject);

	public void SetcCoseseTarget(Transform _transform) => closeseTarget = _transform;
}
