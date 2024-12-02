using Microsoft.Win32.SafeHandles;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;
public enum StatType
{
	strength, // 可以增加一点伤害
	agility,  // 可以增加闪避和关键打击能力
	intelligence, // 增加魔法伤害和魔抗 1点魔法伤害和3点魔抗
	vitality,  // 增加生命值

	damage, //基础伤害
	critChance, //暴击几率
	critPower,  // 默认值是150% 暴击伤害

	health, //hp
	armor, // 护甲
	magicResistance, //魔抗
	evasion, //闪避值

	fireDamage, // 火伤
	iceDamage, // 冰伤
	lightingDamage, //光伤
}

public class CharacterStats : MonoBehaviour
{
	private EntityFX fx;
	[Header("Major stats")]
	public Stats strength; // 可以增加一点伤害
	public Stats agility;  // 可以增加闪避和关键打击能力
	public Stats intelligence; // 增加魔法伤害和魔抗 1点魔法伤害和3点魔抗
	public Stats vitality;  // 增加生命值

	[Header("Offensive stats")]
	public Stats damage; //基础伤害
	public Stats critChance; //暴击几率
	public Stats critPower;  // 默认值是150% 暴击伤害

	[Header("Defencive stats")]
	public Stats maxHealth; //hp
	public Stats armor; // 护甲
	public Stats magicResistance; //魔抗
	public Stats evasion; //闪避值

	[Header("Magic stats")]
	public Stats fireDamage; // 火伤
	public Stats iceDamage; // 冰伤
	public Stats lightingDamage; //光伤

	public bool isIgnited; // 随着时间的推移逐渐扣血,点燃
	public bool isChilled; // 减少护甲 削甲
	public bool isShocked; // 减少命中率 失明

	private float ignitedTimer;
	private float chilledTimer;
	private float shockedTimer;

	[SerializeField] private float ailmentsDuration = 4;
	private float igniteDamageCooldown = .3f;
	private float igniteDamageTimer;
	private int igniteDamage;
	[SerializeField] private GameObject shockStrikePerfab;
	private int shockDamage;

	public int currentHealth;

	public System.Action onHealthChanged;

	public bool isDead { get; private set; }
	protected bool isVulnerable;
	protected bool isInvincible;
	protected virtual void Start()
	{
		critPower.SetDefaultValue(150);
		currentHealth = GetMaxHealthValue();

		// damage.AddModifier(4);
		fx = GetComponent<EntityFX>();
	}

	protected virtual void Update()
	{
		ignitedTimer -= Time.deltaTime;
		chilledTimer -= Time.deltaTime;
		shockedTimer -= Time.deltaTime;

		igniteDamageTimer -= Time.deltaTime;

		if (ignitedTimer < 0)
		{
			isIgnited = false;
		}

		if (chilledTimer < 0)
		{
			isChilled = false;
		}

		if (shockedTimer < 0)
		{
			isShocked = false;
		}
		if (isIgnited)
			ApplyIgniteDamage();
	}

	public void MakeVurableFor(float _duration) => StartCoroutine(VulnerableForCorutine(_duration));


	private IEnumerator VulnerableForCorutine(float _duration)
	{
		isVulnerable = true;

		yield return new WaitForSeconds(_duration);

		isVulnerable = false;
	}

	public virtual void IncreaseStatBy(int _modifier,float _duration,Stats _statToModify)
	{
		// start corotouine for stat Increase
		StartCoroutine(StatModCoroutine(_modifier, _duration, _statToModify));
	}

	private IEnumerator StatModCoroutine(int _modifier, float _duration, Stats _statToModify)
	{
		_statToModify.AddModifier(_modifier);
		yield return new WaitForSeconds(_duration);
		_statToModify.RemoveModifier(_modifier);
	}

