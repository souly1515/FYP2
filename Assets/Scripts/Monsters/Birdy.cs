using UnityEngine;
using System.Collections;

public class Birdy : E_Base {
	PolygonCollider2D attackColF;
	PolygonCollider2D attackColB;
	Vector3 nextPos;//for moving
	public float RestTime=2.0f;
	public float  PreAttackTime=1.0f; 
	Vector3 targetDir;
	bool Attacking=false;
	public float avgMoveDist=1.5f;
	bool moving=false;
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

	public InternalAttackState attackStates;

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
				timeLeft = RestTime;
				attackColB.enabled = false;
				attackColF.enabled = false;
				Attacking = false;
				anim.SetBool ("Attack", false);
			}

			break;
		case InternalAttackState.MOVE:
			{
				AnimatorStateInfo t = anim.GetCurrentAnimatorStateInfo (0);
				if ((transform.position - nextPos).sqrMagnitude < 0.5f && t.IsTag ("WalkLand")) {
					anim.SetBool ("Walk", false);
					attackStates = InternalAttackState.REST;
					timeLeft = RestTime + 0.5f;
				} else if (t.normalizedTime >= 1 && t.IsTag ("WalkLand")) {
					attackStates = InternalAttackState.PRE_MOVE;
				} else {
					if (t.IsTag ("WalkMidAir")) {
						if (timeLeft > 0) {
							timeLeft -= Time.deltaTime;
							transform.position = transform.position + (nextPos - transform.position).normalized * 2.0f * Time.deltaTime;
						} else {
							anim.SetTrigger ("Land");
						}
					} else if (t.IsTag ("Walk")) {
						timeLeft = 1.0f;
					}
				}

				break;
			}
		case InternalAttackState.PRE_MOVE:
			if (anim.GetCurrentAnimatorStateInfo (0).normalizedTime >= 1) {
				attackStates = InternalAttackState.MOVE;
				anim.SetTrigger ("Move");
			}
			break;
		case InternalAttackState.PRE_ATTACK:
			{
			
				AnimatorStateInfo t = anim.GetCurrentAnimatorStateInfo (0);
				//basically i put walk in here cause i was lazy to make another completely different state for this
				anim.SetBool ("Walk", true);
				moving = true;
				if ((transform.position - Target.transform.position).sqrMagnitude <= 2.0f && t.IsTag ("WalkLand")) {

					Attacking = true;
					anim.SetBool ("Walk", false);
					anim.SetBool ("AttackPrep", true);
					if (timeLeft > 0 && anim.GetCurrentAnimatorStateInfo (0).normalizedTime < 1) {//test if charge time is over and the charge animation is over as well
						timeLeft -= Time.deltaTime;
					} else {//if it is then go attack
						if (targetDir.y > 0) {
							attackColB.enabled = true;
						} else {
							attackColF.enabled = true;
						}
						attackStates = InternalAttackState.ATTACK;
						anim.SetBool ("AttackPrep", false);
						anim.SetBool ("Attack", true);
					}

				} else if (t.normalizedTime >= 1 && t.IsTag ("WalkLand")) {


				} else {

					if (t.IsTag ("WalkMidAir")) {
						if (timeLeft > 0) {
							timeLeft -= Time.deltaTime;
							transform.position = transform.position + (nextPos - transform.position).normalized * 2.0f * Time.deltaTime;
						} else {
							anim.SetTrigger ("Land");
						}
					} else if (t.IsTag ("Walk")) {
						timeLeft = 1.0f;
					}
				}
			Attacking = true;
			anim.SetBool ("Walk", false);
			anim.SetBool ("AttackPrep", true);
			if (timeLeft > 0 && anim.GetCurrentAnimatorStateInfo (0).normalizedTime < 1) {//test if charge time is over and the charge animation is over as well
				timeLeft -= Time.deltaTime;
			} else {//if it is then go attack
				if (targetDir.y > 0) {
					attackColB.enabled = true;
				} else {
					attackColF.enabled = true;
				}
				attackStates = InternalAttackState.ATTACK;
				anim.SetBool ("AttackPrep", false);
				anim.SetBool ("Attack", true);

			}
		

			break;
		}
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
					anim.SetBool("Walk",true);
					{
						float angle = UnityEngine.Random.Range(0,360);
						Vector3 moveDir=new Vector3();
						moveDir.x=Mathf.Cos (angle); 
						moveDir.y=Mathf.Sin (angle);
						float dist=UnityEngine.Random.Range(0.5f,1.5f)*avgMoveDist;
						nextPos=moveDir*dist;
						originalPos=transform.position;
					}
					//do the random direction thing for the move
					break;
				default:
					attackStates=InternalAttackState.PRE_ATTACK;
					timeLeft=PreAttackTime;
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
		
		switch (states) {
		case E_States.IDLE:
			Target = Physics2D.OverlapCircle (gameObject.transform.position, detectionRange, Tracks);
			if (Target) {
				states = E_States.ATTACK;
				stateChange = false;
			}
			break;
		case E_States.ATTACK:
			if (!Physics2D.OverlapCircle (gameObject.transform.position, detectionRange, Tracks)) {
				states = E_States.IDLE;
				stateChange = true;
			}
			break;
		}
	}

}
