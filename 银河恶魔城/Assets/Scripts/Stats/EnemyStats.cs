using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyStats : CharacterStats
{
	private Enemy enemy;
	private Player player;
	private ItemDrop myDropSystem;

	[Header("drop details")]
	public Dictionary<ItemData,float> dropDictionary;
	[SerializeField] private List<ItemData> dropItem;
	[Range(0f,100f)]
	[SerializeField] private List<float> dropChance;
	[Header("Level details")]
	[SerializeField] private int level;

	[Range(0f, 1f)]
	[SerializeField] private float percentageModifier = .4f;

	public Stats soulDropAmount;

	protected override void Start()
	{
		soulDropAmount.SetDefaultValue(100);
		dropDictionary = new Dictionary<ItemData, float>();
		ApplyDropDictionary();
		ApplyLevelModifiers();
		base.Start();
		enemy = GetComponent<Enemy>();
		player = PlayerManager.instance.player;
		myDropSystem = GetComponent<ItemDrop>();
	}

	private void ApplyDropDictionary()
	{
		for(int i = 0;i < dropItem.Count;i++)
		{
			if(i >= dropChance.Count)
			{
				// ��Щ�������û�б�����ֵ
				dropDictionary[dropItem[i]] = 0;
				continue;
			}
			dropDictionary[dropItem[i]] = dropChance[i];
		}
	}

	private void ApplyLevelModifiers()
	{
		// ���ӵ��˵�����
		Modify(strength);
		Modify(agility);
		Modify(intelligence);
		Modify(vitality);

		Modify(damage);
		Modify(critChance);
		Modify(critPower);

		Modify(maxHealth);
		Modify(armor);
		Modify(evasion);
		Modify(magicResistance);

		Modify(fireDamage);
		Modify(iceDamage);
		Modify(lightingDamage);
	}

	public override void DoDamage(CharacterStats _targetStats)
	{
		// ���˵Ĺ�����Ҫ������������ħ��
		base.DoDamage(_targetStats);
		DoMagicalDamage(_targetStats); // �����ͨ����������ħ���˺����Ƴ�

	}

	private void Modify(Stats _stat)
	{
		// �����˵ȼ�����1ʱ,��������
		for(int i = 1;i < level;i++)
		{
			float modifier = _stat.GetValue() * percentageModifier;

			_stat.AddModifier(Mathf.RoundToInt(modifier));
		}
	}

	public override void Die()
	{
		base.Die();
		enemy.Die();
		// ɱ�����˿��Լ�Ǯ
		PlayerManager.instance.currency += soulDropAmount.GetValue();

		myDropSystem.GenerateDrop();
	}
	/*

	public override void ShockTarget(GameObject _shockStrikePerfab, int _shockDamage)
	{
		base.ShockTarget(_shockStrikePerfab, _shockDamage);

		GameObject newShockStrike = Instantiate(_shockStrikePerfab, player.transform.position, Quaternion.identity);
		newShockStrike.GetComponent<ThunderStrike_Controller>().Setup(_shockDamage, player.GetComponent<CharacterStats>());
	}
	*/
	protected override void ShockTarget(GameObject _shockStrikePerfab, int _shockDamage)
	{
		base.ShockTarget(_shockStrikePerfab, _shockDamage);
		// �ռ��ڿ�¡����Χ����ĵ���Ŀ��
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);
		float closeDistance = Mathf.Infinity;
		Transform closestEnemy = null;
		foreach (var hit in colliders)
		{
			if (hit.GetComponent<Enemy>() != null)
			{
				float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

				if (distanceToEnemy < closeDistance)
				{
					closeDistance = distanceToEnemy;
					closestEnemy = hit.transform;
				}
			}
		}

		if (closestEnemy != null)
		{
			GameObject newShockStrike = Instantiate(_shockStrikePerfab, transform.position, Quaternion.identity);

			newShockStrike.GetComponent<ShockStrike_Controller>().Setup(_shockDamage, closestEnemy.GetComponent<CharacterStats>());
			StartCoroutine(DestoryShock(.3f, newShockStrike));
		}

	}
	private IEnumerator DestoryShock(float _seconds, GameObject _gameobject)
	{
		yield return new WaitForSeconds(.3f);
		Destroy(_gameobject);
	}


}
