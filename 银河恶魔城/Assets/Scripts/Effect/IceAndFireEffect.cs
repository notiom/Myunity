using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ice and Fire effect", menuName = "Data/Item effect/Ice and Fire")]
public class IceAndFireEffect : ItemEffect
{
	[SerializeField] private GameObject iceAndFirePrefab;
	[SerializeField] private float xVelocity;
	private PlayerStats stats;
	[SerializeField] private int buffAmount;
	[SerializeField] private int buffDuration;
	private StatType buffType; // 随机选择加冰还是火buff
	public override void ExecuteEffect(Transform respawnPosition)
	{
		Player player = PlayerManager.instance.player;
		stats = PlayerManager.instance.player.GetComponent<PlayerStats>();

		bool thirdAttack = player.primaryAttack.comboCounter == 2;

		if (thirdAttack) 
		{
			buffType = Random.Range(0, 2) == 0 ? StatType.fireDamage : StatType.iceDamage;
			// 在新一次buff来临时,上一次buff应该结束
			stats.IncreaseStatBy(buffAmount, buffDuration, stats.StatToModify(buffType));
			GameObject newIceAndFire = Instantiate(iceAndFirePrefab, respawnPosition.position, player.transform.rotation);
			newIceAndFire.GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity * (int)player.facingDir,0);

			Destroy(newIceAndFire,10);
		}
	}
}