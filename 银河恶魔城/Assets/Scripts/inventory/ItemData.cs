using System.Text;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum DataType
{
	Material,
	Equipment
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
	public DataType itemType;
	public string itemName;
	public Sprite icon;
	public string itemId;
	public int sellPrices;

	protected StringBuilder sb = new StringBuilder();
	public virtual string GetDescription()
	{
		return "";
	}

	private void OnValidate()
	{
#if UNITY_EDITOR
		string path = AssetDatabase.GetAssetPath(this);
		itemId = AssetDatabase.AssetPathToGUID(path);
#endif
	}
}
