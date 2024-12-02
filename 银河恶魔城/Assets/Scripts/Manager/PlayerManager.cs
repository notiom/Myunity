using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour,ISaveManager
{
	public static PlayerManager instance; // µ¥ÀýÄ£Ê½
	public Player player;

	[SerializeField] public int currency;
	[SerializeField] public TextMeshProUGUI coins;

	[Header("CheckPoints location")]
	[SerializeField] public Transform[] checkpoints;
	[Header("Checkpoint index")]
	public int rightBound;
	private void Awake()
	{
		if(instance != null)
		{
			Destroy(instance.gameObject);
		}
		else
		{
			instance = this;
		}
		
	}

	public bool HaveEnoughMoney(int _price)
	{
		if(_price > currency)
		{
			return false;
		}
		currency -= _price;
		return true;
	}

	public void LoadData(GameData _data)
	{
		this.currency = _data.currency;
		this.rightBound = _data.rightBound;
	}

	public void SaveData(ref GameData _data)
	{
		_data.currency = this.currency;
		_data.rightBound = this.rightBound;
	}

	public int GetCurrency() => currency;

}
