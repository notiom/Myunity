using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill_Controller : MonoBehaviour
{

	[SerializeField] private GameObject hotKeyPrefab;
	[SerializeField] private List<KeyCode> keyCodeList;
	public float maxSize;
	public float growSpeed;
	public float shrinkSpeed;
	private bool canGrow = true;
	private bool canShrink = false;
	private float blackholeTimer;

	private int amountOfAttacks = 4;
	private float cloneAttackCooldown = .3f;
	private float cloneAttackTimer;
	private bool canCreateHotKeys = true;
	private bool canAttack;

	private bool playerCanDisapear = true;
	private List<Transform> targets = new List<Transform>();
	private List<GameObject> createHotKey = new List<GameObject>();

	public bool playerCanExitState { get; private set; }

	public void SetupBlackhole(float _maxSize, float _growSpeed, float _shrinkSpeed, int _amountOfAttacks, float _cloneAttackCooldown, float _blackholeDuration)
	{
		maxSize = _maxSize;
		growSpeed = _growSpeed;
		shrinkSpeed = _shrinkSpeed;
		amountOfAttacks = _amountOfAttacks;
		cloneAttackCooldown = _cloneAttackCooldown;
		blackholeTimer = _blackholeDuration;

		if(SkillManager.instance.clone.crystalInsteadOfClone)
		{
			playerCanDisapear = false;
		}
	}
	private void Start()
	{
		transform.localScale = new Vector2(0.5f, 0.5f);
	}
	private void Update()
	{
		cloneAttackTimer -= Time.deltaTime;
		blackholeTimer -= Time.deltaTime;

		if(blackholeTimer < 0)
		{
			blackholeTimer = Mathf.Infinity;
			if(targets.Count > 0)
			{
				ReleaseCloneAttack();
			}
			else 
			{
				DestoryHotKeys();
				FinishBlackHoleAbility();
			}

		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			ReleaseCloneAttack();
		}

		CloneAttackLogic();

		if (canGrow && !canShrink)
		{
			transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
		}

		if (canShrink)
		{
			transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);

			if (transform.localScale.x < 0)
			{
				PlayerManager.instance.player.fx.MakeTransparent(false);
				Destroy(gameObject);
			}

		}
	}


	private void ReleaseCloneAttack()
	{
		DestoryHotKeys();
		canAttack = true;
		canCreateHotKeys = false;

		if(playerCanDisapear)
		{
			playerCanExitState = false;
			PlayerManager.instance.player.fx.MakeTransparent(true);
		}

	}

	private void CloneAttackLogic()
	{
		if (cloneAttackTimer < 0 && canAttack)
		{
			cloneAttackTimer = cloneAttackCooldown;

			if(targets.Count == 0) 
			{
				DestoryHotKeys();
				FinishBlackHoleAbility();
				canAttack = false;
				return;
			}
			int randomIndex = Random.Range(0, targets.Count);
			float xOffset;
			if (Random.Range(0, 100) > 50) xOffset = .5f;
			else xOffset = -.5f;

			if(SkillManager.instance.clone.crystalInsteadOfClone)
			{
				SkillManager.instance.crystal.CreateCrystal();
				SkillManager.instance.crystal.SetAttackToEnemy(targets[randomIndex]);
			}
			else
			{
				if(targets[randomIndex] != null)
					SkillManager.instance.clone.CreateClone(targets[randomIndex], new Vector3(xOffset, 0));
				else
					targets.RemoveAt(randomIndex);
			}

			amountOfAttacks--;

			if (amountOfAttacks <= 0)
			{
				canAttack = false;
				Invoke("FinishBlackHoleAbility", .5f);
			}
		}
	}

	private void FinishBlackHoleAbility()
	{
		canShrink = true;
		playerCanExitState = true;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.GetComponent<Enemy>() != null)
		{
			collision.GetComponent<Enemy>().FreezeTime(true);

			CreateHotKey(collision);
		}
	}

	private void CreateHotKey(Collider2D collision)
	{
		if (keyCodeList.Count <= 0)
		{
			return;
		}

		if (!canCreateHotKeys) return;

		GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
		createHotKey.Add(newHotKey);
		KeyCode chooseKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
		keyCodeList.Remove(chooseKey);

		newHotKey.GetComponent<Blackhole_Hotkey_Controller>().SetupHotKey(chooseKey, collision.transform, this);
		// targets.Add(collision.transform);
	}

	private void DestoryHotKeys()
	{
		if(createHotKey.Count <= 0) 
		{
			return;
		}
		for(int i = 0;i < createHotKey.Count;i++) 
		{
			Destroy(createHotKey[i]);
		}
	}

	private void OnTriggerExit2D(Collider2D collision) => collision.GetComponent<Enemy>()?.FreezeTime(false);


	public void AddEnemyToList(Transform _enemyTransform) => targets.Add(_enemyTransform);
}
