using UnityEngine;

public class IceAndFire_Controller : MonoBehaviour
{
	bool flag = true;
	protected void OnTriggerEnter2D(Collider2D collision)
	{
		// ����ʹһ��Ŀ�겻�ᱻ��ײ���
		if (collision.GetComponent<Enemy>() != null && flag)
		{
			PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
			EnemyStats enemyTarget = collision.GetComponent<EnemyStats>();
			playerStats.DoMagicalDamage(enemyTarget);
			flag = false;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		flag = true;
	}
}
