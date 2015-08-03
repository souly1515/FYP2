using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class Lion : E_Base {
	public enum InternalAttackStates{
		IDLE,
		SPIKE_ATTACK,
		SPIKE_PREP,
		INVUL_START,
		INVUL_END,
		ROAR_PREP,
		ROAR_ATTACK,
		BULLETHELL_PREP,
		BULLETHELL_ATTACK
	}
	public InternalAttackStates a_states=InternalAttackStates.IDLE;
	SpriteRenderer Skull;
	SpriteRenderer Head;
	SpriteRenderer Body;
	public GameObject spikePrefab;
	public GameObject spiritPrefab;
	public GameObject roarPrefab;
	public GameObject roarProjectilePrefab;
	public bool Invul;//invul
	public int AttackLeft;//to keep track of the number of attacks to invul/out of invul
	public float IdleTime=2.0f;
	public float RoarPrepTime=1.0f;
	float SpiritShootIntervalFixed = 0.5f;
	public float SpiritShootInterval = 0.5f;
	public float SpikeSpawnTime;
	public int spiritsLeft=0;
	int pattern;
	List<GameObject> spikeList;
	List<GameObject> SpiritList;
	Lion_Roar roarScript;
	public Slider healthThing=null;
	//float avgSpikeDist=0.5f;


	
	public string sheetName;
	Sprite[] subsprite;
	public string lastSheetName;
	public string InvulStateSprite;
	public string NormalStateSprite;


	protected override void Start ()
	{
		Head = transform.FindChild ("Head").GetComponent<SpriteRenderer>();
		Skull = transform.FindChild ("Skull").GetComponent<SpriteRenderer> ();
		Body = GetComponent<SpriteRenderer> ();
		spikeList = new List<GameObject> ();
		SpiritList = new List<GameObject> ();
		detectionRange = 1000;
		base.Start ();
		sheetName = NormalStateSprite;
	}

	protected override void Update ()
	{
		healthThing.value = (float)stats.health / (float)stats.maxHealth;
		base.Update ();
	}

	protected override void OnDeath ()
	{
		//dont do anything here
	}

	void LateUpdate()
	{
		
		//this entire part is for the sprite changing if there is a bug with sprites displaying wrongly
		//check the sprite names
		//internal sprite sheet names must be the same like lion 1,lion 2
		if (lastSheetName != sheetName) {
			Sprite[] tSprite = Resources.LoadAll<Sprite> ("Monsters/Lion/" + sheetName)as Sprite[];
			if(tSprite.Length>0)
				subsprite=tSprite; 
			string bodyName = Body.sprite.name;
			string skullName = Skull.sprite.name;
			string headName = Head.sprite.name;
			if(subsprite!=null)
				if (subsprite.Length>0) {
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
					if(Invul)
					{
						a_states=InternalAttackStates.INVUL_END;
						//set spirits left
						//spiritsLeft=0;
					}
					else
						a_states=InternalAttackStates.INVUL_START;
				}
				AttackLeft--;
			}
			break;
		case InternalAttackStates.INVUL_START:
		{
			timeLeft-=Time.deltaTime;
			if(!Invul)
			{
				spiritsLeft=UnityEngine.Random.Range(4,11);
				SpiritShootInterval=10.0f/spiritsLeft*SpiritShootIntervalFixed;
			}
			AnimatorStateInfo t=anim.GetCurrentAnimatorStateInfo(0);
			if(!t.IsTag("transform")&&!Invul)
			{
				anim.SetTrigger("Transform");
			}
			else if((t.normalizedTime>0.9f||!t.IsTag("transform"))&&SpiritList.Count>=spiritsLeft)
			{
				a_states=InternalAttackStates.IDLE;
			}
			else if(t.normalizedTime>0.4f)
			{
				sheetName=InvulStateSprite;
			}
			if(timeLeft<=0)
			{
				if(SpiritList.Count<spiritsLeft)
				{
					GameObject temp=Instantiate(spiritPrefab,transform.position+new Vector3(-0.2f*transform.localScale.x,0.2f,0),Quaternion.identity)as GameObject;
					SpiritAI temp2=temp.GetComponent<SpiritAI>();
					temp2.CircleTarget=transform.position;
					if(transform.localScale.x<0)
					{
						temp2.angle=0.75f;
						temp2.d=false;
					}
					else
					{
						temp2.d=true;
					}
					SpiritList.Add(temp);
					timeLeft=SpiritShootInterval;
				}
			}
			Invul=true;
			AttackLeft=UnityEngine.Random.Range(2,5);
			//play invul animation
		}
			break;
		case InternalAttackStates.INVUL_END:
			//shoot spirits towards the player
			if(spiritsLeft>0)
			{
				if(timeLeft<0)
				{
					foreach(GameObject temp in SpiritList)
					{
						//if(temp.transform.position.y<transform.position.y)
						{
							timeLeft=SpiritShootInterval;
							SpiritAI temp2=temp.GetComponent<SpiritAI>();
							temp2.Shoot(Target.gameObject);
							spiritsLeft--;
							//shoot stuff towards the player;
							if(spiritsLeft<=0)
							{
								anim.SetTrigger("Transform");
								//play invul deactivate animation
							}
							SpiritList.Remove(temp);
							break;
						}
					}

				}
				else
				{
					timeLeft-=Time.deltaTime;
				}
			}
			else
			{
				AnimatorStateInfo t=anim.GetCurrentAnimatorStateInfo(0);
				Invul=false;
				if(!t.IsTag("transform")&&!(AttackLeft>0))//if attack is greater then 0 it means its alr transforming
				{
					//anim.SetTrigger("Transform");
				}
				else if(t.normalizedTime>=0.9f||!t.IsTag("transform"))//so here i can test if its still in the transform and if its not and the attack left is greater then 0 then the transform is over
				{
					a_states=InternalAttackStates.IDLE;
				}
				else if(t.normalizedTime>0.4f)
				{
					sheetName=NormalStateSprite;
				}
				AttackLeft=UnityEngine.Random.Range(3,7);
			
			}
			break;
		case InternalAttackStates.ROAR_ATTACK:
			if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime>=1&&timeLeft<0)
			{
				anim.SetTrigger("RoarEnd");
				a_states=InternalAttackStates.IDLE;
				timeLeft=IdleTime;
				roarScript.RoarOver=true;
			}
			break;
		case InternalAttackStates.ROAR_PREP:
			if(!anim.GetCurrentAnimatorStateInfo(0).IsTag("Roar"))
			{
				anim.SetTrigger("Roar");
			}
			if(!roarScript&&anim.GetCurrentAnimatorStateInfo(0).normalizedTime>0.7f&&anim.GetCurrentAnimatorStateInfo(0).IsTag("Roar"))
			{
				Vector3 pos=new Vector3(-0.5f*transform.localScale.x/Mathf.Abs(transform.localScale.x),1.0f,0);
				GameObject roarObj=Instantiate(roarPrefab,transform.position+pos,Quaternion.identity)as GameObject;
				Vector3 s=roarObj.transform.localScale;
				s.x=transform.localScale.x/(Mathf.Abs(transform.localScale.x));
				roarObj.transform.localScale=s;
				roarScript=roarObj.GetComponent<Lion_Roar>();
			}
			if(timeLeft<0&&roarScript)
			{
				a_states=InternalAttackStates.ROAR_ATTACK;
				//instantiate the roar thing
				//roar should destroy itself after it done
			}
			else
				timeLeft-=Time.deltaTime;
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
				anim.SetTrigger("CastEnd");
				spikeList.Clear();
				a_states=InternalAttackStates.IDLE;
				timeLeft=IdleTime;
			}
			break;
		}
		case InternalAttackStates.SPIKE_PREP:
			if(pattern>=0&&pattern<2)//0-1
			{
				//random attack
				int num=UnityEngine.Random.Range(5,10);
				for(int i=0;i<num;++i)
				{
					//intantiate spike
					float angle = UnityEngine.Random.Range(0,360);
					Vector3 atkDir=new Vector3();
					atkDir.x=Mathf.Cos (angle); 
					atkDir.y=Mathf.Sin (angle);
					float dist=UnityEngine.Random.Range(0.0f,5.0f);
					Vector3 relPos=atkDir*dist;
					float time=UnityEngine.Random.Range(0.5f,2.0f);

					GameObject spike=Instantiate(spikePrefab,Target.transform.position+relPos,Quaternion.identity)as GameObject;
					spike.GetComponent<Lion_Spike>().timeLeft=time;	
					spike.transform.position=Target.transform.position+relPos;
					spikeList.Add(spike);
				}
				anim.SetTrigger("Cast");
				a_states=InternalAttackStates.SPIKE_ATTACK;
			}
			else
			{
				//huge attack
				for(int i=0;i<5;i++)
				{
					float angle = UnityEngine.Random.Range(0,360);
					Vector3 atkDir=new Vector3();
					atkDir.x=Mathf.Cos (angle); 
					atkDir.y=Mathf.Sin (angle);
					float dist=UnityEngine.Random.Range(0.0f,1.0f);
					Vector3 relPos=atkDir*dist;
					float time=UnityEngine.Random.Range(1.0f,2.5f);
					
					GameObject spike=Instantiate(spikePrefab,Target.transform.position+relPos,Quaternion.identity)as GameObject;
					spike.GetComponent<Lion_Spike>().timeLeft=time;	
					//spike.transform.position=Target.transform.position+relPos;
					spikeList.Add(spike);
				}
				anim.SetTrigger("Cast");
				a_states=InternalAttackStates.SPIKE_ATTACK;

			}
			//instantiate stuff in random positions around the player
			//put them into spikeList
			//spike should destroy themselves after they are done
			//patterns?
			//patterns could include big aoe with alot of spikes in one localised area
			//and a bunch of random ones
			break;
		}
	}

	public override void ApplyDamage (float attack,C_Base c)
	{
		if (!Invul) {
			base.ApplyDamage (attack,c);
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
		//Target = Physics2D.OverlapCircle (gameObject.transform.position, detectionRange, Tracks);
	}
}
