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
		// ���������ֵ����10%ʱ�Żἤ��ü���
		// �ռ��ڿ�¡����Χ����ĵ���Ŀ��

		stats = PlayerManager.instance.player.GetComponent<PlayerStats>();
		// �����������˺���ʹ����ֵ����10%����,�ʹ�������˾��
		if (stats.currentHealth - stats.damage.GetValue() > Mathf.RoundToInt(stats.GetMaxHealthValue() * .1f))
		{
			// Ҫ���ü��ܵ���ȴʱ�� 
			Inventory.instance.lastTimeUseArmor = float.NegativeInfinity;
			return;
		}

		stats.startCoroutiueAttack(2f);
		// ����ͣʱ��,�����˺�
		Collider2D[] colliders = Physics2D.OverlapCircleAll(_enemyPosition.position, 2);

		foreach (var hit in colliders)
		{
			hit.GetComponent<Enemy>()?.FreezeTimeFor(duration);
		}

	}


}
