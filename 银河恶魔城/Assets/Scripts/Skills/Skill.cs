using System.Collections;
using UnityEngine;

public class Skill : MonoBehaviour
{
	[Header("cooldown info")]
	public float cooldown;
	public float cooldownTimer;
	protected Player player;

	protected virtual void Start()
	{
		player = PlayerManager.instance.player;
		StartCoroutine(StartCheckUnlock());
	}

	protected virtual void Update()
	{
		cooldownTimer -= Time.deltaTime;
	}

	public virtual bool CanUseSkill()
	{
		// Use Skill
		if (cooldownTimer < 0)
		{
			UseSkill();
			cooldownTimer = cooldown;
			return true;
		}

		player.fx.CreatePopUpText("Skill is on cooldown");
		// Debug.Log("Skill is on cooldown");
		return false;
	}

	protected virtual void CheckUnlock()
	{

	}

	public virtual void UseSkill()
	{
		// do some skill spesific things


	}

	protected virtual Transform FindClosestEnemy(Transform _checkTransform)
	{
		// 收集在克隆体周围最近的敌人目标
		Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkTransform.position, 25);
		float closeDistance = Mathf.Infinity;
		Transform closestEnemy = null;
		foreach (var hit in colliders)
		{
			if (hit.GetComponent<Enemy>() != null)
			{
				float distanceToEnemy = Vector2.Distance(_checkTransform.position, hit.transform.position);

				if (distanceToEnemy < closeDistance)
				{
					closeDistance = distanceToEnemy;
					closestEnemy = hit.transform;
				}
			}
		}
		return closestEnemy;
	}

	private IEnumerator StartCheckUnlock()
	{
		yield return new WaitForSeconds(.1f);
		CheckUnlock();
	}
}
