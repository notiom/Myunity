using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Image itemIcon;

    [SerializeField] private Button button;
    [SerializeField] private Image[] materialImage;
    [SerializeField] public TextMeshProUGUI solution; // 合成情况

	private void Start()
	{
		solution.gameObject.SetActive(false);
	}
	public void SetupCraftWindow(EquipmentData _data)
    {
        for(int i = 0;i < materialImage.Length;i++) 
        {
            materialImage[i].color = Color.clear;
            materialImage[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
        }

        for(int i = 0;i < _data.craftingMaterials.Count;i++)
        {
            if(_data.craftingMaterials.Count > materialImage.Length)
            {
                Debug.LogWarning("You have more materials amount than you have material slots in craft window");
            }

            materialImage[i].sprite = _data.craftingMaterials[i].data.icon;
            materialImage[i].color = Color.white;
            TextMeshProUGUI materialSlotText = materialImage[i].GetComponentInChildren<TextMeshProUGUI>();


			materialSlotText.text = _data.craftingMaterials[i].stackSize.ToString();
			materialSlotText.color = Color.white;
		}

        itemIcon.sprite = _data.icon;
        itemName.text = _data.itemName;
        itemDescription.text = _data.GetDescription();

		button.onClick.RemoveAllListeners();
		button.onClick.AddListener(() => Inventory.instance.CanCraft(_data, _data.craftingMaterials));
    }

    public IEnumerator delayEraseText(float _seconds)
    {
        yield return new WaitForSeconds(_seconds);
		solution.gameObject.SetActive(false);
	}

}
