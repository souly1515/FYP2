﻿using UnityEngine;
using System.Collections;

public class E_Stats
{
	public int health;
	public float defense;
	public float attack;
	public float moveSpd;
	public float atkSpd;
	public float minAggro;
	public float maxAggro;
};

public enum E_States
{
	STATE_NONE=0,
	IDLE,
	TRACKING,
	ATTACK,
	STATE_TOTAL
}

public abstract class E_Base : MonoBehaviour {
	public E_States states;
	public E_Stats stats=new E_Stats();
	public int currentHealth;
	public float knockbackDrag;
	public LayerMask Tracks;
	public float detectionRange=10;
	public float attackRange=5;
	public Collider2D Target;
	protected Vector2 originalPos;
	protected Vector2 lastIdlePos;
	protected Vector2 knockBackEffect;
	protected Rigidbody2D rb ;
	public float timeLeft;//for timings with state machine
	protected bool stateChange;//tracks if state has changed
	bool stunned=false;
	float stunDur = 0.0f;

	// Use this for initialization
	virtual protected void Start () {
		stats.health = 5;
		originalPos = transform.position;
		rb = gameObject.GetComponent<Rigidbody2D> ();
		knockbackDrag = 0.8f;
		stateChange = false;
	}

	protected abstract void Attack_State();

	protected abstract void Tracking_State();

	protected abstract void Idle_State();

	// Update is called once per frame
	virtual protected void Update () {
		currentHealth = stats.health;
		if (knockBackEffect.sqrMagnitude > 0) {

			Vector2 minus=rb.velocity*(1-knockbackDrag);
			rb.velocity=rb.velocity-minus;
			knockBackEffect-=minus;
			
			if(rb.velocity.sqrMagnitude<0.1f)
			{
				knockBackEffect=Vector2.zero;
				rb.velocity=Vector2.zero;
			}
			else if(knockBackEffect.sqrMagnitude<0.1f)
				knockBackEffect=Vector2.zero;

		}
		if (!stunned) 
		{
			ChangeState ();
		}
		else 
		{
			states=E_States.IDLE;
			stunDur-=Time.deltaTime;
			if(stunDur<=0)
			{
				stunned=false;
			}
		}
		UpdateStates ();
	}

	public void ApplyDamage(float attack,float stunDur)
	{
		//apply armor application
		//stats.health -= (int)attack;
		Debug.Log ("Damaged\n");
		if (stats.health <= 0) {
			gameObject.SetActive (false);

		}
	}
	public void KnockBack(float amount,Vector2 Dir)
	{
		knockBackEffect = Dir * amount;
		rb.velocity = Dir * amount;
	}

	protected virtual void ChangeState()
	{
			switch (states) {
		case E_States.IDLE:
			Target = Physics2D.OverlapCircle (gameObject.transform.position, detectionRange, Tracks);
			if (Target) {
				states = E_States.TRACKING;
				stateChange = false;
			}
			break;
		case E_States.ATTACK:
			if (!Physics2D.OverlapCircle (gameObject.transform.position, attackRange, Tracks)) {
				states = E_States.TRACKING;
				stateChange = true;
			}
			break;
		case E_States.TRACKING:
			if (!Physics2D.OverlapCircle (gameObject.transform.position, detectionRange, Tracks)) {
				states = E_States.IDLE;
				stateChange = true;
				//rb.velocity=knockBackEffect;
			} else if (Physics2D.OverlapCircle (gameObject.transform.position, attackRange, Tracks)) {
				states = E_States.ATTACK;
				stateChange = true;
				//rb.velocity=knockBackEffect;
			}

			break;
		}
	}

	protected  virtual void UpdateStates()
	{
		switch (states) {
			
		case E_States.IDLE:
			break;
		case E_States.ATTACK:
			//attacking code here
			Attack_State();
			break;
		case E_States.TRACKING:
			Tracking_State();
			break;
		}
		stateChange=false;
	}
}