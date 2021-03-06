﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Frog:  E_Base {
	protected BoxCollider2D hitBox;
	protected float timeLeft2 = 0.0f;
	public float JumpAirTime=4.0f;
	public float JumpMovementFreeze=3.0f;
	public float JumpPrepTime=2.0f;
	public float TonguePrepTime=3.0f;
	public float TongueFreezeTime=2.0f;
	public float IdleTime=2.0f;
	public float HeadRestPeriod=1.0f;
	public GameObject Shadow;
	public GameObject Tongue;
	public GameObject TongueB;
	public GameObject Head;
	public GameObject Bubbles;
	public GameObject Dust;
	public SpriteRenderer BodySprite;
	public Vector3 ShadowOffset=new Vector3(-1.5f,-0.5f);
	Vector3 ShadowSize;
	public GameObject Tadpole;
	Animator HeadAnim;
	Animator DustAnim;
	bool HeadRest=false;
	public Vector3 lastLashDir;
	public bool left = true;//which direction its facing
	public bool up = false;//which direction its facing
	DamageOnContact jumpDamage;
	public Slider healthThing;
	public enum InternalAttackState
	{
		IDLE,
		PREP_JUMP,
		PREP_TONGUE_LASH,
		JUMP,
		TONGUE_LASH,
		SPAWN_TADPOLES,
	}
	public InternalAttackState a_State = InternalAttackState.IDLE;

	override protected void Start ()
	{
		healthThing = GameObject.Find ("BossHealth").GetComponent<Slider> ();
		base.Start ();
		anim = gameObject.GetComponent<Animator> ();
		hitBox = gameObject.GetComponent<BoxCollider2D> ();
		Head=transform.FindChild("Head").gameObject;
		Bubbles=transform.FindChild("Bubbles").gameObject;
		Tongue=transform.FindChild("Tongue_F").gameObject;
		TongueB=transform.FindChild("Tongue_B").gameObject;
		Shadow=transform.FindChild("Shadow").gameObject;
		Dust = transform.FindChild ("DustClouds").gameObject;
		BodySprite = gameObject.GetComponent<SpriteRenderer> ();
		jumpDamage = GetComponent<DamageOnContact> ();
		jumpDamage.enabled = false;
		HeadAnim = Head.GetComponent<Animator> ();
		HeadAnim.enabled = false;
		DustAnim = Dust.GetComponent<Animator> ();
		Dust.transform.SetParent (null);
	}

	public override void KnockBack (float amount, Vector2 Dir, float stunDuration)
	{
	}

	protected override void Update ()
	{
		healthThing.value = (float)stats.health / (float)stats.maxHealth;
		timeLeft -= Time.deltaTime;
		timeLeft2 -= Time.deltaTime;
		base.Update ();
		//if (stunned) {
			//HeadAnim.enabled=true;
		//}
	}
	protected override void stunHandle ()
	{
		//states=E_States.ATTACK;
	}
	
	protected override void Attack_State()
	{
		switch (a_State) {
		case InternalAttackState.IDLE:
			HeadAnim.enabled=true;
			if(jumpDamage.enabled)
				jumpDamage.enabled=false;
			if(HeadAnim.GetCurrentAnimatorStateInfo(0).normalizedTime>=1)
			{
				if(up)
				{
					HeadAnim.SetBool("Forward",false);
				}
				else
				{
					HeadAnim.SetBool("Forward",true);
				}
				if(timeLeft<=0)
				{
					//random attack
					//higher chance to do the tongue lash attack
					//normal chance to jump
					//low chance to spawn tadpoles
					
					//*
					HeadAnim.enabled=false;
					int chance=UnityEngine.Random.Range(0,10);//random here
					if(chance<5)
					{
						a_State=InternalAttackState.PREP_TONGUE_LASH;
						timeLeft=TonguePrepTime;
						timeLeft2=TongueFreezeTime;
						anim.SetBool("FrogLashPrep",true);
						//anim.SetBool("JumpPrep",false);
					}
					else if(chance >=5&&chance<=7)//5-7 jump attack
					{
						a_State=InternalAttackState.PREP_JUMP;
						timeLeft=JumpPrepTime;
						anim.SetBool("JumpPrep",true);
					}
					else{
						a_State=InternalAttackState.SPAWN_TADPOLES;
						anim.SetBool("SpawnTadpoles",true);
					}
					//*/
				}
				else if(HeadRest)
				{
					timeLeft2-=Time.deltaTime;
					if(timeLeft2<=0)
					{
						int a=UnityEngine.Random.Range(1,3);
						HeadAnim.SetInteger("AnimNum",a);
						HeadRest=false;
					}
				}
				else
				{
					HeadRest=true;
					timeLeft2=HeadRestPeriod;
					HeadAnim.SetInteger("AnimNum",0);
				}
			}
			break;
		case InternalAttackState.JUMP:
		{
			if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime>=1)
			{
				Shadow.transform.localScale=Vector3.Lerp(Vector3.zero,ShadowSize,(JumpAirTime-timeLeft)/JumpAirTime);
				Vector3 dir=Target.transform.position-transform.position;
				if(timeLeft<=0)
				{
					BodySprite.enabled=true;
					jumpDamage.enabled=true;
					Head.SetActive(true);
					Bubbles.SetActive(true);
					hitBox.enabled=true;
					a_State=InternalAttackState.IDLE;
					timeLeft=IdleTime;
					if(dir.x>0)
					{
						left=false;
						Vector3 scale=transform.localScale;
						if(dir.y<0)
						{
							if(scale.x>0)
							{
								scale.x*=-1;
								transform.localScale=scale;
							}
						}
						else
						{
							if(scale.x<0)
							{
								scale.x*=-1;
								transform.localScale=scale;
							}
						}
					}
					else{
						left=true;
						Vector3 scale=transform.localScale;
						if(dir.y<0)
						{
							if(scale.x<0)
							{
								scale.x*=-1;
								transform.localScale=scale;
							}
						}
						else
						{
							if(scale.x>0)
							{
								scale.x*=-1;
								transform.localScale=scale;
							}
						}
					}
					if(dir.y>0)
					{
						up=true;
						anim.SetBool("Forward",false);
					}
					else 
					{
						up=false;
						anim.SetBool("Forward",true);
					}
					anim.SetBool("Jump",false);
					DustAnim.SetTrigger("Land");
					Vector3 tempTran=transform.position;
					tempTran.y-=0.1f;
					Dust.transform.position=tempTran;
				}
				else if(timeLeft2>0)
				{
					dir+=ShadowOffset;
					if(dir.sqrMagnitude>0.001)//if its too close dont bother moving
					{
						dir.Normalize();
						transform.position+=dir*Time.deltaTime*3.0f;
					}
				}
			}
		}
			break;
		case InternalAttackState.PREP_JUMP:
			if(timeLeft<=0)
			{
				hitBox.enabled=false;//make the frog invulnerable cause there is not hitbox for the skill to hit;
				timeLeft=JumpAirTime;
				timeLeft2=JumpMovementFreeze;
				Shadow.SetActive(true);
				ShadowSize=Shadow.transform.localScale;
				Shadow.transform.localScale=new Vector3(0,0,1);
				BodySprite.enabled=false;
				Head.SetActive(false);
				Bubbles.SetActive(false);
				a_State=InternalAttackState.JUMP;
				anim.SetBool("JumpPrep",false);
				anim.SetBool("Jump",true);
				DustAnim.SetTrigger("Jump");
				Vector3 tempTran=transform.position;
				tempTran.y-=0.1f;
				Dust.transform.position=tempTran;
			}
			break;
		case InternalAttackState.TONGUE_LASH:
			//play animation;
			if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime>=1)
			{
				if(up)
				{
					TongueB.SetActive(false);
				}
				else{
					Tongue.SetActive(false);
				}
				a_State=InternalAttackState.IDLE;
				timeLeft=IdleTime;
				anim.SetBool("FrogLash",false);
			}
			break;
		case InternalAttackState.PREP_TONGUE_LASH:
			if(timeLeft<=0)
			{
				anim.SetBool("FrogLash",true);
				anim.SetBool("FrogLashPrep",false);
				if(up)
				{
					TongueB.SetActive(true);
					//deal damage here
				}
				else
				{
					Tongue.SetActive(true);
					//deal damage here
				}
				a_State=InternalAttackState.TONGUE_LASH;
			}
			else if(timeLeft2>0)
			{
				Vector3 dir=Target.transform.position-transform.position;
				if(left)
				{
					if(dir.x>0)
					{
						a_State=InternalAttackState.PREP_JUMP;
						timeLeft=JumpPrepTime;
						anim.SetBool("FrogLashPrep",false);
						anim.SetBool("JumpPrep",true);
					}
				}
				else{
					if(dir.x<0)
					{
						a_State=InternalAttackState.PREP_JUMP;
						timeLeft=JumpPrepTime;
						anim.SetBool("FrogLashPrep",false);
						anim.SetBool("JumpPrep",true);
					}
				}
				if(up)
				{
					if(dir.y<0)
					{
						a_State=InternalAttackState.PREP_JUMP;
						timeLeft=JumpPrepTime;
						anim.SetBool("FrogLashPrep",false);
						anim.SetBool("JumpPrep",true);
					}
				}
				else{
					if(dir.y>0)
					{
						a_State=InternalAttackState.PREP_JUMP;
						timeLeft=JumpPrepTime;
						anim.SetBool("FrogLashPrep",false);
						anim.SetBool("JumpPrep",true);
					}
				}
				if((Target.gameObject.transform.position-transform.position).sqrMagnitude>100)
				{
					a_State=InternalAttackState.PREP_JUMP;
					timeLeft=JumpPrepTime;
					anim.SetBool("FrogLashPrep",false);
					anim.SetBool("JumpPrep",true);
				}
				dir.Normalize();
				float angle=Mathf.Atan(dir.y/dir.x);//not using atan2 cause i dont want quadrant info
				angle*=180/3.142f;
				if(!up)
				{
					angle*=75.0f/90.0f;
					if(!left)
					{
						angle+=25;
						angle=-angle;//applies the 25 inside alr
					}
					else
					{
						angle-=25;
					}
						if(angle<0)
						angle+=360;
					Tongue.transform.eulerAngles=new Vector3(0,0,angle);
				}
				else
				{
					angle*=75.0f/90.0f;//note to future people if any
					//get the artist to draw a back version of the tongue
					if(angle>45)
					{
						a_State=InternalAttackState.PREP_JUMP;
						timeLeft=JumpPrepTime;
					}
					if(left)
					{
						angle+=25;
						angle=-angle;
					}
					else
					{
						angle-=25;
					}
					if(angle<0)
						angle+=360;
					TongueB.transform.eulerAngles=new Vector3(0,0,angle);
				}
			}
			break;
		case InternalAttackState.SPAWN_TADPOLES:
		{
			int num=UnityEngine.Random.Range(3,7);
			GameObject temp;
			for(int i=0;i<num;++i)
			{
				float angle=UnityEngine.Random.Range(0,6.283f);
				Vector3 pos=new Vector3();
				pos.x=Mathf.Cos(angle)*12;
				pos.y=Mathf.Sin (angle)*12;

				temp=Instantiate(Tadpole,pos,Quaternion.identity)as GameObject;
				Charger s=temp.GetComponent<Charger>();
				s.targ=Target.transform.position;
				s.a_State=Charger.InternalAttackState.CHARGE;
				s.states=E_States.ATTACK;
				s.charVel=s.targ-s.gameObject.transform.position;
				s.charVel=s.charVel.normalized*5.0f;

			}
			a_State=InternalAttackState.IDLE;
		}
			break;
		}

	}

	protected override void ChangeState ()
	{
		switch (states) {
		case E_States.IDLE:
			states=E_States.ATTACK;
			break;
		case E_States.TRACKING:
			states=E_States.ATTACK;
			break;
		}
	}

	protected override void Tracking_State ()
	{

	}

	protected override void Idle_State ()
	{

	}

}
