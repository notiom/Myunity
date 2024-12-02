using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
	// Start is called before the first frame update
	[SerializeField] private PlayerStats playerStats;
	[SerializeField] private Slider slider;

	[SerializeField] private Image dashImage;
	[SerializeField] private Image parryImage;
	[SerializeField] private Image crystalImage;
	[SerializeField] private Image swordImage;
	[SerializeField] private Image blackholeImage;
	[SerializeField] private Image flaskImage;
	[SerializeField] private TextMeshProUGUI healthText;

	[SerializeField] private TextMeshProUGUI dashCooldown;
	[SerializeField] private TextMeshProUGUI parryCooldown;
	[SerializeField] private TextMeshProUGUI crystalCooldown;
	[SerializeField] private TextMeshProUGUI swordCooldown;
	[SerializeField] private TextMeshProUGUI blackholeCooldown;
	[SerializeField] private TextMeshProUGUI flaskCooldown;

	private SkillManager skills;

	private bool flaskCanCooldown;
	private float currentFlaskItemCooldown;

	[Header("Coins info")]
	[SerializeField] private TextMeshProUGUI pricesText;

	void Start()
	{
		gameObject.SetActive(true);
		if (playerStats != null)
		{
			playerStats.onHealthChanged += UpdateHealthUI;

		}
		skills = SkillManager.instance;
	}

	// Update is called once per frame
	void Update()
	{
		pricesText.text = PlayerManager.instance.GetCurrency().ToString("#,#");

		if (Input.GetKeyDown(KeyCode.L) && skills.dash.dashUnlocked)
		{
			SetCooldownOf(dashImage);
		}

		if (Input.GetKeyDown(KeyCode.I) && skills.parry.parryUnlocked)
		{
			SetCooldownOf(parryImage);
		}

		if (Input.GetKeyDown(KeyCode.Space) && skills.crystal.crystalUnlocked)
		{
			SetCooldownOf(crystalImage);
		}

		if (Input.GetKeyDown(KeyCode.O) && skills.sword.swordUnlocked)
		{
			SetCooldownOf(swordImage);
		}

		if (Input.GetKeyDown(KeyCode.R) && skills.blackhole.blackholeUnlocked)
		{
			SetCooldownOf(blackholeImage);
		}

		EquipmentData currentFlask = Inventory.instance.GetEquipment(EquipmentType.Flask);
		if (currentFlask != null)
		{
			flaskCanCooldown = true;
			currentFlaskItemCooldown = currentFlask.itemCooldown;
		}

		if (Input.GetKeyDown(KeyCode.Alpha1) && flaskCanCooldown)
		{
			SetCooldownOf(flaskImage);
		}

		CheckCooldownof(dashImage, skills.dash.cooldown, dashCooldown);
		CheckCooldownof(parryImage, skills.parry.cooldown, parryCooldown);
		CheckCooldownof(crystalImage, skills.crystal.cooldown, crystalCooldown);
		CheckCooldownof(swordImage, skills.sword.cooldown, swordCooldown);
		CheckCooldownof(blackholeImage, skills.blackhole.cooldown, blackholeCooldown);
		if (flaskCanCooldown)
		{
			CheckCooldownof(flaskImage, currentFlaskItemCooldown, flaskCooldown);
			if (flaskImage.fillAmount <= 0) flaskCanCooldown = false;
		}
	}

	private void UpdateHealthUI()
	{
		slider.maxValue = playerStats.GetMaxHealthValue();
		slider.value = playerStats.currentHealth;
		healthText.text = playerStats.currentHealth.ToString();
	}

	private void SetCooldownOf(Image _image)
	{
		if (_image.fillAmount <= 0)
		{
			_image.fillAmount = 1;
		}
	}

	private void CheckCooldownof(Image _image, float _cooldown, TextMeshProUGUI _text)
	{
		if (_image.fillAmount > 0)
		{
			_image.fillAmount -= 1 / _cooldown * Time.deltaTime;
			_text.gameObject.SetActive(true);
			_text.text = System.Math.Round(_cooldown * _image.fillAmount,1).ToString();
		}
		else
		{
			_text.gameObject.SetActive(false);

		}

	}
}
