using System.Collections;
using TMPro;
using UnityEngine;

public class UI_ItemToolTip : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI itemNameText;
	[SerializeField] private TextMeshProUGUI itemTypeText;
	[SerializeField] private TextMeshProUGUI itemDescription;

	private void Start()
	{

	}

	public void ShowToolTip(EquipmentData item)
	{
		if (item == null)
		{
			return; 
		}
		itemNameText.text = item.itemName;
		itemTypeText.text = item.equipmentType.ToString();
		itemDescription.text = item.GetDescription();

		if (itemNameText.text.Length > 15)
		{
			itemNameText.fontSize = itemNameText.fontSize * .7f;
		}

		gameObject.SetActive(true);
	}

	public void HideToolTip()
	{
		gameObject.SetActive(false);
	}


}
