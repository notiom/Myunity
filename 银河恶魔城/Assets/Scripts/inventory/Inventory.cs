using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Inventory : MonoBehaviour, ISaveManager
{
	public static Inventory instance;

	public List<ItemData> startEquipment;
	public List<InventoryItem> equipment;
	public Dictionary<EquipmentData, InventoryItem> equipmentDictionary;

	public List<InventoryItem> inventoryItems; //��������ϵ�����͸���
	public List<InventoryItem> stashItems;

	public Dictionary<ItemData, InventoryItem> inventoryDictionary; // ���ҵ�����Ʒ�Ƿ���� ��:��Ʒ ֵ:��Ʒ���ͺʹ�С
	public Dictionary<ItemData, InventoryItem> stashDictionary;

	[Header("Inventory UI")]
	[SerializeField] private Transform inventorySlotParent;
	[SerializeField] private Transform stashSlotParent;
	[SerializeField] private Transform equipmentSlotparent;
	[SerializeField] private Transform statslotParent;

	private UI_StatSlot[] statSlot;

	private UI_ItemSlot[] inventoryitemSlot;
	private UI_ItemSlot[] stashItemSlot;
	private UI_EquipmentSlot[] equipmentSlot;


	[Header("Items cooldown")]
	private float lastTimeUseFlask = float.NegativeInfinity;
	public float lastTimeUseArmor = float.NegativeInfinity;

	public UI_CraftWindow craftWindow;

	[Header("Data base")]
	[SerializeField] private List<ItemData> itemDataBase;
	public List<InventoryItem> loadedItems;
	public List<EquipmentData> loadedEquipments;
	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void Start()
	{
		inventoryItems = new List<InventoryItem>();
		inventoryDictionary = new Dictionary<ItemData, InventoryItem>();
		inventoryitemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();

		stashItems = new List<InventoryItem>();
		stashDictionary = new Dictionary<ItemData, InventoryItem>();
		stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();

		equipment = new List<InventoryItem>();
		equipmentDictionary = new Dictionary<EquipmentData, InventoryItem>();

		equipmentSlot = equipmentSlotparent.GetComponentsInChildren<UI_EquipmentSlot>();

		statSlot = statslotParent.GetComponentsInChildren<UI_StatSlot>();
		AddStartingItems();
	}

	private void AddStartingItems()
	{
		if (loadedEquipments.Count > 0)
		{
			foreach (EquipmentData item in loadedEquipments)
			{
				EquipItem(item);
			}
		}

		if (loadedItems.Count > 0)
		{
			foreach (InventoryItem item in loadedItems)
			{
				for (int i = 0; i < item.stackSize; i++)
				{
					AddItem(item.data);
				}
			}
			return;
		}
		for (int i = 0; i < startEquipment.Count; i++)
		{
			if (startEquipment[i] != null)
				AddItem(startEquipment[i]);
		}
	}

	public void EquipItem(ItemData _item)
	{
		// ����ת����
		EquipmentData newEquipment = _item as EquipmentData;
		// �Ѿ���װ������Ʒ
		InventoryItem newItem = new InventoryItem(newEquipment);
		EquipmentData oldEquipment = null;

		foreach (KeyValuePair<EquipmentData, InventoryItem> item in equipmentDictionary)
		{
			if (item.Key.equipmentType == newEquipment.equipmentType)
			{
				// �ڱ���ʱ����ɾ��Ԫ��
				oldEquipment = item.Key;
			}
		}
		// ɾ���б���ֵ��еĸ�ֵ
		if (oldEquipment != null)
		{

			UnEquipItem(oldEquipment);
			AddItem(oldEquipment);
		}

		equipment.Add(newItem);
		equipmentDictionary.Add(newEquipment, newItem);
		newEquipment.AddModifiers();
		RemoveItem(_item);
		UpdateSlotUI();
	}

	public void UnEquipItem(EquipmentData itemToDelete)
	{
		// �ų�ĳ��װ������û��װװ�������
		// if (equipmentDictionary[itemToDelete] == null) return;
		if (equipmentDictionary.TryGetValue(itemToDelete, out InventoryItem value))
		{
			equipment.Remove(value);
			equipmentDictionary.Remove(itemToDelete);
			itemToDelete.RemoveModifiers();
		}
	}

	public void UpdateSlotUI()
	{
		// ����װ����
		for (int i = 0; i < equipmentSlot.Length; i++)
		{
			equipmentSlot[i].CleanUpSlot();
		}

		for (int i = 0; i < equipmentSlot.Length; i++)
		{
			foreach (KeyValuePair<EquipmentData, InventoryItem> item in equipmentDictionary)
			{
				if (item.Key.equipmentType == equipmentSlot[i].slotType)
				{
					equipmentSlot[i].UpdateSlot(item.Value);
				}
			}
		}
		// ��ȫ������
		for (int i = 0; i < inventoryitemSlot.Length; i++)
		{
			inventoryitemSlot[i].CleanUpSlot();
		}

		for (int i = 0; i < stashItemSlot.Length; i++)
		{
			stashItemSlot[i].CleanUpSlot();
		}
		// �����»���
		for (int i = 0; i < inventoryItems.Count; i++)
		{
			inventoryitemSlot[i].UpdateSlot(inventoryItems[i]);
		}

		for (int i = 0; i < stashItems.Count; i++)
		{
			stashItemSlot[i].UpdateSlot(stashItems[i]);
		}

		// ����UI����
		for (int i = 0; i < statSlot.Length; i++)
		{
			statSlot[i].UpdateStartVAlueUI();
		}

	}

	public void AddItem(ItemData _item)
	{
		if (_item.itemType == DataType.Equipment && CanAddItem())
		{
			AddToInventory(_item);
		}

		else if (_item.itemType == DataType.Material)
		{
			AddToStash(_item);
		}
		UpdateSlotUI();
	}

	private void AddToStash(ItemData _item)
	{
		if (stashDictionary.TryGetValue(_item, out InventoryItem value))
		{
			value.AddStack();
		}
		else
		{
			InventoryItem newItem = new InventoryItem(_item);
			stashItems.Add(newItem);
			stashDictionary.Add(_item, newItem);
		}
	}

	private void AddToInventory(ItemData _item)
	{

		if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
		{
			value.AddStack();
		}
		else
		{
			InventoryItem newItem = new InventoryItem(_item);
			inventoryItems.Add(newItem);
			inventoryDictionary.Add(_item, newItem);
		}
	}

	public void RemoveItem(ItemData _item)
	{
		if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
		{
			value.RemoveStack();
			if (value.stackSize <= 0)
			{
				inventoryItems.Remove(value);
				inventoryDictionary.Remove(_item);
			}
		}

		if (stashDictionary.TryGetValue(_item, out InventoryItem stashvalue))
		{
			stashvalue.RemoveStack();
			if (stashvalue.stackSize <= 0)
			{
				stashItems.Remove(stashvalue);
				stashDictionary.Remove(_item);
			}
		}
		UpdateSlotUI();
	}
	public List<InventoryItem> GetEquipmentList() => equipment;

	public List<InventoryItem> GetMaterialList() => stashItems;

	public EquipmentData GetEquipment(EquipmentType _type)
	{
		foreach (KeyValuePair<EquipmentData, InventoryItem> item in equipmentDictionary)
		{
			if (item.Key.equipmentType == _type)
			{
				// until find only tpye to return 
				return item.Key;
			}
		}
		return null;
	}

	public void UseFlask()
	{
		EquipmentData currentFlask = GetEquipment(EquipmentType.Flask);

		if (currentFlask == null) return;

		bool canUseFlak = Time.time > lastTimeUseFlask + currentFlask.itemCooldown;

		if (canUseFlak)
		{
			lastTimeUseFlask = Time.time;
			currentFlask.Effect(null);
		}
		else
		{
			Debug.Log("Flask on cooldown");
		}
	}

	public bool CanUseArmor()
	{
		EquipmentData currentArmor = GetEquipment(EquipmentType.Armor);

		if (currentArmor == null) return false; // ûװ������ʹ��
		if (Time.time > lastTimeUseArmor + currentArmor.itemCooldown)
		{
			lastTimeUseArmor = Time.time;
			return true;
		}
		Debug.Log("Armor on cooldown");
		return false;
	}

	public bool CanAddItem()
	{
		if (inventoryItems.Count >= inventoryitemSlot.Length)
		{
			Debug.Log("No Space!");
			return false;
		}
		return true;
	}

	public bool CanCraft(EquipmentData craftData, List<InventoryItem> craftingMaterials)
	{
		foreach (var craftmaterial in craftingMaterials)
		{
			if (stashDictionary.TryGetValue(craftmaterial.data, out InventoryItem stashvalue))
			{
				// �����Ҫ�Ĳ��϶���ϣ����,�Ϳ���
				if (craftmaterial.stackSize > stashvalue.stackSize)
				{
					// ������ֵ���û���ҵ�ҲҪ����false
					craftWindow.solution.gameObject.SetActive(true);
					craftWindow.solution.text = "No enough materials to carft!";
					craftWindow.StartCoroutine(craftWindow.delayEraseText(1f));
					return false;
				}
			}
			else
			{
				// ������ֵ���û���ҵ�ҲҪ����false
				craftWindow.solution.gameObject.SetActive(true);
				craftWindow.solution.text = "No enough materials to carft!";
				craftWindow.StartCoroutine(craftWindow.delayEraseText(1f));
				return false;
			}
		}

		// ִ�е�����˵�����Ժϳ�
		// ȥɾ����Ҫ��Ԫ��
		foreach (var craftmaterial in craftingMaterials)
		{
			for (int i = 0; i < craftmaterial.stackSize; i++)
			{
				RemoveItem(craftmaterial.data);
			}
		}

		// ��inventory�м������ɵ�Ԫ��
		AddItem(craftData);
		craftWindow.solution.gameObject.SetActive(true);
		craftWindow.solution.text = "Craft Successful!";
		craftWindow.StartCoroutine(craftWindow.delayEraseText(1f));
		return true;
	}

	public void LoadData(GameData _data)
	{
		foreach (KeyValuePair<string, int> pair in _data.inventory)
		{
			foreach (var item in itemDataBase)
			{
				if (item != null && item.itemId == pair.Key)
				{
					InventoryItem itemToLoad = new InventoryItem(item);
					itemToLoad.stackSize = pair.Value;

					loadedItems.Add(itemToLoad);
				}
			}
		}

		foreach (string loadedItemId in _data.equipmentId)
		{
			foreach (var item in itemDataBase)
			{
				if (item != null && item.itemId == loadedItemId)
				{
					loadedEquipments.Add(item as EquipmentData);
				}
			}
		}
	}

	public void SaveData(ref GameData _data)
	{
		_data.inventory.Clear();
		_data.equipmentId.Clear();

		foreach (KeyValuePair<ItemData, InventoryItem> pair in inventoryDictionary)
		{
			_data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
		}

		foreach (KeyValuePair<ItemData, InventoryItem> pair in stashDictionary)
		{
			_data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
		}

		foreach (KeyValuePair<EquipmentData, InventoryItem> pair in equipmentDictionary)
		{
			// �������Ѿ�װ�����ϵ��豸
			_data.equipmentId.Add(pair.Key.itemId);
		}
	}
#if UNITY_EDITOR
	[ContextMenu("Fill up item data base")]
	private void FillupItemDataBase() => itemDataBase = new List<ItemData>(GetItemDataBase());

	private List<ItemData> GetItemDataBase()
	{
		itemDataBase = new List<ItemData>();
		string[] assetNames = AssetDatabase.FindAssets("", new[] { "Assets/Data/Items" });

		foreach (string SOName in assetNames)
		{
			var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
			var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOpath);
			itemDataBase.Add(itemData);
		}

		return itemDataBase;
	}
#endif
}


