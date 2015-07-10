using UnityEngine;
using System.Collections;

public class TadpoleSlimeCharger :  Charger {
	TadpoleSlime_Slime slimeScript;
	GameObject slime;

	override protected void Start ()
	{
		slime = (transform.GetChild (0)).gameObject;
		slimeScript = slime.GetComponent<TadpoleSlime_Slime> ();
		base.Start ();
		base.detectionRange = 10;
		base.attackRange = 5;
		anim = gameObject.GetComponent<Animator> ();
		slimeScript = GetComponentInChildren<TadpoleSlime_Slime> ();
	}

	protected override void Update ()
	{
		base.Update ();
		if (charVel.x > 0) {
			if (left) {
				left = false;
				Vector3 temp=transform.localScale;
				temp.x*=-1;
				transform.localScale = temp;	
			}

		} else {
			if(!left)
			{
				left=true;
				Vector3 temp=transform.localScale;
				temp.x*=-1;
				transform.localScale = temp;	
			}
		}
	}

	protected override void Attack_State()
	{
		anim.SetFloat ("y_speed", charVel.y);
		charVel.z = 0;
		switch (a_State) {
		case InternalAttackState.IDLE:
			if(damageScript.enabled)
				damageScript.enabled=false;
			if(timeLeft>0||!slimeScript.regenerated)
			{
				timeLeft-=Time.deltaTime;
			}
			else{
				targ=Target.transform.position;
				charVel=targ-gameObject.transform.position;
				charVel=charVel.normalized*5.0f;
				timeLeft=UnityEngine.Random.Range(prepTime*0.8f,prepTime*1.2f);
				a_State=InternalAttackState.CHARGE_PREP;
				anim.SetFloat ("y_speed", charVel.y);
				anim.SetBool("Charge_Prep",true);
			}
			//stop moving and start counting down then switch to charge enemy
			break;
		case InternalAttackState.CHARGE:
			if(Vector3.Dot(targ-gameObject.transform.position,charVel)>0){//not passed yet
				Vector3 temp=gameObject.transform.position;
				charVel+=charVel.normalized*1.2f*Time.deltaTime;
				gameObject.transform.position=temp+charVel*Time.deltaTime;
			}
			else{//passed the charge pos
				timeLeft=UnityEngine.Random.Range(chargeCoolOff*0.8f,chargeCoolOff*1.2f);
				a_State=InternalAttackState.CHARGE_SLOW;
			}
			//move at high speed towards the enemy
			//after moving pass the enemy start a countdown before stopping
			//use dot product between moving direction and the direction between enemy and original player pos>0 to determine if it passed the player
			break;
		case InternalAttackState.CHARGE_SLOW:
			timeLeft-=Time.deltaTime;
			Vector3 pos=gameObject.transform.position;
			gameObject.transform.position=pos+charVel*Time.deltaTime;
			if(timeLeft<0)
			{
				charVel=Vector3.Lerp(charVel,Vector3.zero,Time.deltaTime*5);
			}
			else{
				charVel+=charVel.normalized*1.2f*Time.deltaTime;
			}
			if(charVel.sqrMagnitude<0.01)
			{
				a_State=InternalAttackState.IDLE;
				timeLeft=UnityEngine.Random.Range(idleTime*0.8f,idleTime*1.2f);
				anim.SetBool("Charge",false);
				slime.transform.SetParent(transform);
				slime.transform.localPosition=new Vector3(0,0,slime.transform.position.z);
				if(charVel.y>0)
					slimeScript.RegenerateSlime(false);
				else
					slimeScript.RegenerateSlime(true);
				//dist=UnityEngine.Random.Range(2,4);
			}
			break;
		case InternalAttackState.CHARGE_PREP:
			targ=Target.transform.position;
			charVel=targ-gameObject.transform.position;
			charVel=charVel.normalized*5.0f;
			anim.SetFloat ("y_speed", charVel.y);
			timeLeft-=Time.deltaTime;
			if(timeLeft<=0)
			{
				damageScript.enabled=true;
				if(charVel.y>0)
					slimeScript.KillSlime(false);
				else
					slimeScript.KillSlime(true);
				a_State=InternalAttackState.CHARGE;
				anim.SetBool("Charge_Prep",false);
				anim.SetBool("Charge",true);
				slime.transform.SetParent(null);
			}
			break;
		}
		charVel.z = 0;

	}

	protected override void ChangeState ()
	{
		switch (states) {
		case E_States.IDLE:
			Target=Physics2D.OverlapCircle(gameObject.transform.position,detectionRange,Tracks);
			if(Target)
			{
				states=E_States.ATTACK;
				targ=Target.transform.position;
				charVel=targ-gameObject.transform.position;
				charVel=charVel.normalized*5.0f;
				timeLeft=UnityEngine.Random.Range(prepTime*0.8f,prepTime*1.2f);
				a_State=InternalAttackState.CHARGE_PREP;
				anim.SetFloat ("y_speed", charVel.y);
				anim.SetBool("Charge_Prep",true);
			}
			break;
		case E_States.ATTACK:
			Target=Physics2D.OverlapCircle(gameObject.transform.position,detectionRange,Tracks);
			if(!Target)
			{
				states=E_States.IDLE;
				stateChange=true;
			}
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
