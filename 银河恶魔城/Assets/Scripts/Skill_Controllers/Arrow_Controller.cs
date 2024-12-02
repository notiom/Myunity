using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_Controller : MonoBehaviour
{
	[SerializeField] private string targetLayerName = "Player";

	[SerializeField] private float xVelocity;
	[SerializeField] private Rigidbody2D rb;

	[SerializeField] private bool canMove = true;
	[SerializeField] private bool flipped;

	private CharacterStats myStats;

	public void SetUpArrow(float _speed,CharacterStats _myStats)
	{
		xVelocity = _speed;
		myStats = _myStats;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayerName))
		{
			myStats.DoDamage(collision.GetComponent<CharacterStats>());
			StuckInto(collision);
		}
		else if(collision.gameObject.layer == LayerMask.NameToLayer("Map"))
		{
			StuckInto(collision);
		}

	}

	private void StuckInto(Collider2D collision)
	{
		// 禁用物理引擎
		GetComponentInChildren<ParticleSystem>().gameObject.SetActive(false);
		GetComponent<CapsuleCollider2D>().enabled = false;
		rb.isKinematic = true;
		canMove = false;
		rb.constraints = RigidbodyConstraints2D.FreezeAll;
		transform.parent = collision.transform;

		Destroy(gameObject,5);
	}

	private void Update()
	{
		if(canMove) rb.velocity = new Vector2(xVelocity, rb.velocity.y);
	}

	public void FlipArrow()
	{
		if (flipped) return;
		xVelocity *= -1;
		flipped = true;
		transform.Rotate(0, 180, 0);
		targetLayerName = "Enemy";

	}
}
