using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
	private Player player;

	public bool canAttack = true;
	protected override void Start()
	{
		base.Start();
		player = GetComponent<Player>();
	}

	public override void TakeDamage(int _damage)
	{
		base.TakeDamage(_damage);
	}

	public override void Die()
	{
		base.Die();

		player.Die();

		GetComponent<PlayerItemDrop>()?.GenerateDrop();
	}

	protected override void ShockTarget(GameObject _shockStrikePerfab,int _shockDamage)
	{
		base.ShockTarget(_shockStrikePerfab, _shockDamage);
		GameObject newShockStrike = Instantiate(_shockStrikePerfab, transform.position, Quaternion.identity);
		newShockStrike.GetComponent<ShockStrike_Controller>().Setup(_shockDamage, player.GetComponent<CharacterStats>());
		StartCoroutine(DestoryShock(.3f,newShockStrike));
	}

	protected override void DecreaseHealthBy(int _amount)
	{
		EquipmentData currentArmor = Inventory.instance.GetEquipment(EquipmentType.Armor);
		if (Inventory.instance.CanUseArmor())
			currentArmor.Effect(player.transform);

		if(canAttack)
			base.DecreaseHealthBy(_amount);		
	}

	public void startCoroutiueAttack(float _seconds) => StartCoroutine(AttackFor(_seconds));

	private IEnumerator AttackFor(float _seconds)
	{
		canAttack = false;

		yield return new WaitForSeconds(_seconds);

		canAttack = true;
	}

	public override void OnEvasion()
	{
		base.OnEvasion();
		player.skill.dodge.CreateMirageOnDodge();
	}

	public void CloneDoDamge(CharacterStats _targetStats,float _multiplier)
	{
		// ������ֵ
		if (TargetCanAvoidAttack(_targetStats)) return;
		int totalDamage = damage.GetValue() + strength.GetValue();

		if(_multiplier > 0) 
		{
			totalDamage = Mathf.RoundToInt(totalDamage * _multiplier);
		}
		if (CanCrit())
		{
			totalDamage = CalculateCritDamage(totalDamage);
		}
		totalDamage = CheckTargetArmor(_targetStats, totalDamage);

		// ���ħ������ ħ�������ͼ��ħ������
		// int magicalDamage = DoMagicalDamage(_targetStats);
		// totalDamage += magicalDamage;
		_targetStats.TakeDamage(totalDamage);
		// DoMagicalDamage(_targetStats); // �����ͨ����������ħ���˺����Ƴ�
	}

	private IEnumerator DestoryShock(float _seconds, GameObject _gameobject)
	{
		yield return new WaitForSeconds(.3f);
		Destroy(_gameobject);
	}
}
