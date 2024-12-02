using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask,

}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class EquipmentData :ItemData
{
    public EquipmentType equipmentType;
	[Header("Unique effect")]
	public float itemCooldown;
	public ItemEffect[] itemEffects;
	[TextArea]
	public string itemEffectDescription;

	[Header("Major stats")]
	public int strength; // 可以增加一点伤害
	public int agility;  // 可以增加闪避和关键打击能力
	public int intelligence; // 增加魔法伤害和魔抗 1点魔法伤害和3点魔抗
	public int vitality;  // 增加生命值

	[Header("Offensive stats")]
	public int damage; //基础伤害
	public int critChance; //暴击几率
	public int critPower;  // 默认值是150% 暴击伤害

	[Header("Defencive stats")]
	public int health; //hp
	public int armor; // 护甲
	public int magicResistance; //魔抗
	public int evasion; //闪避值

	[Header("Magic stats")]
	public int fireDamage; // 火伤
	public int iceDamage; // 冰伤
	public int lightingDamage; //光伤

	[Header("Craft requirements")]
	private int descriptionLength;
	public List<InventoryItem> craftingMaterials;

	public void Effect(Transform _enemyPosition)
	{
		foreach (var item in itemEffects)
		{
			item.ExecuteEffect(_enemyPosition);
		}
	}
	public void AddModifiers()
    {
		PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

		playerStats.strength.AddModifier(strength);
		playerStats.agility.AddModifier(agility);
		playerStats.intelligence.AddModifier(intelligence);
		playerStats.vitality.AddModifier(vitality);

		playerStats.damage.AddModifier(damage);
		playerStats.critChance.AddModifier(critChance);
		playerStats.critPower.AddModifier(critPower);

		playerStats.maxHealth.AddModifier(health);
		playerStats.armor.AddModifier(armor);
		playerStats.magicResistance.AddModifier(magicResistance);
		playerStats.evasion.AddModifier(evasion);

		playerStats.fireDamage.AddModifier(fireDamage);
		playerStats.iceDamage.AddModifier(iceDamage);
		playerStats.lightingDamage.AddModifier(lightingDamage);

    }

	public void RemoveModifiers()
	{
		PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

		playerStats.strength.RemoveModifier(strength);
		playerStats.agility.RemoveModifier(agility);
		playerStats.intelligence.RemoveModifier(intelligence);
		playerStats.vitality.RemoveModifier(vitality);

		playerStats.damage.RemoveModifier(damage);
		playerStats.critChance.RemoveModifier(critChance);
		playerStats.critPower.RemoveModifier(critPower);

		playerStats.maxHealth.RemoveModifier(health);
		playerStats.armor.RemoveModifier(armor);
		playerStats.magicResistance.RemoveModifier(magicResistance);
		playerStats.evasion.RemoveModifier(evasion);

		playerStats.fireDamage.RemoveModifier(fireDamage);
		playerStats.iceDamage.RemoveModifier(iceDamage);
		playerStats.lightingDamage.RemoveModifier(lightingDamage);
	}

	public override string GetDescription()
	{
		sb.Length = 0;
		descriptionLength = 0;
		AddItemDescription(strength, "strength");
		AddItemDescription(agility, "agility");
		AddItemDescription(intelligence, "intelligence");
		AddItemDescription(vitality, "vitality");

		AddItemDescription(damage, "damage");
		AddItemDescription(critChance, "critChance");
		AddItemDescription(critPower, "critPower");

		AddItemDescription(health, "health");
		AddItemDescription(armor, "armor");
		AddItemDescription(magicResistance, "magicResistance");
		AddItemDescription(evasion, "evasion");

		AddItemDescription(fireDamage, "fireDamage");
		AddItemDescription(iceDamage, "iceDamage");
		AddItemDescription(lightingDamage, "lightingDamage");
		AddItemDescription(sellPrices, "sellPrices");

		if (descriptionLength < 5)
		{
			for(int i = 0;i < descriptionLength;i++) 
			{
				sb.AppendLine();
				sb.Append("");
			}
		}

		if(itemEffectDescription.Length > 0)
		{
			// sb.AppendLine();
			sb.Append(itemEffectDescription);
		}

		return sb.ToString();
	}

	private void AddItemDescription(int _value,string _name)
	{
		if(_value != 0)
		{
			if(sb.Length > 0) 
			{
				sb.AppendLine();
			}

			if(_value > 0)
			{
				sb.Append("+" + _value + " " + _name);
			}
			descriptionLength++;
		}
	}

}
