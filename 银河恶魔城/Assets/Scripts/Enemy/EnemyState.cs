using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyState
{
	protected EnemyStateMachine stateMachine;
	protected Enemy enemyBase;

    protected Rigidbody2D rb;

	private string animBoolName;
    protected float stateTimer;
    protected bool triggerCalled;

	protected FacingDir moveDir { get; set; } = FacingDir.Right;

	public EnemyState(Enemy _enemyBase,EnemyStateMachine _stateMachine,string _animBoolName)
    {
        this.enemyBase = _enemyBase;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
		rb = enemyBase.rb;
		triggerCalled = false;
		enemyBase.anim.SetBool(animBoolName, true);
	}

    public virtual void Exit() 
    {
        enemyBase.anim.SetBool(animBoolName, false);
        enemyBase.AssignLastAnimName(animBoolName);
    }


	public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
