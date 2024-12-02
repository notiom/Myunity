using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int possibleItemDrop;
    [SerializeField] private ItemData[] possibleDrop;
    private List<ItemData> dropList;
    [SerializeField] private GameObject dropPrefab;
    [SerializeField] private ItemData item;
    private EnemyStats enemyStats;

	private void Start()
	{
		enemyStats = GetComponent<EnemyStats>();
        dropList = new List<ItemData>();
	}
	public virtual void GenerateDrop()
    {
        for(int i = 0;i < possibleDrop.Length;i++) 
        {
			if (enemyStats.dropDictionary.TryGetValue(possibleDrop[i],out float value))
            {
                if(Random.Range(0, 100) <= value)
                {
					dropList.Add(possibleDrop[i]);

				}
            }

        }

        // 掉落几个物品
        for(int i = 0;i < possibleItemDrop;i++)
        {
            if (dropList.Count == 0) return;
            ItemData RandomItem = dropList[Random.Range(0,dropList.Count - 1)];
            dropList.Remove(RandomItem);
            DropItem(RandomItem);
        }

        
    }
    public void DropItem(ItemData _itemData)
    {
        GameObject newDrop = Instantiate(dropPrefab,transform.position,Quaternion.identity);

        Vector2 randomVelocity = new Vector2(Random.Range(-3, 3), Random.Range(7, 10));

        newDrop.GetComponent<ItemObject>().SetupItem(_itemData,randomVelocity);
    }
}
