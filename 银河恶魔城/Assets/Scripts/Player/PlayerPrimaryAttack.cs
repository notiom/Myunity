using UnityEngine;

public class PlayerPrimaryAttack : PlayerState
{
	public int comboCounter { get; private set; }

	private float lastTimeAttacked;
	private float comboWindow = 0.5f;
	public PlayerPrimaryAttack(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();
		xinput = 0; // 需要使用他去修正攻击的方向
					// Debug.Log("comboCounter" + comboCounter);
		if (comboCounter > 2 || Time.time - lastTimeAttacked >= comboWindow) comboCounter = 0;
		player.anim.SetInteger("ComboCounter", comboCounter);
		#region attack dir
		FacingDir attackDir = player.facingDir;
		if (xinput != 0) attackDir = xinput > 0 ? FacingDir.Right : FacingDir.Left;
		#endregion
		player.SetVelocity(player.attackMovement[comboCounter].x * (int)attackDir, player.attackMovement[comboCounter].y);
		stateTimer = .1f;
	}

	public override void Exit()
	{
		base.Exit();
		comboCounter++;
		lastTimeAttacked = Time.time;
		player.StartCoroutine("BusyFor", .15f);
		// Debug.Log("lastTimeAttacked" + lastTimeAttacked);
	}

	public override void Update()
	{
		base.Update();
		if (stateTimer < 0)
			player.SetVelocity(0, 0);
		if (triggerCalled)
		{
			stateMachine.ChangeState(player.idleState);
		}
	}
}
