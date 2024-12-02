using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
	[SerializeField] private ItemData itemData;
	[SerializeField] private Rigidbody2D rb;
	[SerializeField] private BoxCollider2D cd;
	[SerializeField] private BoxCollider2D cdChlidren;
	private Vector2 velocity;

	private void SetupVisuals()
	{
		if (itemData == null) return;
		GetComponent<SpriteRenderer>().sprite = itemData.icon;
		gameObject.name = "Item object - " + itemData.itemName;
	}

	public void SetupItem(ItemData _itemData,Vector2 _velocity)
	{
		itemData = _itemData;
		rb.velocity = _velocity;

		SetupVisuals();
	}

	public void PickupItem()
	{
		if (!Inventory.instance.CanAddItem() && itemData.itemType == DataType.Equipment)
		{
			// 位移并且关闭物体的碰撞器
			// 10s之后自动销毁未捡起的物体
			cd.enabled = false; // 关闭碰撞器
			cdChlidren.enabled = false;

			rb.constraints = RigidbodyConstraints2D.FreezePositionX;
			rb.constraints = RigidbodyConstraints2D.FreezePositionY;
			PlayerManager.instance.player.fx.CreatePopUpText("Inventory is full!");
			StartCoroutine(DelayDestoryMe(10));
			return; 
		}
		Inventory.instance.AddItem(itemData);
		Destroy(gameObject);
	}

	private IEnumerator DelayDestoryMe(float _seconds)
	{
		yield return new WaitForSeconds(_seconds);
		Destroy(gameObject);
	}

}
