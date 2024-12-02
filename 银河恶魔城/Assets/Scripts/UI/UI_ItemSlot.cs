using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ItemSlot : MonoBehaviour , IPointerDownHandler,IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] protected Image itemImage;
	[SerializeField] protected TextMeshProUGUI itemText;
	[SerializeField] private UI_SellItem sellItem;

	protected UI ui;

	public InventoryItem item;

	protected virtual void Start()
	{
		// sellItem = GetComponentInChildren<UI_SellItem>();
		ui = GetComponentInParent<UI>();
	}

	public void UpdateSlot(InventoryItem _newItem)
	{
		item = _newItem;
		itemImage.color = Color.white;
		if(item != null) 
		{
			itemImage.sprite = item.data.icon;

			if(item.stackSize > 1)
			{
				itemText.text = item.stackSize.ToString();
			}
			else
			{
				itemText.text = "";
			}
		}
	}

	public void CleanUpSlot()
	{
		item = null;
		itemImage.sprite = null;
		itemImage.color = Color.clear;
		itemText.text = "";
	}
	public virtual void OnPointerDown(PointerEventData eventData)
	{
		if (item == null || item.data == null) return;

		if(Input.GetKey(KeyCode.LeftControl))
		{
			// 售卖应该加钱
			PlayerManager.instance.currency += item.data.sellPrices;
			PlayerManager.instance.coins.text = PlayerManager.instance.GetCurrency().ToString("#,#");
			Inventory.instance.RemoveItem(item.data);
			return;
		}

		if (eventData.button == PointerEventData.InputButton.Left)
		{
			if (item.data.itemType == DataType.Equipment)
			{
				Inventory.instance.EquipItem(item.data);
			}
		}

		if(eventData.button == PointerEventData.InputButton.Right)
		{
			sellItem.ShowConfirmPanel(item.data);
		}
		ui.itemToolTip.HideToolTip();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (item == null || item.data == null)
			return;

		ui.itemToolTip.ShowToolTip(item.data as EquipmentData);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		
		if (item == null || item.data == null)
			return;
		ui.itemToolTip.HideToolTip();
		
	}
}
