using UnityEngine;
using UnityEngine.UI;

public class UI_SellItem : MonoBehaviour
{
	private ItemData item;  // ���� item ����Ҫ��������Ʒ

	[SerializeField] private GameObject confirmPanel;
	public void ShowConfirmPanel(ItemData _item)
	{
		item = _item;
		// ����ȷ�����
		confirmPanel.SetActive(true);

		// ��Ӽ���������ť
		Button yesButton = confirmPanel.transform.Find("confrim").GetComponent<Button>();
		Button noButton = confirmPanel.transform.Find("cancel").GetComponent<Button>();

		yesButton.onClick.RemoveAllListeners();  // ���֮ǰ�ļ�����
		yesButton.onClick.AddListener(ConfirmSell);  // ���ȷ�������¼�

		noButton.onClick.RemoveAllListeners();  // ���֮ǰ�ļ�����
		noButton.onClick.AddListener(CancelSell);  // ���ȡ���¼�
	}

	private void ConfirmSell()
	{
		// ������Ʒ���߼�

		PlayerManager.instance.currency += item.sellPrices;
		PlayerManager.instance.coins.text = PlayerManager.instance.currency.ToString("#,#");
		Inventory.instance.RemoveItem(item);

		// ����ȷ�����
		confirmPanel.SetActive(false);
	}

	private void CancelSell()
	{
		// ����ȷ�����
		// GetComponentInParent<GameObject>().SetActive(true);
		confirmPanel.SetActive(false);
	}
}