	public virtual void DoDamage(CharacterStats _targetStats)
	{
		bool critcalStrike = false;
		// 总闪避值
		if (_targetStats.isInvincible) return;
		if (TargetCanAvoidAttack(_targetStats)) return;

		_targetStats.GetComponent<Entity>().SetupKnockbackDir(transform);
		int totalDamage = damage.GetValue() + strength.GetValue();
		if (CanCrit())
		{
			totalDamage = CalculateCritDamage(totalDamage);
			critcalStrike = true;
		}
		fx.CreateHitFx(_targetStats.transform, critcalStrike);
		totalDamage = CheckTargetArmor(_targetStats, totalDamage);

		// 检查魔法攻击 魔法攻击就检测魔法攻击
		// int magicalDamage = DoMagicalDamage(_targetStats);
		// totalDamage += magicalDamage;
		_targetStats.TakeDamage(totalDamage);
		// DoMagicalDamage(_targetStats); // 如果普通攻击不想运魔法伤害就移除

	}

	#region magic and ailments
	public virtual void DoMagicalDamage(CharacterStats _targetStats)
	{
		// 魔法攻击
		int _fireDamage = fireDamage.GetValue();
		int _iceDamage = iceDamage.GetValue();
		int _lightingDamage = lightingDamage.GetValue();
		int totalMagicalDamage = _fireDamage + _iceDamage + _lightingDamage + intelligence.GetValue();

		// 检查魔抗
		totalMagicalDamage = CheckTargetResistance(_targetStats, totalMagicalDamage);
		_targetStats.TakeDamage(totalMagicalDamage);

		if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0) return;
		AttemptyToApplyAilments(_targetStats, _fireDamage, _iceDamage, _lightingDamage);

	}

	private void AttemptyToApplyAilments(CharacterStats _targetStats, int _fireDamage, int _iceDamage, int _lightingDamage)
	{
		bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;
		bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightingDamage;
		bool canApplyShock = _lightingDamage > _fireDamage && _lightingDamage > _iceDamage;


		while (!canApplyIgnite && !canApplyChill && !canApplyShock)
		{

			// 避免三个值都相同
			if (Random.value < .33f && _fireDamage > 0)
			{
				canApplyIgnite = true;
				break;
			}

			if (Random.value < .5f && _iceDamage > 0)
			{
				canApplyIgnite = true;
				break;
			}

			if (Random.value < 1.0f && _lightingDamage > 0)
			{
				canApplyIgnite = true;
				break;
			}
		}
		if (canApplyIgnite)
		{
			_targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f)); // 本身火的伤害为 firedamage, 后续的伤害为fireDamage的 1/5
		}

		if (canApplyShock)
		{
			_targetStats.SetupShockStrikeDamage(Mathf.RoundToInt(_lightingDamage * .4f)); // 附加伤害为light的 40%
		}
		_targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
	}

	public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
	{

		/*
		if(isIgnited || isChilled || isShocked)
		{
			return;
		}
		*/
		// 先取消原有的颜色闪烁
		fx.CancelColorChange();
		if (_ignite)
		{
			isIgnited = _ignite;
			// 新状态来临时将旧状态置为false
			isChilled = false;
			isShocked = false;
			ignitedTimer = ailmentsDuration;

			fx.IgniteFxFor(ailmentsDuration);
		}
		else if (_chill)
		{
			isChilled = _chill;
			isIgnited = false;
			isShocked = false;
			chilledTimer = ailmentsDuration;
			float slowPercentage = .2f;
			GetComponent<Entity>().SlowEntityBy(slowPercentage, ailmentsDuration);
			fx.ChillFxFor(ailmentsDuration);

		}
		else if (_shock)
		{
			isShocked = _shock;
			isIgnited = false;
			isChilled = false;
			shockedTimer = ailmentsDuration;
			fx.ShockFxFor(ailmentsDuration);
			ShockTarget(shockStrikePerfab, shockDamage);
		}
	}

	protected virtual void ShockTarget(GameObject _shockStrikePerfab, int _shockDamage)
	{
		
	}

	public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;

	public void SetupShockStrikeDamage(int _damage) => shockDamage = _damage;

	private static int CheckTargetResistance(CharacterStats _targetStats, int totalMagicalDamage)
	{
		// 伤害减免率 = 魔抗 / 100 + 魔抗

		float resitance = _targetStats.magicResistance.GetValue() +
			(_targetStats.intelligence.GetValue() * 3);
		float attackDefensePercent = resitance / (100 + resitance);

		totalMagicalDamage = Mathf.RoundToInt(totalMagicalDamage * (1 - attackDefensePercent));
		totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
		return totalMagicalDamage;
	}

	private void ApplyIgniteDamage()
	{
		if (igniteDamageTimer < 0)
		{

			DecreaseHealthBy(igniteDamage);

			if (currentHealth <= 0 && !isDead)
			{
				Die();
			}
			igniteDamageTimer = igniteDamageCooldown;
		}
	}

	#endregion

	public virtual void TakeDamage(int _damage)
	{
		if (isInvincible)
			return;

		if (isVulnerable)
		{
			_damage = Mathf.RoundToInt(_damage * 1.1f);
		}
		DecreaseHealthBy(_damage);
		GetComponent<Entity>().DamageEffect();

		if (currentHealth <= 0)
		{
			Die();
		}

	}

	public virtual void IncreaseHealthBy(int _amount)
	{
		currentHealth += _amount;
		if(currentHealth > GetMaxHealthValue())
		{
			currentHealth = GetMaxHealthValue();
		}

		onHealthChanged?.Invoke();
	}

	protected virtual void DecreaseHealthBy(int _damage)
	{
		if (currentHealth - _damage <= 0)
			currentHealth = 0;
		else
			currentHealth -= _damage;
		if(_damage > 0)
		{
			fx.CreatePopUpText("-" + _damage.ToString());
		}
		onHealthChanged?.Invoke();
	}


	public virtual void Die()
	{
		isDead = true;
	}

	public void KillEntity()
	{
		if(!isDead)
			Die();
	}
		
	public virtual void OnEvasion()
	{
		
	}

	#region Stat calculations
	protected bool TargetCanAvoidAttack(CharacterStats _targetStats)
	{
		int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();
		_targetStats.OnEvasion();

		if (_targetStats.isShocked)
		{
			// 增加闪避率
			// 只是针对某个怪的闪避率,不计入UI更新中
			totalEvasion += 20;
		}
		if (Random.Range(0, 100) < totalEvasion)
		{
			return true;
		}
		return false;
	}

	protected int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
	{
		// 伤害减免率 = 护甲 / 100 + 护甲

		// 如果被冰冻可以削弱护甲
		int totalArmor = _targetStats.isChilled ? Mathf.RoundToInt(_targetStats.armor.GetValue() * .8f) : _targetStats.armor.GetValue();
		float attackArmorPercent = totalArmor / (100 + totalArmor);

		totalDamage = Mathf.RoundToInt(totalDamage * (1 - attackArmorPercent));
		totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
		return totalDamage;
	}

	protected bool CanCrit()
	{
		int totalCritcalChance = critChance.GetValue() + agility.GetValue(); // 敏捷度也会提高暴击的几率
		if (Random.Range(0, 100) <= totalCritcalChance)
		{
			return true;
		}
		return false;
	}

	protected int CalculateCritDamage(int _damage)
	{
		float totalCritPower = (critPower.GetValue() + strength.GetValue()) * .01f;

		float critDamage = _damage * totalCritPower;

		return Mathf.RoundToInt(critDamage);
	}

	//生命力也可以加最大血量,一个生命力五点血量
	public int GetMaxHealthValue() => maxHealth.GetValue() + vitality.GetValue()* 5;

	#endregion

	public void MakeInvincible(bool _invincible) => isInvincible = _invincible;
	public virtual Stats StatToModify(StatType buffType)
	{
		switch (buffType)
		{
			case StatType.strength:
				return strength;
			case StatType.agility:
				return agility;
			case StatType.intelligence:
				return intelligence;
			case StatType.vitality:
				return vitality;

			case StatType.damage:
				return damage;
			case StatType.critChance:
				return critChance;
			case StatType.critPower:
				return critPower;

			case StatType.health:
				return maxHealth;
			case StatType.armor:
				return armor;
			case StatType.magicResistance:
				return magicResistance;
			case StatType.evasion:
				return evasion;

			case StatType.fireDamage:
				return fireDamage;
			case StatType.iceDamage:
				return iceDamage;
			case StatType.lightingDamage:
				return lightingDamage;

			default:
				return null;
		}
	}
}
