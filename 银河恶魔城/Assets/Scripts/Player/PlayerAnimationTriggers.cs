using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    public void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
        foreach(var hit in colliders) 
        {
            if(hit.GetComponent<Enemy>() != null)
            {
				// �е����ڹ�����Χ��
				EnemyStats _target = hit.GetComponent<EnemyStats>();
                if(_target != null) 
                {
					player.stats.DoDamage(_target);
				}


				// inventory get weapon call item effect
                // ����û������ʱ�ĵ���
				Inventory.instance.GetEquipment(EquipmentType.Weapon)?.Effect(_target.transform);
            }
        }
    }

	private void ThrowSword()
	{
        SkillManager.instance.sword.createSword();
	}

}
