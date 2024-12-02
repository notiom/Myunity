using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_CraftList : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Transform craftSlotParent;
    [SerializeField] private GameObject craftSlotPrefab;

    [SerializeField] private List<EquipmentData> craftEquipment;
	[SerializeField] private GameObject WeaponPage;

	private static bool flag = true;

	private void Start()
	{
		if(flag)
		{
			WeaponPage.GetComponent<UI_CraftList>().SetupCraftList();
			SetupDefaultCraftWindow();
			flag = false;
		}
	}

	public void SetupCraftList()
	{
		for(int i = 0;i < craftSlotParent.childCount;i++) 
		{
			Destroy(craftSlotParent.GetChild(i).gameObject);
		}

		for (int i = 0; i < craftEquipment.Count; i++)
		{
			GameObject newSlot = Instantiate(craftSlotPrefab, craftSlotParent);
			newSlot.GetComponent<UI_CraftSlot>().SetUpCraftSlot(craftEquipment[i]);
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		SetupCraftList();
	}

	public void SetupDefaultCraftWindow()
	{
		if (craftEquipment[0] != null)
		{
			GetComponentInParent<UI>().craftWindow.SetupCraftWindow(WeaponPage.GetComponent<UI_CraftList>().craftEquipment[0]);
		}
	}
}
