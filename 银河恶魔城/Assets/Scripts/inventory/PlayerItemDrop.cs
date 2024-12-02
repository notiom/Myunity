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
		List<InventoryItem> itemsToDrop = new List<InventoryItem>(); // ���ڴ洢��Ҫ��������Ʒ
		List<InventoryItem> materialToDrop = new List<InventoryItem> ();

		// ����װ��
		foreach (InventoryItem item in currentEquipment)
		{
			if (Random.Range(0, 100) <= chanceToLooseItems)
			{
				itemsToDrop.Add(item); // �Ȱ���Ʒ��ӵ����������б�
			}
		}

		// ������ɺ���ִ���Ƴ��Ͷ�������
		foreach (InventoryItem item in itemsToDrop)
		{
			DropItem(item.data);
			inventory.UnEquipItem(item.data as EquipmentData);
			inventory.UpdateSlotUI();
		}

		// ������Ʒ
		foreach (InventoryItem item in currentMaterial)
		{
			if (Random.Range(0, 100) <= chanceToLooseMaterial)
			{
				materialToDrop.Add(item); // �Ȱ���Ʒ��ӵ����������б�
			}
		}

		// ������ɺ���ִ���Ƴ��Ͷ�������
		foreach (InventoryItem item in materialToDrop)
		{
			DropItem(item.data);
			inventory.RemoveItem(item.data);
			inventory.UpdateSlotUI();
		}
	}
}
