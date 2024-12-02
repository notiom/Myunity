using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Freeze enemies effect", menuName = "Data/Item effect/Freeze enemies")]
public class FreezeEnemiesEffect : ItemEffect
{
    [SerializeField] private float duration;
	private PlayerStats stats;

	public override void ExecuteEffect(Transform _enemyPosition)
	{
		// 当玩家生命值低于10%时才会激活该技能
		// 收集在克隆体周围最近的敌人目标

		stats = PlayerManager.instance.player.GetComponent<PlayerStats>();
		// 如果吃了这个伤害会使生命值降到10%以下,就触发名刀司命
		if (stats.currentHealth - stats.damage.GetValue() > Mathf.RoundToInt(stats.GetMaxHealthValue() * .1f))
		{
			// 要重置技能的冷却时间 
			Inventory.instance.lastTimeUseArmor = float.NegativeInfinity;
			return;
		}

		stats.startCoroutiueAttack(2f);
		// 先暂停时间,不吃伤害
		Collider2D[] colliders = Physics2D.OverlapCircleAll(_enemyPosition.position, 2);

		foreach (var hit in colliders)
		{
			hit.GetComponent<Enemy>()?.FreezeTimeFor(duration);
		}

	}


}
