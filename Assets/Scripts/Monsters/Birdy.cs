using UnityEngine;
using System.Collections;

public class Birdy : E_Base {
	PolygonCollider2D attackColF;
	PolygonCollider2D attackColB;
	Vector3 nextPos;//for moving
	float RestTime=2.0f;
	Vector3 targetDir;
	bool Attacking=false;
	public enum InternalAttackState
	{
		ATTACK,//head extension
		PRE_ATTACK,//head compression
		MOVE,//move towards the player
		PRE_MOVE,
		REST//considered idle
	}

	protected override void Start ()
	{
		base.Start ();
		GameObject temp = transform.FindChild ("Head").gameObject;
		attackColF = temp.GetComponent<PolygonCollider2D> ();
		temp = transform.FindChild ("HeadB").gameObject;
		attackColB = temp.GetComponent<PolygonCollider2D> ();
		attackColB.enabled = false;
		attackColF.enabled = false;
	}

	InternalAttackState attackStates;

	protected override void Attack_State ()
	{
		targetDir = Target.gameObject.transform.position - transform.position;
		targetDir.Normalize ();
		if (targetDir.x > 0) {
			if (transform.localScale.x > 0) {
				Vector3 temp = transform.localScale;
				temp.x *= -1;
				transform.localScale = temp;
			}
		} else {
			if(transform.localScale.x<0)
			{
				Vector3 temp = transform.localScale;
				temp.x *= -1;
				transform.localScale = temp;
			}
		}
		if (targetDir.y > 0) {
			anim.SetBool ("Forward", false);
		} else {
			anim.SetBool("Forward",true);
		}

		switch (attackStates) {
		case InternalAttackState.ATTACK:
			if (anim.GetCurrentAnimatorStateInfo (0).normalizedTime >= 1) {
				attackStates = InternalAttackState.REST;
				//set time left
				timeLeft=RestTime;
				attackColB.enabled=false;
				attackColF.enabled=false;
				Attacking=false;
			}

			break;
		case InternalAttackState.MOVE:
			if ((transform.position - nextPos).sqrMagnitude < 0.5f) {
				attackStates=InternalAttackState.REST;
				timeLeft=RestTime;
			}
			else if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime>=1)
			{
				attackStates=InternalAttackState.PRE_MOVE;
			}
			else
			{
				transform.position=transform.position+(transform.position-nextPos).normalized*2.0f*Time.deltaTime;
			}

			break;
		case InternalAttackState.PRE_MOVE:

			break;
		case InternalAttackState.PRE_ATTACK:
			if((transform.position-Target.transform.position).sqrMagnitude>2.0f&&!Attacking)
			{
				anim.SetBool("Walk",true);
				if(timeLeft<=0)//not in rest mode
				{
					if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime<1)//means that it haven't jumped finish?
					{						
						transform.position=transform.position+(transform.position-Target.transform.position).normalized*2.0f*Time.deltaTime;
					}
					else
					{
						timeLeft=0.2f;
					}
				}
				else{//in rest mode
					if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime>=1)//prepare to jump again
					{
						timeLeft=0;
						//timeLeft-=Time.deltaTime;
					}
				}
			}
			else
			{
				Attacking=true;
				anim.SetBool("Walk",false);
				anim.SetBool("AttackPrep",true);
				if(timeLeft>0&&anim.GetCurrentAnimatorStateInfo(0).normalizedTime<1)//test if charge time is over and the charge animation is over as well
				{
					timeLeft-=Time.deltaTime;
				}
				else//if it is then go attack
				{
					if(targetDir.y>0)
					{
						attackColB.enabled=true;
					}
					else
					{
						attackColF.enabled=true;
					}
					attackStates=InternalAttackState.ATTACK;

				}
			}


			break;
		case InternalAttackState.REST:
			if(timeLeft>0)
			{
				timeLeft-=Time.deltaTime;
			}
			else
			{
				int chance = UnityEngine.Random.Range(0,5);
				switch(chance)
				{
				case 0:
					attackStates=InternalAttackState.MOVE;
					//do the random direction thing for the move
					break;
				default:
					attackStates=InternalAttackState.PRE_ATTACK;
					//set timer for the attack;
					break;
				}
			}
			break;
		}
	}

	protected override void Idle_State ()
	{

	}

	protected override void Tracking_State ()
	{

	}

	protected override void ChangeState ()
	{

	}

	protected override void Update ()
	{
		base.Update ();
	}
}
