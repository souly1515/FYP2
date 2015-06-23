using UnityEngine;
using System.Collections;

public class Charger :  E_Base {
	Vector3 targ;//where its charging
	public Vector3 charVel;
	const float chargeCoolOff=0.50f;//after the attack miss how long it will charge for
	const float prepTime=1.0f;
	public enum Internal_AttackState
	{
		CHARGE_PREP,
		CHARGE,
		CHARGE_SLOW
	}
	public Internal_AttackState a_State=Internal_AttackState.CHARGE_PREP;

	override protected void Start ()
	{
		base.Start ();
		base.detectionRange = 10;
		base.attackRange = 5;
	}

	public override void Attack_State()
	{
		switch (a_State) {
		case Internal_AttackState.CHARGE_PREP:
			if(timeLeft>0)
			{
				timeLeft-=Time.deltaTime;
			}
			else{
				targ=Target.transform.position;
				charVel=targ-gameObject.transform.position;
				charVel=charVel.normalized*5.0f;
				a_State=Internal_AttackState.CHARGE;
			}
			//stop moving and start counting down then switch to charge enemy
			break;
		case Internal_AttackState.CHARGE:
			if(Vector3.Dot(targ-gameObject.transform.position,charVel)>0){//not passed yet
				Vector3 temp=gameObject.transform.position;
				charVel+=charVel.normalized*1.2f*Time.deltaTime;
				gameObject.transform.position=temp+charVel*Time.deltaTime;
			}
			else{//passed the charge pos
				timeLeft=UnityEngine.Random.Range(chargeCoolOff*0.8f,chargeCoolOff*1.2f);
				a_State=Internal_AttackState.CHARGE_SLOW;
			}
			//move at high speed towards the enemy
			//after moving pass the enemy start a countdown before stopping
			//use dot product between moving direction and the direction between enemy and original player pos>0 to determine if it passed the player
			break;
		case Internal_AttackState.CHARGE_SLOW:
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
				a_State=Internal_AttackState.CHARGE_PREP;
				timeLeft=UnityEngine.Random.Range(prepTime*0.8f,prepTime*1.2f);
				//dist=UnityEngine.Random.Range(2,4);
			}
			break;
		}

	}

	protected override void ChangeState ()
	{
		switch (states) {
		case E_States.IDLE_MOVE:
			Target=Physics2D.OverlapCircle(gameObject.transform.position,detectionRange,Tracks);
			if(Target)
			{
				//the moment detection occurs the enemy will charge at the player
				states=E_States.ATTACK;

				//uncomment once done with circle test
				//*
				states=E_States.ATTACK;
				targ=Target.transform.position;
				charVel=targ-gameObject.transform.position;
				charVel=charVel.normalized*5.0f;
				a_State=Internal_AttackState.CHARGE;
				stateChange=true;
				//*/
			}
			break;
		case E_States.ATTACK:
			Target=Physics2D.OverlapCircle(gameObject.transform.position,detectionRange,Tracks);
			if(!Target)
			{
				states=E_States.IDLE_MOVE;
				stateChange=true;
			}
			break;
		}
	}

	public override void Tracking_State ()
	{

	}


}
