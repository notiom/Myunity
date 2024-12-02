using UnityEngine;
using UnityEngine.UI;

public class UI_SellItem : MonoBehaviour
{
	private ItemData item;  // 假设 item 是你要操作的物品

	[SerializeField] private GameObject confirmPanel;
	public void ShowConfirmPanel(ItemData _item)
	{
		item = _item;
		// 激活确认面板
		confirmPanel.SetActive(true);

		// 添加监听器给按钮
		Button yesButton = confirmPanel.transform.Find("confrim").GetComponent<Button>();
		Button noButton = confirmPanel.transform.Find("cancel").GetComponent<Button>();

		yesButton.onClick.RemoveAllListeners();  // 清除之前的监听器
		yesButton.onClick.AddListener(ConfirmSell);  // 添加确认售卖事件

		noButton.onClick.RemoveAllListeners();  // 清除之前的监听器
		noButton.onClick.AddListener(CancelSell);  // 添加取消事件
	}

	private void ConfirmSell()
	{
		// 售卖物品的逻辑

		PlayerManager.instance.currency += item.sellPrices;
		PlayerManager.instance.coins.text = PlayerManager.instance.currency.ToString("#,#");
		Inventory.instance.RemoveItem(item);

		// 隐藏确认面板
		confirmPanel.SetActive(false);
	}

	private void CancelSell()
	{
		// 隐藏确认面板
		// GetComponentInParent<GameObject>().SetActive(true);
		confirmPanel.SetActive(false);
	}
}

