using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerState
{
	protected PlayerStateMachine stateMachine;
	protected Player player;

	protected Rigidbody2D rb;

	protected static int jumpCount = 0;
	private string animBoolName;
	protected float xinput = 0;
	protected float yinput = 0;

	protected float stateTimer;
	// ¹¥»÷´¥·¢
	protected bool triggerCalled;

	public PlayerState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName)
	{
		this.stateMachine = _stateMachine;
		this.player = _player;
		this.animBoolName = _animBoolName;
	}

	public virtual void Enter()
	{
		rb = player.rb;
		// Debug.Log("I enter " + animBoolName);
		player.anim.SetBool(animBoolName, true);
		triggerCalled = false;
	}

	public virtual void Update()
	{
		stateTimer -= Time.deltaTime;
		xinput = Input.GetAxisRaw("Horizontal");
		player.anim.SetFloat("yVelocity", rb.velocity.y);
		yinput = Input.GetAxisRaw("Vertical");
		if(stateMachine.currentState != player.blackholeState)
		{
			player.fx.MakeTransparent(false);
		}

	}
	public virtual void Exit()
	{
		// Debug.Log("I exit " + animBoolName);
		player.anim.SetBool(animBoolName, false);
	}

	public virtual void AnimationFinishTrigger()
	{
		triggerCalled = true;
	}
}
