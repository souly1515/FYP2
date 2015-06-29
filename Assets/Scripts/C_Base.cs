using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class C_Stats
{
	public int Str;//strength
	public int Dex;//dexterity
	public int Int;//intelligence
	public int End;//endurance
	public int Wil;//willpower, potentially haki thing
	public int moveSpd;//move speed
}

public class buffs
{
	public enum buffTypes
	{
		SLOW,
		DAMAGE,
		DOT,//damage over time
		STUN
	}
	public float effect;//how powerful the buff is
	public float duration;
	public buffTypes type;
	public string buffName;
	static public Dictionary<string,int> stackDic = new Dictionary<string, int> ();
}

public class C_Base: MonoBehaviour {
	public List<buffs> buffList=new List<buffs>();
	public Vector2 nextPos;
	public C_Stats stats;
	public float moveSpd=4;//original move speed
	public float moveSpdModded;//after modifications
	public float speedMod=1.0f;
	public bool moving=false;
	public GameObject skillPrefab;
	Rigidbody2D rb;
	Animator anim;
	float atkDurationLeft;
	public bool inAttackAnimation;
	//bool left;
	public Vector3 direction;
	public Vector3 lastDir;
	public Weapon myWeap;

	public int weaponType;
	int attackType;

	//debug
	//end of debug

	//inventory

	//equipment

	//

