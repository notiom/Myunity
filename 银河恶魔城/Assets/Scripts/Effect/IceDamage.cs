using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class IceDamage : MonoBehaviour
{
	private CharacterStats iceStats;

	private void Awake()
	{
		iceStats = GetComponent<CharacterStats>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.GetComponent<Player>() != null)
		{
			PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
			iceStats.DoMagicalDamage(playerStats);
		}
	}
}
