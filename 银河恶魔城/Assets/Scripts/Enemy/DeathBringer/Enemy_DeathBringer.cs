using UnityEngine;

public class Enemy_DeathBringer : Enemy
{
	public bool isBusy;
	[Header("Teleport details")]
	[SerializeField] private float defaultChanceToTeleport;
	[HideInInspector] private float chanceToTeleport;
	private float lastTimeTeleport;
	private float teleportTimer;
	[SerializeField] private float teleportCooldown;
	[SerializeField] private float checkTeleportCooldown;

	[Header("Spell cast details")]
	[SerializeField] private GameObject spellPrefab;
	[SerializeField] private float defaultSpellChance;
	[HideInInspector] private float spellChance;
	private float lastTimeCast;
	[SerializeField] private float spellCastCooldown;
	// 每隔几s检查一次是否可以释放技能并且如果不释放增加施释放几率
	private float speelCastTimer;
	[SerializeField] private float checkCastCooldown;

	[HideInInspector] public int amountOfSpells;
	[SerializeField] private int baseAmountOfSpells = 2;

	public bool fireOrIce;
	public bool bossbegunBattle;


	#region States
	public DeathBringerIdleState idleState { get; private set; }
	public DeathBringerTeleportState teleportState { get; private set; }
	public DeathBringerBattleState battleState { get; private set; }
	public DeathBringerAttackState attackState { get; private set; }
	public DeathBringerSpellCastState speilCastState { get; private set; }
	public DeathBringerDeadState deadState { get; private set; }

	#endregion

	#region Awake
	protected override void Awake()
	{
		base.Awake();
		idleState = new DeathBringerIdleState(this, stateMachine, "Idle", this);
		teleportState = new DeathBringerTeleportState(this, stateMachine, "Teleport", this);
		battleState = new DeathBringerBattleState(this, stateMachine, "Move", this);
		attackState = new DeathBringerAttackState(this, stateMachine, "Attack", this);
		speilCastState = new DeathBringerSpellCastState(this, stateMachine, "SpeilCast", this);
		deadState = new DeathBringerDeadState(this, stateMachine, "Dead", this);
	}
	#endregion

	protected override void Start()
	{
		base.Start();
		stateMachine.Initialize(idleState);
	}

	protected override void Update()
	{
		base.Update();
		if (stats.currentHealth >= Mathf.RoundToInt(stats.GetMaxHealthValue() * 0.75f))
		{
			amountOfSpells = baseAmountOfSpells;
		}
		else if (stats.currentHealth >= Mathf.RoundToInt(stats.GetMaxHealthValue() * 0.5f))
		{
			amountOfSpells = 2 * baseAmountOfSpells;
		}

		else if (stats.currentHealth >= Mathf.RoundToInt(stats.GetMaxHealthValue() * 0.25f))
		{
			amountOfSpells = 3 * baseAmountOfSpells;
		}

		else
		{
			amountOfSpells = 4 * baseAmountOfSpells;
		}
		if (isBusy) return;
		speelCastTimer -= Time.deltaTime;
		teleportTimer -= Time.deltaTime;
		if (speelCastTimer < 0)
		{
			speelCastTimer = checkCastCooldown;
			if (Random.Range(0, 100) < spellChance && CanDoSpellCast())
			{
				spellChance = defaultSpellChance;
				stateMachine.ChangeState(speilCastState);
				return;
			}
			spellChance += defaultSpellChance;
		}

		if (teleportTimer < 0)
		{
			teleportTimer = checkTeleportCooldown;
			if (Random.Range(0, 100) < chanceToTeleport && CanToTeleport())
			{
				chanceToTeleport = defaultChanceToTeleport;
				stateMachine.ChangeState(teleportState);
				return;
			}
			chanceToTeleport += defaultChanceToTeleport;
		}

	}

	public override void Die()
	{
		base.Die();
		stateMachine.ChangeState(deadState);
	}

	public void CastSpell()
	{
		Player player = PlayerManager.instance.player;

		float xOffset = player.rb.velocity.x == 0 ? 0 : (int)player.facingDir;
		Vector3 spellPosition = new Vector3(player.transform.position.x + xOffset,
											player.transform.position.y + 1.5f);

		GameObject newSpell = Instantiate(spellPrefab, spellPosition, Quaternion.identity);

		newSpell.GetComponent<DeathBringerSpell_Controller>().SetupSpell(stats);
	}

	public void FindPosition()
	{
		Vector3 checkPosition = PlayerManager.instance.player.transform.position + new Vector3(Random.Range(-2, 2), 0);
		if (IsPointOnLayer(checkPosition, "Brick"))
		{
			Debug.Log("撞墙！");
			return;
		}
		transform.position = checkPosition;
	}

	private bool IsPointOnLayer(Vector2 position, string layerName, float radius = 2f)
	{
		// 使用 Physics2D.OverlapPoint 检查该位置是否与任何物体发生碰撞
		Collider2D collider = Physics2D.OverlapCircle(position, radius);

		// 如果有碰撞器并且碰撞的物体属于指定的层
		if (collider != null && collider.gameObject.layer == LayerMask.NameToLayer(layerName))
		{
			return true;
		}
		return false;
	}

	public bool CanToTeleport()
	{
		if (Time.time >= lastTimeTeleport + teleportCooldown)
		{
			lastTimeTeleport = Time.time;
			return true;
		}
		return false;
	}

	private bool CanDoSpellCast()
	{
		if (Time.time >= lastTimeCast + spellCastCooldown)
		{

			lastTimeCast = Time.time;
			return true;
		}
		return false;
	}

}
