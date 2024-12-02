using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightDeadState : EnemyState
{
	private Enemy_Night enemy;

	public bool isExplosion;

	public NightDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Night _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
	{
		this.enemy = _enemy;
	}

	public override void AnimationFinishTrigger()
	{
		base.AnimationFinishTrigger();
	}

	public override void Enter()
	{
		base.Enter();
		// enemy.anim.SetBool(enemy.lastAnimBoolName, true);
		// enemy.anim.speed = 0;
		// enemy.cd.enabled = false;
		rb.constraints = RigidbodyConstraints2D.FreezePosition;

	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void Update()
	{
		base.Update();
		rb.velocity = new Vector2(0, 0);
		if (isExplosion)
		{
			// 仅能爆炸伤害一次
			isExplosion = false;
			enemy.cd.enabled = false;
			Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.transform.position, enemy.attackCheckRadius);
			foreach (var hit in colliders)
			{
				if (hit.GetComponent<Player>() != null)
				{
					// 有敌人在攻击范围内
					PlayerStats target = hit.GetComponent<PlayerStats>();
					enemy.stats.DoDamage(target);
				}
			}
		}
	}
}
