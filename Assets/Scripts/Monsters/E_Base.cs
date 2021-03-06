﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class E_Stats
{
	public int health;
	public int maxHealth;
	public float defense;
	public float attack;
	public float moveSpd;
	public float atkSpd;
	public float minAggro;
	public float maxAggro;
};

public enum E_States
{
	IDLE,
	TRACKING,
	ATTACK,
	STUNNED,
	STATE_TOTAL
}

public abstract class E_Base : MonoBehaviour {
	public E_States states;
	public E_Stats stats=new E_Stats();
	public int currentHealth;
	public float knockbackDrag;
	public LayerMask Tracks;
	public float detectionRange=6;
	public float attackRange=3;
	public Collider2D Target;
	public float invincTime;
	public float invincTimeLeft;
	protected Vector3 originalPos;
	protected Vector3 lastIdlePos;
	protected Vector3 knockBackEffect;
	protected Rigidbody2D rb ;
	protected E_States LastState;
	protected C_Base characterScript;
	public float timeLeft;//for timings with state machine
	protected bool stateChange;//tracks if state has changed
	protected Animator anim;
	protected bool stunned=false;
	protected float InitialKnockBack;
	protected float stunDur = 0.0f;
	public bool dead=false;
	public float DeadTimer=2.0f;
	public GameObject healthBar;
	protected List<SpriteRenderer> spriteR=new List<SpriteRenderer>();
	bool DroppedWeapon=false;
	public GameObject WeaponPrefab;


	// Use this for initialization
	virtual protected void Start () {
		originalPos = transform.position;
		rb = gameObject.GetComponent<Rigidbody2D> ();
		knockbackDrag = 0.8f;
		stateChange = false;
		anim = GetComponent<Animator> ();
		spriteR.AddRange( GetComponents<SpriteRenderer> ());
		spriteR.AddRange (GetComponentsInChildren<SpriteRenderer> ());

		if (stats.health > 0) {
			stats.maxHealth=stats.health;
		}
		Transform temp= transform.FindChild ("HealthBar");
		if (temp)
			healthBar = temp.gameObject;

		WeaponPrefab = Resources.Load ("Weapons/Weapon")as GameObject;
	}

	protected abstract void Attack_State();

	protected abstract void Tracking_State();

	protected abstract void Idle_State();

	protected virtual void OnDeath()
	{
		if(!DroppedWeapon)
		{
			DroppedWeapon=true;
			int t=Random.Range(0,100);
			if(t<10)
			{
				GameObject temp=Instantiate(WeaponPrefab,transform.position,Quaternion.identity)as GameObject;
				weaponInfo t2=temp.GetComponent<Weapon>().info;
				//if(t2)
				{
					int level=GameObject.FindGameObjectWithTag("Character").GetComponent<C_Base>().stats.Level;
					t2.damage=level+Random.Range(0,5)-2;
					t2.spriteName="weapon"+Random.Range(0,8).ToString();
					int a=Random.Range(0,2);
					if(t2.damage<=0)
					{
						t2.damage=1;
					}
					if(a==0)
					{
						t2.wtype=WeaponTypes.ATTACK;
					}
					else
					{
						t2.wtype=WeaponTypes.DEFENCE;
					}
				}
			}
		}
		if(!anim)
			Destroy (gameObject);
		if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime>=0)
		{
			DeadTimer-=Time.deltaTime;
			if(DeadTimer<=0)
			{
				foreach(SpriteRenderer s in spriteR)
				{
					Color tempColor=s.color;
					tempColor.a-=1.0f*Time.deltaTime;
					s.color=tempColor;
					if(s.color.a<=0)
					{
						Destroy(gameObject);
						break;
					}
				}
			}
		}
	}

	// Update is called once per frame
	virtual protected void Update () {
		if (healthBar) {
			float nx=(float)((float)stats.health / (float)stats.maxHealth);
			if(nx<0)
				nx=0;
			healthBar.transform.localScale = new Vector3 (nx, 1, 1);
		}
		if (dead) {
			OnDeath();
		} else {
			invincTimeLeft -= Time.deltaTime;
			currentHealth = stats.health;
			if (knockBackEffect.sqrMagnitude > 0) {
				float reduction = InitialKnockBack * knockbackDrag;
				knockBackEffect -= knockBackEffect * reduction * Time.deltaTime;
				//knockBackEffect-=minus;
				transform.position += knockBackEffect * Time.deltaTime;
				if (knockBackEffect.sqrMagnitude < 0.1f)
					knockBackEffect = Vector2.zero;

			} else if (!stunned) {
				ChangeState ();
				UpdateStates ();
			} else {
				stunDur -= Time.deltaTime;
				if (stunDur <= 0) {
					stunHandle ();
				}
			}
		}
	}

	protected virtual void stunHandle()
	{

			stunned=false;
			states=E_States.IDLE;
			if(anim)
				anim.SetBool("Flinch",false);
	}
	
	virtual public void ApplyDamage(float attack,C_Base c)
	{
		if (invincTimeLeft > 0)
			return;
		invincTimeLeft = invincTime;
		//apply armor application
		stats.health -= Mathf.CeilToInt(attack);
		if (stats.health <= 0) {
			c.stats.AddExp(10);
			if(anim)
			{
				anim.SetTrigger("Death");
				anim.SetBool("Flinch",false);
				dead=true;
			}
			else{
				Destroy(gameObject);
			}
		}
	}
	virtual public void KnockBack(float amount,Vector2 Dir,float stunDuration)
	{
		if (dead)
			return;
		stunDur = stunDuration;
		knockBackEffect = Dir * amount;
		stunned = true;
		LastState = states;
		InitialKnockBack = amount;
		states = E_States.STUNNED;
		if(anim)
			anim.SetBool ("Flinch", true);
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
