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
		REST,//considered idle
		ATTACK,//head extension
		PRE_ATTACK,//head compression
		MOVE,//move towards the player
		PRE_MOVE
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
		if (attackStates == InternalAttackState.MOVE||attackStates==InternalAttackState.PRE_MOVE) {
			targetDir = nextPos - transform.position;
		}
		else
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
			if (anim.GetCurrentAnimatorStateInfo (0).normalizedTime >= 1&&timeLeft<=0&&anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) {
				attackStates = InternalAttackState.REST;
				//set time left
				timeLeft = RestTime;
				attackColB.enabled = false;
				attackColF.enabled = false;
				Attacking = false;
				anim.SetBool ("Attack", false);
			}
			else
				timeLeft-=Time.deltaTime;

			break;
		case InternalAttackState.MOVE:
			{
			AnimatorStateInfo t = anim.GetCurrentAnimatorStateInfo (0);
			if((transform.position - nextPos).sqrMagnitude < 0.5f)
			{
				if(!t.IsTag("WalkLand"))
				{
					anim.SetTrigger("Land");
				}
				else
				{
					anim.SetBool("Walk",false);
					attackStates=InternalAttackState.REST;
				}
			}
			else if(t.IsTag("WalkLand"))
			{
				transform.position = transform.position + (nextPos - transform.position).normalized * (2*(1-t.normalizedTime)) * Time.deltaTime;//slow down as the animations stops
			}
			else if(t.IsTag("WalkMidAir"))
			{
				if (timeLeft > 0) {
					timeLeft -= Time.deltaTime;
					transform.position = transform.position + (nextPos - transform.position).normalized * 2.0f * Time.deltaTime;
				} else {
					anim.SetTrigger ("Land");
				}
			}else if (t.IsTag ("Walk")) {
				timeLeft = 1.0f;
				transform.position = transform.position + (nextPos - transform.position).normalized * (2.0f*t.normalizedTime) * Time.deltaTime;

			}
			else if(t.IsTag("WalkPrep"))
			{
				if(t.normalizedTime>=1)
					anim.SetTrigger("Move");
			}
			/*
				if ((transform.position - nextPos).sqrMagnitude < 0.5f && t.IsTag ("WalkLand")) {
					anim.SetBool ("Walk", false);
					attackStates = InternalAttackState.REST;
					timeLeft = RestTime + 0.5f;
				} else if (t.normalizedTime >= 1 && t.IsTag ("WalkLand")) {
					attackStates = InternalAttackState.PRE_MOVE;
					timeLeft=0.5f;
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
			*/
				break;
			}
		case InternalAttackState.PRE_MOVE:
			if (anim.GetCurrentAnimatorStateInfo (0).normalizedTime >= 1&&timeLeft<=0&&anim.GetCurrentAnimatorStateInfo(0).IsTag("WalkPrep")) {
				attackStates = InternalAttackState.MOVE;
				anim.SetTrigger ("Move");
			}
			else
				timeLeft-=Time.deltaTime;
			break;
		case InternalAttackState.PRE_ATTACK:
			{
			
				AnimatorStateInfo t = anim.GetCurrentAnimatorStateInfo (0);
				//basically i put walk in here cause i was lazy to make another completely different state for this
				anim.SetBool ("Walk", true);
				moving = true;
			Vector3 temp=(transform.position - Target.transform.position);
				if (((transform.position - Target.transform.position).sqrMagnitude <=  1.5f && (t.IsTag ("Idle")||t.IsTag("PrepAttack")))||Attacking) {

					Attacking = true;
					anim.SetBool ("Walk", false);
					anim.SetBool ("AttackPrep", true);
					if (timeLeft > 0 && anim.GetCurrentAnimatorStateInfo (0).normalizedTime < 1) {//test if charge time is over and the charge animation is over as well
						timeLeft -= Time.deltaTime;
					} else {//if it is then go attack
						if(t.IsTag("PrepAttack"))
						{
							if (targetDir.y > 0) {
								attackColB.enabled = true;
							} else {
								attackColF.enabled = true;
							}
							attackStates = InternalAttackState.ATTACK;
							anim.SetBool ("AttackPrep", false);
							anim.SetBool ("Attack", true);
							timeLeft=0.5f;
						}
					}

				}
			else if((transform.position - Target.transform.position).sqrMagnitude <= 1.5f )
			{
				if(!t.IsTag("WalkLand"))
				{
					anim.SetTrigger("Land");
				}
				anim.SetBool("Walk",false);
			}
			else if(t.IsTag("WalkLand"))
			{
				transform.position = transform.position + (Target.transform.position - transform.position).normalized * (2*(1-t.normalizedTime)) * Time.deltaTime;//slow down as the animations stops
			}
			else if(t.IsTag("WalkMidAir"))
			{
				if (timeLeft > 0) {
					timeLeft -= Time.deltaTime;
					transform.position = transform.position + (Target.transform.position - transform.position).normalized * 2.0f * Time.deltaTime;
				} else {
					anim.SetTrigger ("Land");
				}
			}else if (t.IsTag ("Walk")) {
				timeLeft = 1.0f;
				transform.position = transform.position + (Target.transform.position - transform.position).normalized * (2.0f*t.normalizedTime) * Time.deltaTime;
				
			}
			else if(t.IsTag("WalkPrep"))
			{
				if(t.normalizedTime>=1)
					anim.SetTrigger("Move");
			}
			/*
			else if (t.normalizedTime >= 1 && t.IsTag ("WalkLand")) {


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
				//*/
			/*
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
			//*/

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
					attackStates=InternalAttackState.PRE_MOVE;
					anim.SetBool("Walk",true);
					{
						float angle = UnityEngine.Random.Range(0,360);
						Vector3 moveDir=new Vector3();
						moveDir.x=Mathf.Cos (angle); 
						moveDir.y=Mathf.Sin (angle);
						float dist=UnityEngine.Random.Range(0.5f,1.5f)*avgMoveDist;
						nextPos=moveDir*dist;
						originalPos=transform.position;
						timeLeft=0.5f;
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
	
	public override void ApplyDamage (float attack, C_Base c)
	{
		base.ApplyDamage (attack, c);
		if (stats.health <= 0) {
			attackColB.enabled=false;
			attackColF.enabled=false;
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
