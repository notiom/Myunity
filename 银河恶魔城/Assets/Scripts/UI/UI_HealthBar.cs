using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
	private Entity entity => GetComponentInParent<Entity>();
	private CharacterStats myStats => GetComponentInParent<CharacterStats>();
	private RectTransform myTransform;
	private Slider slider;
	[SerializeField] private TextMeshPro healthValue;

	private void Awake()
	{
		myTransform = GetComponent<RectTransform>();
	}
	private void Start()
	{

		slider = GetComponentInChildren<Slider>();

		UpdateHealthUI();
	}

	private void UpdateHealthUI()
	{
		slider.maxValue = myStats.GetMaxHealthValue();
		slider.value = myStats.currentHealth;
		healthValue.text = myStats.currentHealth.ToString();
	}

	private void FlipUI() => myTransform.Rotate(0, 180, 0);

	private void OnEnable()
	{
		entity.onFlipped += FlipUI;
		myStats.onHealthChanged += UpdateHealthUI;
	}

	private void OnDisable()
	{
		if(entity != null)  entity.onFlipped -= FlipUI;

		if(myStats != null) myStats.onHealthChanged -= UpdateHealthUI;
	}
}
