using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceFall_Controller : MonoBehaviour
{
	private CharacterStats iceStats;

	private void Awake()
	{
		iceStats = GetComponent<CharacterStats>();
		transform.Rotate(180, 0, 0);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.GetComponent<Player>() != null)
		{
			PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
			iceStats.DoMagicalDamage(playerStats);
		}

		// 只要碰撞了就要销毁
		Destroy(gameObject,0.2f);
	}
}
