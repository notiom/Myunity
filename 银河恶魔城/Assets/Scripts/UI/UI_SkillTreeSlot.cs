using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillTreeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISaveManager
{
	private UI ui;

	[SerializeField] private string skillName;
	[SerializeField] private int skillPrice;
	[TextArea]
	[SerializeField] private string skillDescription;
	[SerializeField] private Color lockedSkillColor;

	public bool unlocked;
	private Image skillImage;

	[SerializeField] private UI_SkillTreeSlot[] shouldBeUnlocked;
	[SerializeField] private UI_SkillTreeSlot[] shouldBeLocked;

	[SerializeField] private TextMeshProUGUI solution;
	private TextMeshProUGUI pricesText;

	private void OnValidate()
	{
		gameObject.name = "SkillTreeSlot_UI - " + skillName;
	}

	private void Awake()
	{
		skillImage = GetComponent<Image>();
		pricesText = GetComponentInChildren<TextMeshProUGUI>();
	}

	private void Start()
	{
		skillImage.color = lockedSkillColor;
		ui = GetComponentInParent<UI>();
		pricesText.text = "$" + skillPrice.ToString("#,#");
		pricesText.color = Color.white;
		pricesText.fontSize = 25;
		PlayerManager.instance.coins.text = "$" + PlayerManager.instance.GetCurrency().ToString("#,#");
		PlayerManager.instance.coins.color = Color.white;
		// GetComponent<Button>().onClick.RemoveAllListeners();
	}

	public void UnlockSkillSlot()
	{
		// 如果有一个该解锁的没解锁,就不能解锁该技能
		if (unlocked)
		{
			// 如果已经解锁,就返回已解锁
			solution.gameObject.SetActive(true);
			solution.text = "this skill already be unlocked";
			StartCoroutine(delayEraseText(1f));
			return;
		}

		for (int i = 0; i < shouldBeUnlocked.Length; i++)
		{
			if (shouldBeUnlocked[i].unlocked == false)
			{
				solution.gameObject.SetActive(true);
				solution.text = "need to unlock front skill!";
				StartCoroutine(delayEraseText(1f));
				return;
			}
		}

		// 如果有一个不该被解锁的解锁了,不能解锁该技能
		for (int i = 0; i < shouldBeLocked.Length; i++)
		{
			if (shouldBeLocked[i].unlocked == true)
			{
				solution.gameObject.SetActive(true);
				solution.text = "only one upgrad can be choose!";
				StartCoroutine(delayEraseText(1f));
				return;
			}
		}

		if (!PlayerManager.instance.HaveEnoughMoney(skillPrice))
		{
			// 没有足够的钱就返回
			solution.gameObject.SetActive(true);
			solution.text = "No enough money to unlock this skill!";
			StartCoroutine(delayEraseText(1f));
			return;
		}
		unlocked = true;
		skillImage.color = Color.white;
		solution.gameObject.SetActive(true);
		solution.text = "success unlock this skill";
		pricesText.text = null;
		pricesText.color = Color.clear;
		PlayerManager.instance.coins.text = PlayerManager.instance.GetCurrency().ToString("#,#");
		StartCoroutine(delayEraseText(1f));
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		ui.skillToolTip.ShowToolTip(skillDescription, skillName);

		Vector2 mousePosition = Input.mousePosition;

		float xOffset = 0;
		float yOffset = 0;
		if (mousePosition.x > Screen.width / 2)
		{
			xOffset = -Screen.width / 10;
		}
		else
		{
			xOffset = Screen.width / 10;
		}

		if (mousePosition.y > Screen.height / 2)
		{
			yOffset = -Screen.height / 5;
		}
		else
		{
			yOffset = Screen.height / 5;
		}

		ui.skillToolTip.transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y + yOffset);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		ui.skillToolTip.HideToolTip();
	}

	private IEnumerator delayEraseText(float _seconds)
	{
		yield return new WaitForSeconds(_seconds);
		solution.gameObject.SetActive(false);
	}

	public void LoadData(GameData _data)
	{
		if (_data.skillTree.TryGetValue(skillName, out bool value))
		{
			unlocked = value;
			if (unlocked)
			{
				skillImage.color = Color.white;
				pricesText.text = null;
				pricesText.color = Color.clear;
				PlayerManager.instance.coins.text = PlayerManager.instance.GetCurrency().ToString("#,#");
			}
		}
	}

	public void SaveData(ref GameData _data)
	{
		if (_data.skillTree.TryGetValue(skillName, out bool value))
		{
			_data.skillTree.Remove(skillName);
			_data.skillTree.Add(skillName, unlocked);
		}

		else
		{
			_data.skillTree.Add(skillName, unlocked);
		}
	}

	private IEnumerator DelayChangeColor(float _seconds)
	{
		yield return new WaitForSeconds(_seconds);
	}
}
