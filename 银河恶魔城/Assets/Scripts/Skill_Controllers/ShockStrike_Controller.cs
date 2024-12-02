using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockStrike_Controller : MonoBehaviour
{
    [SerializeField] private CharacterStats targetStats;
    [SerializeField] private float speed;

    private int damage;
    private Animator anim;
    private bool triggered;
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void Setup(int _damage,CharacterStats _targetStats)
    {
        damage = _damage;
        targetStats = _targetStats;
    }
    // Update is called once per frame
    void Update()
    {
        if (!targetStats)
            return;
        if(triggered) 
        {
            return;
        }
        // 不能让这个雷电电距离最近的目标了
        // transform.position = Vector2.MoveTowards(transform.position, targetStats.transform.position, speed * Time.deltaTime);
        // transform.right = transform.position - targetStats.transform.position;

		if (Vector2.Distance(transform.position, targetStats.transform.position) < .1f)
        {
			anim.transform.localPosition = new Vector3(0, .5f);
			anim.transform.localRotation = Quaternion.identity;
            transform.rotation = Quaternion.identity;
            transform.localScale = new Vector3(2.5f, 2.5f);

            triggered = true;
			anim.SetTrigger("Hit");
		}
    }

    public void DamageAndSelfDestory()
    {
		targetStats.TakeDamage(damage);
		Destroy(gameObject, .4f);
	}

}
