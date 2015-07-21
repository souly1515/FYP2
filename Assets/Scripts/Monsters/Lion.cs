using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Lion : E_Base {
	enum InternalAttackStates{
		SPIKE_ATTACK,
		SPIKE_PREP,
		IDLE,
		INVUL_START,
		INVUL_END,
		ROAR_PREP,
		ROAR_ATTACK,
		BULLETHELL_PREP,
		BULLETHELL_ATTACK
	}
	InternalAttackStates a_states=InternalAttackStates.IDLE;
	SpriteRenderer Skull;
	SpriteRenderer Head;
	SpriteRenderer Body;
	public GameObject spikePrefab;
	public GameObject projectilePrefab;
	public bool Invul;//invul
	public int AttackLeft;//to keep track of the number of attacks to invul/out of invul
	public float IdleTime=2.0f;
	public float RoarPrepTime=1.0f;
	public float SpiritShootInterval = 0.5f;
	public float SpikeSpawnTime;
	int spiritsLeft=0;
	int pattern;
	List<GameObject> spikeList;


	
	string sheetName;
	Sprite[] subsprite;
	string lastSheetName;
	public string InvulStateSprite;
	public string NormalStateSprite;


	protected override void Start ()
	{
		Head = transform.FindChild ("Head").GetComponent<SpriteRenderer>();
		Skull = transform.FindChild ("Skull").GetComponent<SpriteRenderer> ();
		Body = GetComponent<SpriteRenderer> ();
		spikeList = new List<GameObject> ();
		base.Start ();
	}

	protected override void Update ()
	{
		//this entire part is for the sprite changing if there is a bug with sprites displaying wrongly
		//check the sprite names
		//internal sprite sheet names must be the same like lion 1,lion 2
		if (lastSheetName != sheetName) {
			subsprite = Resources.LoadAll<Sprite> ("Monsters/" + sheetName)as Sprite[];

			string bodyName = Body.sprite.name;
			string skullName = Skull.sprite.name;
			string headName = Head.sprite.name;
			if (subsprite != null) {
				Sprite nbody = Array.Find (subsprite, item => item.name == bodyName);
				Sprite nskull = Array.Find (subsprite, item => item.name == skullName);
				Sprite nhead = Array.Find (subsprite, item => item.name == headName);
				if (nbody && nskull && nhead) {
					lastSheetName = sheetName;
					Body.sprite = nbody;
					Skull.sprite = nskull;
					Head.sprite = nhead;
				}
			}
		} else {
			string bodyName = Body.sprite.name;
			string skullName = Skull.sprite.name;
			string headName = Head.sprite.name;
			if (subsprite != null) {
				Sprite nbody = Array.Find (subsprite, item => item.name == bodyName);
				Sprite nskull = Array.Find (subsprite, item => item.name == skullName);
				Sprite nhead = Array.Find (subsprite, item => item.name == headName);
				if (nbody && nskull && nhead) {
					Body.sprite = nbody;
					Skull.sprite = nskull;
					Head.sprite = nhead;
				}
			}
		}
		base.Update ();
	}

	protected override void Attack_State ()
	{
		switch (a_states) {
		case InternalAttackStates.BULLETHELL_ATTACK:

			break;
		case InternalAttackStates.BULLETHELL_PREP:

			break;
		case InternalAttackStates.IDLE:
			if(timeLeft>0){
				timeLeft-=Time.deltaTime;
			}
			else{
				if(AttackLeft>0)
				{
					//int temp=UnityEngine.Random.Range(0,10);//uncomment if i implement the bullet hell thing
					int temp=UnityEngine.Random.Range(0,7);
					if(temp<4)//0-3
					{
						a_states=InternalAttackStates.SPIKE_PREP;
						pattern=temp;
					}
					else if(temp<7)//4-6
					{
						a_states=InternalAttackStates.ROAR_PREP;
						pattern=temp-4;//0-2
						timeLeft=RoarPrepTime;
					}
					else//7-9
					{
						a_states=InternalAttackStates.BULLETHELL_PREP;
						pattern=temp-7;//0-2
					}
				}
				else{
					a_states=InternalAttackStates.INVUL_START;
				}
			}
			break;
		case InternalAttackStates.INVUL_END:
			//shoot spirits towards the player
			if(spiritsLeft>0)
			{
				if(timeLeft<0)
				{
					spiritsLeft-=1;
					timeLeft=SpiritShootInterval;
					//shoot stuff towards the player;
					if(spiritsLeft<=0)
					{
						//play invul deactivate animation
					}
				}
				else
				{
					timeLeft-=Time.deltaTime;
				}
			}
			else
			{
				float t=anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
				if(t>0.4f)
				{
					sheetName=NormalStateSprite;
				}
				if(t<=1)
				{
					a_states=InternalAttackStates.IDLE;
				}
			
			}
			break;
		case InternalAttackStates.ROAR_ATTACK:
			if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime>=1&&timeLeft<0)
			{
				a_states=InternalAttackStates.IDLE;
				timeLeft=IdleTime;
			}
			break;
		case InternalAttackStates.ROAR_PREP:
			if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime>=1&&timeLeft<0)
			{
				a_states=InternalAttackStates.ROAR_ATTACK;
				//instantiate the roar thing
				//roar should destroy itself after it done
			}
			break;
		case InternalAttackStates.SPIKE_ATTACK:
		{
			bool b=false;
			foreach(GameObject spike in spikeList)
			{
				if(spike)//test if its still there
				{
					b=true;
					break;
				}
			}
			if(!b)//if there is no more spikes in the list then attack is over
			{
				spikeList.Clear();
				a_states=InternalAttackStates.IDLE;
				timeLeft=IdleTime;
			}
			break;
		}
		case InternalAttackStates.SPIKE_PREP:
			//instantiate stuff in random positions around the player
			//put them into spikeList
			//spike should destroy themselves after they are done
			//patterns?
			//patterns could include big aoe with alot of spikes in one localised area
			//and a bunch of random ones
			break;
		}
	}

	public override void ApplyDamage (float attack)
	{
		if (!Invul) {
			base.ApplyDamage (attack);
		}
	}
	public override void KnockBack (float amount, Vector2 Dir, float stunDuration)
	{
		//no knock back should be applied to this guy
		//make him flash red or something
	}

	protected override void Idle_State ()
	{

	}

	protected override void Tracking_State ()
	{

	}

	protected override void ChangeState ()
	{
		states = E_States.ATTACK;
	}
}
