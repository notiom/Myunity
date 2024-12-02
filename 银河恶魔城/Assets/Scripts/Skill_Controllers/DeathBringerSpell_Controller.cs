using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerSpell_Controller : MonoBehaviour
{
	[SerializeField] private Transform check;
	[SerializeField] Vector2 boxSize;
	[SerializeField] private LayerMask whatIsPlayer;

	private CharacterStats myStats;

	public void SetupSpell(CharacterStats _stats) => myStats = _stats;

	private void AnimationTrigger()
	{
		// cloneTimer
		Collider2D[] colliders = Physics2D.OverlapBoxAll(check.position, boxSize,whatIsPlayer);
		foreach (var hit in colliders)
		{
			if (hit.GetComponent<Player>() != null)
			{
				hit.GetComponent<Entity>().SetupKnockbackDir(transform);
				CharacterStats stats = hit.GetComponent<CharacterStats>();
				myStats.lightingDamage.AddModifier(10);
				myStats.DoDamage(hit.GetComponent<CharacterStats>());
				myStats.lightingDamage.RemoveModifier(10);
			}
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(check.position, boxSize);
	}

	private void SelfDestroy() => Destroy(gameObject);
}
