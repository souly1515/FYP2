using UnityEngine;
using System.Collections;

public class Slime  :  E_Base{

	public enum InternalAttackState
	{
		ATTACK,
		PRE_MOVE,
		MOVE,
		PRE_ATTACK	
	}
	private Vector3 moveTarg;
	public GameObject projectileType;
	public  float shootInterval = 0.5f;
	public float idleTime=3.0f;
	public float prepTime=1.0f;
	public float chargeSpd=5.0f;
	public float avgMoveDist=2.0f;
	public int avgProj=4;//average projectiles
	public int projLeft = 0;//number of projectiles left to shoot
	float moveProgress = 0.0f;
	float timeMod=0;//used to determine the speed when moving
	float movementspd=3;//movement speed

	public InternalAttackState a_State;


	protected override void Tracking_State ()
	{

	}

	protected override void Attack_State ()
	{
		switch (a_State) {
		case InternalAttackState.ATTACK:
		{
			timeLeft-=Time.deltaTime;
			if(timeLeft<=0)
			{
				//shoot projectile

				//create projectile using instantiate for now
				GameObject temp=Instantiate(projectileType,transform.position,Quaternion.identity)as GameObject;
				Slimeling script=temp.GetComponent<Slimeling>();
				Vector3 playerDir=Target.gameObject.transform.position-gameObject.transform.position;
				float angle=Mathf.Atan2(playerDir.y,playerDir.x);
				angle+=UnityEngine.Random.Range(-0.872f,0.872f);//range is -50 to 50 in rads
				Vector3 projDir=new Vector3();
				projDir.x=Mathf.Cos (angle);
				projDir.y=Mathf.Sin (angle);
				script.velocity=projDir* UnityEngine.Random.Range(0.5f,1.5f)*4.0f;
				//set pojectile velocity using projDir

				//reset timer
				projLeft--;
				if(projLeft>0)
				{
					timeLeft=UnityEngine.Random.Range(shootInterval*0.8f,shootInterval*1.2f);
				}
				else
				{
					timeLeft=idleTime;
					a_State=InternalAttackState.PRE_MOVE;
				}
			}
		}
			break;
		case InternalAttackState.MOVE:
			moveProgress+=Time.deltaTime/timeMod;
			transform.position=Vector3.Lerp(originalPos,moveTarg,moveProgress);
			if((transform.position-moveTarg).sqrMagnitude<0.1)
			{
				timeLeft=UnityEngine.Random.Range(0.8f,1.2f)*prepTime;
				a_State=InternalAttackState.PRE_ATTACK;
			}
			break;
		case InternalAttackState.PRE_ATTACK:
			if(timeLeft<=0)
			{
				projLeft=Mathf.RoundToInt(UnityEngine.Random.Range (0.5f,1.5f)*avgProj);
				a_State=InternalAttackState.ATTACK;
			}
			else
			{
				timeLeft-=Time.deltaTime;
			}
			break;
		case InternalAttackState.PRE_MOVE:
		{
			if(timeLeft<=0)
			{
				float angle = UnityEngine.Random.Range(0,360);
				Vector3 moveDir=new Vector3();
				moveDir.x=Mathf.Cos (angle); 
				moveDir.y=Mathf.Sin (angle);
				float dist=UnityEngine.Random.Range(0.5f,1.5f)*avgMoveDist;
				moveTarg=moveDir*dist;
				originalPos=transform.position;
				moveProgress=0.0f;
				timeMod=(moveTarg-originalPos).magnitude/movementspd;
				a_State=InternalAttackState.MOVE;
			}
			else{
				timeLeft-=Time.deltaTime;
			}
		}
			break;
		}
	}

	protected override void Idle_State ()
	{

	}

	protected override void ChangeState ()
	{
		switch (states) {
		case E_States.IDLE:
			Target=Physics2D.OverlapCircle(gameObject.transform.position,detectionRange,Tracks);
			if(Target)
			{
				//prepare attack
				states=E_States.ATTACK;
				a_State=InternalAttackState.PRE_ATTACK;
			}
			break;
		case E_States.ATTACK:
			Target=Physics2D.OverlapCircle(gameObject.transform.position,detectionRange*1.5f,Tracks);
			if(!Target)
			{
				states=E_States.IDLE;
			}
			break;
		}
	}
}
