using UnityEngine;

public class DeathBringerSpellCastState : EnemyState
{
	private Enemy_DeathBringer enemy;

	private int amountOfSpells;
	private float spellCoodlown = .8f;
	private float spellTimer;
	public DeathBringerSpellCastState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
	{
		this.enemy = _enemy;
	}

	public override void Enter()
	{
		base.Enter();
		enemy.isBusy = true;
		enemy.stats.MakeInvincible(true);
		amountOfSpells = enemy.amountOfSpells;
		spellTimer = spellCoodlown;
	}

	public override void Exit()
	{
		base.Exit();
		enemy.isBusy = false;
		enemy.stats.MakeInvincible(false);
	}

	public override void Update()
	{
		base.Update();
		spellTimer -= Time.deltaTime;
		if (amountOfSpells <= 0)
		{
			stateMachine.ChangeState(enemy.teleportState);
			return;
		}

		if (spellTimer < 0)
		{
			spellTimer = spellCoodlown;
			amountOfSpells--;
			enemy.CastSpell();
		}
	}
}