	public void SetNextPos(Vector2 newPos) {
		nextPos = newPos;
	}

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		//bool left = false;
		direction = new Vector3 ();
	}

	void OnCollisionEnter(Collision col)
	{
			//moving = false;
	}
	bool ProcBuffs()
	{
		bool returnvalue = true;
		moveSpdModded = moveSpd;
		speedMod = 1.0f;
		foreach (buffs buff in buffList)
		{
			buff.duration-=Time.deltaTime;
			if(buff.duration<=0)
			{
				//int stackNum;
				buffs.stackDic[buff.buffName]--;
				buffList.Remove(buff);
			}
			else{
				switch(buff.type)
				{
				case buffs.buffTypes.SLOW:
					if(buffs.stackDic[buff.buffName]<=8)
					{
						moveSpdModded-=moveSpd*buff.effect;
						speedMod-=buff.effect;
					}
					else{
						//damage player
						//then remove the buff;
						//reasons is because i want to damage the player once and then remove the earliest instance of the buff
						//this way should remove the earliest instance and change it to a damage buff instead
						buffs.stackDic[buff.buffName]--;
						buff.buffName="slimeDamage";
						buff.type=buffs.buffTypes.DAMAGE;
						if(buffs.stackDic.ContainsKey(buff.buffName))
						{
							buffs.stackDic[buff.buffName]++;
						}
						else{
							buffs.stackDic.Add(buff.buffName,1);
						}
					}
					break;
				case buffs.buffTypes.STUN:
					returnvalue=false;
					break;
				case buffs.buffTypes.DOT:
						
					break;
				case buffs.buffTypes.DAMAGE:
					//damage=effect*effect damage that will increase exponentially
					//probably effect will be 1.3 - 1.8
					break;
				}

			}
		}
		return returnvalue;
	}
	// Update is called once per frame
	void Update () {
		//basic mechanics: movement with mouse
		if (!ProcBuffs())//basically stunned
			return;
		if (inAttackAnimation) {
			rb.velocity=new Vector2(0,0);
			if(atkDurationLeft<=0)
			{
			


				nextPos=transform.position;
				inAttackAnimation=false;
			}
			else{
				atkDurationLeft-=Time.deltaTime;
			}
		}
		else if (moving) {
			direction=(Vector3)(nextPos)-transform.position;
			direction.Normalize();

			if(direction.sqrMagnitude>0.5)
			{
				lastDir=direction;
			}

			rb.velocity=direction*moveSpdModded;

			Vector2 temp2=(Vector2)(transform.position)-nextPos;
			if(temp2.SqrMagnitude()<0.1)
			{
				moving=false;
				rb.velocity=new Vector2(0,0);
			}

			//basic mechanics: camera movement with character
			Vector3 temp = transform.position;
			temp.z = -10;
			//Camera.main.transform.position = Vector3.Lerp (Camera.main.transform.position, temp, cameraSpd);

			float xDif = temp.x - Camera.main.transform.position.x;

			if (Mathf.Abs (xDif) < 5) {
				xDif = 0;
			} else if (xDif > 0) {
				xDif -= 5;
			} else {
				xDif += 5;
			}

			float yDif = temp.y - Camera.main.transform.position.y;

			if (Mathf.Abs (yDif) < 3) {
				yDif = 0;
			} else if (yDif > 0) {
				yDif -= 3;
			} else {
				yDif += 3;
			}
			Camera.main.transform.position += new Vector3 (xDif, yDif, 0);
			//end of basic mechanics: camera movement with character

			//end of basic mechanics: movement with mouse
		}
		var statename = anim.GetCurrentAnimatorStateInfo (0);
		if (statename.IsName ("C_Run_B") || statename.IsName ("C_Run_F") || statename.IsName ("C_Run_Fflip") || statename.IsName ("C_Run_Bflip")) {
			anim.speed = speedMod;
		} else {
			anim.speed=1.0f; 
		}
		anim.SetFloat("y_Velocity",direction.y);
		anim.SetFloat ("x_Velocity", direction.x);
		AnimatorStateInfo AState = anim.GetCurrentAnimatorStateInfo (anim.GetLayerIndex ("Base Layer"));
		/*if (AState.IsName ("C_Run_B") || AState.IsName ("C_Run_F")) {
			if (direction.x < 0) {
				left = false;
				Vector3 tempScale = transform.localScale;
				if (tempScale.x < 0)
					tempScale.x *= -1;
				transform.localScale = tempScale;
			} else if (direction.x > 0) {
				left = true;
				Vector3 tempScale = transform.localScale;
				if (tempScale.x > 0)
					tempScale.x *= -1;
				transform.localScale = tempScale;
			}
		}*/
	}

	public void attackAnimation()
	{
		anim.SetTrigger("attack");
		Vector3 tempScale=transform.localScale;
		if (tempScale.x > 0)
			tempScale.x *= -1;
		transform.localScale = tempScale;
	}

	public void Attack(int type,Vector2 mousePos)
	{
		direction=-Camera.main.WorldToScreenPoint(transform.position)+(Vector3)mousePos;
		direction.Normalize ();
		switch(weaponType)
		{
		case 1:
			SwordAttack(type);
			break;
		case 2:
			RangedAttack(type);
			break;
		}
		//remove once proper skills are implemented
		inAttackAnimation = true;
		atkDurationLeft = 0.5f;

		//find a way to change the sprite at run time;
		Vector2 Dir = ( Vector2 )(direction);

	}

	void SwordAttack(int type)
	{
		switch (type) {
		case 1:
			//direct.z = direction.y/ direction.x * 180 / 3.142f;
			GameObject skillObj=Instantiate (skillPrefab, transform.position+direction,Quaternion.Euler(new Vector3(0,0,Mathf.Atan2(direction.y,direction.x)*180f/3.142f))) as GameObject;
			
			Skills skill=skillObj.GetComponent<Skills>();
			
			skillsInfo temp=new skillsInfo();
			temp.damage=2;
			temp.castTime=0.4f;
			temp.rangeY=10;
			temp.rangeX=3;
			temp.isProjectile=false;
			temp.knockback=10;
			skill.SetInfo(temp);
			skill.Dir = direction;
			break;
		case 2:

			break;
		case 3:

			break;
		case 4:

			break;
		}
	}

	void RangedAttack(int type)
	{
		switch (type) {
		case 1:
			//direct.z = direction.y/ direction.x * 180 / 3.142f;
			GameObject skillObj=Instantiate (skillPrefab, transform.position+direction,Quaternion.Euler(new Vector3(0,0,Mathf.Atan2(direction.y,direction.x)*180f/3.142f))) as GameObject;
			
			Skills skill=skillObj.GetComponent<Skills>();
			
			skillsInfo temp=new skillsInfo();
			temp.damage=1;
			temp.castTime=2.0f;
			temp.rangeY=3;
			temp.rangeX=3;
			temp.knockback=1;
			temp.velocity=direction*5;
			temp.isProjectile=true;
			skill.SetInfo(temp);
			skill.Dir = direction;
			break;
		}
	}
}
