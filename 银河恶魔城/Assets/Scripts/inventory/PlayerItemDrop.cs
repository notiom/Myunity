using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
	[Header("Player drop")]
	[SerializeField] private float chanceToLooseItems;
	[SerializeField] private float chanceToLooseMaterial;

	public override void GenerateDrop()
	{

		Inventory inventory = Inventory.instance;
		List<InventoryItem> currentEquipment = inventory.GetEquipmentList();
		List<InventoryItem> currentMaterial = inventory.GetMaterialList();
		List<InventoryItem> itemsToDrop = new List<InventoryItem>(); // 用于存储需要丢弃的物品
		List<InventoryItem> materialToDrop = new List<InventoryItem> ();

		// 丢弃装备
		foreach (InventoryItem item in currentEquipment)
		{
			if (Random.Range(0, 100) <= chanceToLooseItems)
			{
				itemsToDrop.Add(item); // 先把物品添加到待丢弃的列表
			}
		}

		// 遍历完成后，再执行移除和丢弃操作
		foreach (InventoryItem item in itemsToDrop)
		{
			DropItem(item.data);
			inventory.UnEquipItem(item.data as EquipmentData);
			inventory.UpdateSlotUI();
		}

		// 丢弃物品
		foreach (InventoryItem item in currentMaterial)
		{
			if (Random.Range(0, 100) <= chanceToLooseMaterial)
			{
				materialToDrop.Add(item); // 先把物品添加到待丢弃的列表
			}
		}

		// 遍历完成后，再执行移除和丢弃操作
		foreach (InventoryItem item in materialToDrop)
		{
			DropItem(item.data);
			inventory.RemoveItem(item.data);
			inventory.UpdateSlotUI();
		}
	}
}
