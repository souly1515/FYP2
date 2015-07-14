using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class C_Stats
{

	[SerializeField]
	private int level=1;
	public int Level{
		get{
			return level;
		}
		private set
		{
			level=value;
		}
	}
	[SerializeField]
	private int exp;
	public int Exp{
		get
		{
			return exp;
		}
		private set
		{
			exp=value;
		}
	}
	[SerializeField]
	private int str;
	public int Str{
		get
		{
			return str;
		}
		private set
		{
			str=value;
		}
	}//strength
	
	[SerializeField]
	private int dex;
	public int Dex{
		get{
			return dex;
		}
		private set
		{
			dex=value;
		}
	}//dexterity
	
	[SerializeField]
	private int intelligence;
	public int Int{
		get{
			return intelligence;
		}
		private set
		{
			intelligence=value;
		}
	}//intelligence

	
	[SerializeField]
	private int end;
	public int End {
		get{
			return end;
		}
		private set
		{
			end=value;
		}
	}//endurance
	[SerializeField]
	private float movespd;
	public float moveSpd{
		get{
			return movespd;
		}
		private set
		{
			movespd=value;
		}
	}//move speed
	[SerializeField]
	private float health=1;
	public float Health{
		get
		{
			return health;
		}
		set{
			if(value>MaxHealth)
			{
				health=MaxHealth;
			}
			else
				health=value;
		}
	}
	[SerializeField]
	private float maxHealth=1;
	public float MaxHealth{
		get{
			return maxHealth;
		}
		private set
		{
			maxHealth=value;
		}
	}
	[SerializeField]
	private float mana=1;
	public float Mana {
		get{
			return mana;
		}
		set{
			if(value>MaxMana)
			{
				mana=MaxMana;
			}
			else
				mana=value;
		}
	}
	[SerializeField]
	private float maxMana=1;
	public float MaxMana{
		get{
			return maxMana;
		}
		private set
		{
			maxMana=value;
		}
	}
	
	[SerializeField]
	private int statpoints;

	public int statPoints {
		get;
		private set;
	}
	
	const int baseHealth=200;
	const int baseMana=100;
	const float baseMs=3.7f;
	void Add(int type)
	{
		if (statPoints <= 0)
			return;
		int statNeeded = 0;
		switch (type) {
		case 1:
			statNeeded=GetPointsNeeded(Str);
			if(statPoints>=statNeeded)
			{
				Str+=1;
				statPoints-=statNeeded;
			}
			else
				return;
			break;
		case 2:
			statNeeded=GetPointsNeeded(Dex);
			if(statPoints>=statNeeded)
			{
				Dex+=1;
				statPoints-=statNeeded;
			}
			break;
		case 3:
			statNeeded=GetPointsNeeded(Int);
			if(statPoints>=statNeeded)
			{
				Int+=1;
				statPoints-=statNeeded;
			}
			break;
		case 4:
			statNeeded=GetPointsNeeded(End);
			if(statPoints>=statNeeded)
			{
				End+=1;
				statPoints-=statNeeded;
			}
			break;
		}
		RecalStats ();
	}
	public void SetExp(int exp)
	{
		Exp = exp;
		CalcLevel ();
	}

	void CalcLevel()
	{
		//set exp to current level
	}

	int expToLevel()
	{
		//return the amount of exp needed to level up
		return 0;
	}

	void AddExp(int amount)
	{
		Exp += amount;
		int expthing = expToLevel ();
		while (Exp > expthing) {
			Exp-=expthing;
			Level++;
		}
	}

	void AddStatPoints()//adds stat points based on current level
	{
		int bonus = Mathf.FloorToInt(Mathf.Pow ((Level - 2), 1.2f));
		if (bonus < 0)
			bonus = 0;
		statPoints += 5 + bonus;
	}

	public int GetPointsNeeded(int num)
	{
		return Mathf.FloorToInt (Mathf.Pow (num / 8, 1.3f)); 
	}
	public void RecalStats()
	{
		float percentageHealth = Health / MaxHealth;
		float percentageMana = Mana / MaxMana;
		MaxHealth = baseHealth + (0.5f * Str)+(3.0f*End)+Level*2.2f;
		Health = (MaxHealth * percentageHealth);
		MaxMana = baseMana + 4 * Int+Level*1;
		Mana = (MaxMana * percentageMana);
		moveSpd = baseMs + Dex * 0.05f;

		//calculate level base on exp
	}
	public void InitStats()
	{
		Health = MaxHealth = Mana = MaxMana=1;
	}
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
	public float moveSpdModded;//after modifications
	public float speedMod=1.0f;
	public bool moving=false;
	Rigidbody2D rb;
	Animator anim;
	public bool inAttackAnimation;
	//bool left;
	public Vector3 direction;
	public Vector3 lastDir;
	public string SkillPath="Skills/";
	public int weaponType;
	public float  InvincTime=0.5f;
	public float InvincTimeLeft;
	GameObject skillGO=null;
	Skills skillScript=null;
	public bool dead=false;
	float deathTimeLeft;
	public float deathTime=3.0f;


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
		int temp=PlayerPrefs.GetInt("PlayerHealth");
		if (temp == 0)
			stats.Health = 300;
		else
			stats.Health = temp;
		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		//bool left = false;
		direction = new Vector3 ();
		stats.InitStats ();
		stats.RecalStats ();
	}

	public void Damage(int damage)
	{
		if (InvincTimeLeft > 0)
			return;
		InvincTimeLeft = InvincTime;
		stats.Health -= damage;
		if (stats.Health <= 0) {
			dead=true;
			PlayerPrefs.SetInt("PlayerHealth",0);
			anim.SetTrigger("Death");
			deathTimeLeft=deathTime;

		}
	}

	bool ProcBuffs()
	{
		bool returnvalue = true;
		moveSpdModded = stats.moveSpd;
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
						moveSpdModded-=stats.moveSpd*buff.effect;
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
						Damage(buffs.stackDic[buff.buffName]);
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
		if (dead) {
			if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime>=0)
			{
				if(deathTimeLeft<=0)
					Application.LoadLevel("Level_1");
				else
					deathTimeLeft-=Time.deltaTime;
			}
			return;
		
		}
		InvincTimeLeft -= Time.deltaTime;
		//basic mechanics: movement with mouse
		if (!ProcBuffs())//basically stunned
			return;
		if (inAttackAnimation) {
			if (skillScript != null) {
				if (!skillScript.SkillOver) {
					rb.velocity = new Vector2 (0, 0);
					nextPos = transform.position;
					inAttackAnimation = true;
				} else
				{
					anim.SetTrigger("EndAttack");
					inAttackAnimation = false;
					skillGO.transform.SetParent(null);
				}
			}
			else
			{
				anim.SetTrigger("EndAttack");
				inAttackAnimation=false;
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
				anim.SetBool("Moving",false);
			}
			else
			{
				anim.SetBool("Moving",true);
			}

			//basic mechanics: camera movement with character
			Vector3 temp = transform.position;
			temp.z = -10;
			//Camera.main.transform.position = Vector3.Lerp (Camera.main.transform.position, temp, cameraSpd);

			float xDif = temp.x - Camera.main.transform.position.x;

			if (Mathf.Abs (xDif) < 2.5f) {
				xDif = 0;
			} else if (xDif > 0) {
				xDif -= 2.5f;
			} else {
				xDif += 2.5f;
			}

			float yDif = temp.y - Camera.main.transform.position.y;

			if (Mathf.Abs (yDif) < 1.5f) {
				yDif = 0;
			} else if (yDif > 0) {
				yDif -= 1.5f;
			} else {
				yDif += 1.5f;
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
		if (direction.y > 0) {
			anim.SetBool ("Forward", false);
		} else {
			anim.SetBool("Forward",true);
		}
		if (direction.x > 0) {
			anim.SetBool ("Left", false);
		} else {
			anim.SetBool("Left",true);
		}
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


	public void Attack(int type,int weaponType,Vector2 mousePos)
	{
		if (!inAttackAnimation) {
			direction = -Camera.main.WorldToScreenPoint (transform.position) + (Vector3)mousePos;
			direction.Normalize ();
			switch(weaponType)
			{
			case 1:
				SwordAttack (type, direction);
				break;
			case 2:
				DefaultAttack(type,direction);
				break;
			}
			//remove once proper skills are implemented
			inAttackAnimation = true;

			//find a way to change the sprite at run time;
			Vector2 Dir = (Vector2)(direction);
		}

	}

	void DefaultAttack(int type,Vector3 AttackDir)
	{
		direction = AttackDir;
		anim.SetTrigger ("attack");
		anim.SetInteger ("AtkType", type);
		inAttackAnimation = true;
		GameObject go = null;
		GameObject skillObj = null;
		skillObj = transform.FindChild ("Skill_Default").gameObject;
		Animator s_Anim = skillObj.GetComponent<Animator> ();
		if (direction.y > 0)
			s_Anim.SetBool ("Forward", false);
		else
			s_Anim.SetBool ("Forward", true);

		if (direction.x > 0)
			s_Anim.SetBool ("Left", false);
		else
			s_Anim.SetBool ("Left", true);

		s_Anim.SetTrigger ("Attack");
		s_Anim.SetInteger ("type", type);
	}

	void SwordAttack(int type,Vector3 AttackDir)
	{
		direction = AttackDir;
		anim.SetTrigger("attack");
		anim.SetInteger ("AtkType", 1);
		inAttackAnimation = true;
		GameObject go=null;
		GameObject skillObj = null;
		if(type!=1)
			go=Resources.Load(SkillPath+"Fire_"+type.ToString())as GameObject;
		switch (type) {
		case 1:
		{
			//direct.z = direction.y/ direction.x * 180 / 3.142f;
			skillsInfo temp=new skillsInfo();
			temp.damage=4;
			temp.castTime=0.4f;
			temp.isProjectile=false;
			temp.knockback=5.0f;
			temp.ConstantDam=false;
			if(AttackDir.y<0)
			{
				go=Resources.Load (SkillPath+"Fire_1F")as GameObject;
				temp.skillName=SkillPath+"Fire_1F";
			}
			else
			{
				go=Resources.Load (SkillPath+"Fire_1B")as GameObject;
				temp.skillName=SkillPath+"Fire_1B";
			}
			skillObj=Instantiate (go, transform.position,Quaternion.identity) as GameObject;
			if(AttackDir.x<0)
			{
				Vector3 scale=skillObj.transform.localScale;
				scale.x=-scale.x;
				skillObj.transform.localScale=scale;
			}
			Skills skill=skillObj.GetComponent<Skills>();

			skill.SetInfo(temp);
			skill.Dir = direction;
		}
			break;
		case 2:
		{

			//direct.z = direction.y/ direction.x * 180 / 3.142f;
			skillObj=Instantiate (go, transform.position,Quaternion.identity) as GameObject;
			Skills skill=skillObj.GetComponent<Skills>();
			if(AttackDir.x>0)
			{
				
			}
			
			skillsInfo temp=new skillsInfo();
			temp.damage=1;
			temp.castTime=0.5f;
			temp.isProjectile=false;
			temp.knockback=0.2f;
			temp.ConstantDam=true;
			temp.skillName=SkillPath+"Fire_"+type.ToString();
			skill.SetInfo(temp);
			skill.Dir = direction;

		}
			break;
		case 3:
		{
			//direct.z = direction.y/ direction.x * 180 / 3.142f;
			skillObj=Instantiate (go, transform.position+direction*1,Quaternion.identity) as GameObject;
			Skills skill=skillObj.GetComponent<Skills>();
			
			skillsInfo temp=new skillsInfo();
			temp.damage=2;
			temp.castTime=0.1f;
			temp.isProjectile=true;
			temp.pierceNumber=5;
			temp.knockback=0.2f;
			temp.ConstantDam=false;
			temp.skillName=SkillPath+"Fire_"+type.ToString();
			skill.SetInfo(temp);
			skill.Dir = direction;
		}
			break;
		case 4:

			break;
		}
		skillGO = skillObj;
		skillScript = skillGO.GetComponent<Skills> ();
		//skillObj.transform.SetParent (transform);
	}

	void RangedAttack(int type)
	{
		/*
		switch (type) {
		case 1:
			//direct.z = direction.y/ direction.x * 180 / 3.142f;
			GameObject skillObj=Instantiate (skillPrefab, transform.position+direction,Quaternion.Euler(new Vector3(0,0,Mathf.Atan2(direction.y,direction.x)*180f/3.142f))) as GameObject;
			
			Skills skill=skillObj.GetComponent<Skills>();
			
			skillsInfo temp=new skillsInfo();
			temp.damage=1;
			temp.castTime=2.0f;
			temp.knockback=1;
			temp.velocity=direction*5;
			temp.isProjectile=true;
			skill.SetInfo(temp);
			skill.Dir = direction;
			break;
		}
		//*/
	}
}
