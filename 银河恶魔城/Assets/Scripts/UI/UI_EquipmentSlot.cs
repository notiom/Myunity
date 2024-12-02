using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipmentSlot : UI_ItemSlot
{
	public EquipmentType slotType;

	private void OnValidate()
	{
		gameObject.name = "Equipment slot -" + slotType.ToString();

	}

	public override void OnPointerDown(PointerEventData eventData)
	{
		// inventory unequip item
		if (item == null || item.data == null) return;

		if(eventData.button == PointerEventData.InputButton.Left)
		{
			Inventory.instance.UnEquipItem(item.data as EquipmentData);
			Inventory.instance.AddItem(item.data as EquipmentData);
			ui.itemToolTip.HideToolTip();
			CleanUpSlot();
		}

	}
}
