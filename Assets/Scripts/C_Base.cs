using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class C_Stats
{
	[SerializeField]
	public float HPregenRate=1f;
	[SerializeField]
	public float MPregenRate=2f;

	[SerializeField]
	private int level=1;
	public int Level{
		get{
			return level;
		}
		set
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
		set
		{
			exp=value;
		}
	}
	[SerializeField]
	private float str;
	public float Str{
		get
		{
			return str;
		}
		set
		{
			str=value;
		}
	}//strength
	
	[SerializeField]
	private float dex;
	public float Dex{
		get{
			return dex;
		}
		set
		{
			dex=value;
		}
	}//dexterity
	
	[SerializeField]
	private float intelligence;
	public float Int{
		get{
			return intelligence;
		}
		set
		{
			intelligence=value;
		}
	}//intelligence

	
	[SerializeField]
	private float end;
	public float End {
		get{
			return end;
		}
		set
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
	public void AddExp(int exp)
	{
		Exp += exp;
		CalcLevel ();
	}

	void CalcLevel()
	{
		int tempExp = Exp;
		int tempLevel = 1;
		while(tempExp>((float)tempLevel+(float)(Mathf.Pow(((float)tempLevel*0.5f),1.5f)*40f+100f)))
		{
			tempExp-=Mathf.FloorToInt((float)tempLevel+(float)(Mathf.Pow(((float)tempLevel*0.5f),1.5f)*40f+100f));
			tempLevel++;
		}
		while (tempLevel > level) {
			str+=0.1f;
			Dex+=2;
			end+=5;
			level++;
		}
		RecalStats();
		level = tempLevel;

		//set exp to current level
	}

	int expToLevel()
	{
		//return the amount of exp needed to level up
		return 0;
	}


	void AddStatPoints()//adds stat points based on current level
	{
		int bonus = Mathf.FloorToInt(Mathf.Pow ((Level - 2), 1.2f));
		if (bonus < 0)
			bonus = 0;
		statPoints += 5 + bonus;
	}

	public int GetPointsNeeded(float num)
	{
		return Mathf.FloorToInt (Mathf.Pow (num / 8, 1.3f)); 
	}
	public void RecalStats()//not used for base stats
	{
		float percentageHealth = Health / MaxHealth;
		float percentageMana = Mana / MaxMana;
		MaxHealth = baseHealth + (0.5f * Str)+(30.0f*End)+Level*2.2f;
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
	public float  InvincTime=1.5f;
	public float InvincTimeLeft;
	GameObject skillGO=null;
	Skills skillScript=null;
	public bool dead=false;
	float deathTimeLeft;
	public float deathTime=3.0f;
	public weaponInfo equippedWeapon;
	float AttackCoolOff;
	SpriteRenderer rend;
	SpriteRenderer[] handrend;
	Anim_controller animControl;
	UnityEngine.UI.Slider health;
	UnityEngine.UI.Slider mana;


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
		int GotSave = PlayerPrefs.GetInt ("GotSave");
		if(GotSave==1)
		{
			float temp=PlayerPrefs.GetFloat("PlayerHealth");
			//if (temp == 0)
			stats.Health = temp;
			temp=PlayerPrefs.GetFloat("PlayerMana");
			stats.Mana=temp;
			temp=PlayerPrefs.GetFloat("PlayerExp");
			stats.SetExp(Mathf.FloorToInt(temp));
			/*
			temp=PlayerPrefs.GetFloat("PlayerStr");
			stats.Str=Mathf.FloorToInt(temp);
			temp=PlayerPrefs.GetFloat("PlayerDex");
			stats.Dex=Mathf.FloorToInt(temp);
			temp=PlayerPrefs.GetFloat("PlayerEnd");
			stats.End=Mathf.FloorToInt(temp);
			temp=PlayerPrefs.GetFloat("PlayerInt");
			stats.Int=Mathf.FloorToInt(temp);
			*/
			temp=PlayerPrefs.GetFloat("PlayerWeaponDamage");
			equippedWeapon.damage=temp;
			string t2=PlayerPrefs.GetString("PlayerWeapon");
			equippedWeapon.spriteName=t2;
			temp=PlayerPrefs.GetInt("PlayerWeaponType");
			if(temp==1)
				equippedWeapon.wtype=WeaponTypes.ATTACK;
			else
				equippedWeapon.wtype=WeaponTypes.DEFENCE;


			/*
			PlayerPrefs.SetFloat("PlayerHealth",temp.stats.Health);
			PlayerPrefs.SetFloat ("PlayerMana",temp.stats.Mana);
			PlayerPrefs.SetFloat ("PlayerStr",temp.stats.Str);
			PlayerPrefs.SetFloat ("PlayerDex",temp.stats.Dex);
			PlayerPrefs.SetFloat("PlayerEnd",temp.stats.End);
			PlayerPrefs.SetFloat("PlayerInt",temp.stats.Int);
			PlayerPrefs.SetFloat ("PlayerExp",temp.stats.Exp);
			PlayerPrefs.SetInt("GotSave",1);

			PlayerPrefs.SetFloat ("PlayerWeaponDamage",temp.equippedWeapon.damage);
			PlayerPrefs.SetString ("PlayerWeapon",temp.equippedWeapon.spriteName);
			if(temp.equippedWeapon.wtype==WeaponTypes.ATTACK)
				PlayerPrefs.SetInt("PlayerWeaponType",1);
			else
				PlayerPrefs.SetInt("PlayerWeaponType",2);
			*/
		}
		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		//bool left = false;
		direction = new Vector3 ();
		stats.InitStats ();
		stats.RecalStats ();
		rend = GetComponent<SpriteRenderer> ();
		handrend = GetComponentsInChildren<SpriteRenderer> ();
		animControl = GetComponent<Anim_controller> ();
		GameObject g = GameObject.Find ("Health");
		health = g.GetComponent<UnityEngine.UI.Slider> ();
		g = GameObject.Find ("Mana");
		mana = g.GetComponent<UnityEngine.UI.Slider> ();
	}

	public void Damage(int damage)
	{
		if (InvincTimeLeft > 0)
			return;
		InvincTimeLeft = InvincTime;
		stats.Health -= damage;
		rend.color = new Color (1.0f, 0.0f, 0.0f);
		foreach (SpriteRenderer s in handrend)
			s.color = new Color (1.0f, 0, 0);
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
		List<buffs> removed = new List<buffs> ();
		for(int i=0;i<buffList.Count;i++)
		{
			buffs buff=buffList[i];
			buff.duration-=Time.deltaTime;
			if(buff.duration<=0)
			{
				//int stackNum;
				i--;
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
			GetComponent<CircleCollider2D>().enabled=false;
			if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime>=1)
			{
				if(deathTimeLeft<=0)
					Application.LoadLevel("MainMenu");
				else
					deathTimeLeft-=Time.deltaTime;
			}
			return;
		
		}
		InvincTimeLeft -= Time.deltaTime;
		stats.Health += stats.HPregenRate * Time.deltaTime;
		stats.Mana += stats.MPregenRate * Time.deltaTime;
		if (stats.Health > stats.MaxHealth)
			stats.Health = stats.MaxHealth;
		if (stats.Mana > stats.MaxMana)
			stats.Mana = stats.MaxMana;
		//basic mechanics: movement with mouse
		if (!ProcBuffs())//basically stunned
			return;
		health.value = stats.Health / stats.MaxHealth;
		mana.value = stats.Mana / stats.MaxMana;

		//basic mechanics: camera movement with character
		Vector3 temp = transform.position;
		temp.z = -10;
		//Camera.main.transform.position = Vector3.Lerp (Camera.main.transform.position, temp, cameraSpd);
		
		float xDif = temp.x - Camera.main.transform.position.x;
		
		if (Mathf.Abs (xDif) < 4f) {
			xDif = 0;
		} else if (xDif > 0) {
			xDif -= 4f;
		} else {
			xDif += 4f;
		}
		
		float yDif = temp.y - Camera.main.transform.position.y;
		
		yDif-=1.5f;
		
		if (Mathf.Abs (yDif) < 1.5f) {
			yDif = 0;
		} else if (yDif > 0) {
			yDif -= 1.5f;
		} else {
			yDif += 1.5f;
		}
		Camera.main.transform.position += new Vector3 (xDif, yDif, 0);


		if (inAttackAnimation) {
				rb.velocity = new Vector2 (0, 0);
				nextPos = transform.position;
				AttackCoolOff-=Time.deltaTime;
				if(AttackCoolOff<=0)
					inAttackAnimation = false;
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

	public void LateUpdate()
	{
		if (InvincTimeLeft <= 0) {
			rend.color = new Color (1, 1, 1);
			foreach (SpriteRenderer s in handrend)
				s.color = new Color (1, 1, 1);
		} else {
			rend.color = new Color (1.0f, 0.4f, 0.0f);
			foreach (SpriteRenderer s in handrend)
				s.color = new Color (1.0f, 0.4f, 0);
		}

	}
		
	public void swapWeapon(ref weaponInfo weapon)
	{
		weaponInfo temp;
		temp = equippedWeapon;
		equippedWeapon = weapon;
		weapon = temp;
		animControl.sheetName = equippedWeapon.spriteName;
		if (equippedWeapon.wtype == WeaponTypes.ATTACK)
			animControl.Fire = true;
		else
			animControl.Fire = false;

	}

	public void Attack(int type,int weaponType,Vector2 mousePos)
	{
		if (!inAttackAnimation) {
			inAttackAnimation=true;
			direction = -Camera.main.WorldToScreenPoint (transform.position) + (Vector3)mousePos;
			direction.Normalize ();
			if(direction.x>0)
			{
				anim.SetBool("Left",false);
			}
			else
				anim.SetBool("Left",true);
			if(direction.y>0)
				anim.SetBool("Forward",false);
			else
				anim.SetBool("Forward",true);
			if(weaponType!=2)
			{
				switch(equippedWeapon.wtype)
				{
				case WeaponTypes.ATTACK:
					Attack_Fire(type, direction);

					break;
				case WeaponTypes.DEFENCE:
					Attack_Nature(type,direction);

					break;
				}
			}
			else
			{
				DefaultAttack(type,direction);
			}
			//remove once proper skills are implemented
			//inAttackAnimation = true;

			//find a way to change the sprite at run time;
			Vector2 Dir = (Vector2)(direction);
		}

	}

	void DefaultAttack(int type,Vector3 AttackDir)
	{
		direction = AttackDir;
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

	void Attack_Nature(int type,Vector3 AttackDir)
	{
		direction = AttackDir;
		anim.SetTrigger("attack");
		anim.SetInteger ("AtkType", 1);
		GameObject go=null;
		GameObject skillObj = null;
		go=Resources.Load(SkillPath+"Earth_"+type.ToString())as GameObject;
		switch (type) {
		case 1:
		{
			//direct.z = direction.y/ direction.x * 180 / 3.142f;
			skillsInfo temp=new skillsInfo();
			//temp.damage=4;
			temp.damage=4f*stats.Str+2f*equippedWeapon.damage;
			temp.castTime=0.4f;
			temp.type=0;
			temp.knockback=5.0f;
			temp.ConstantDam=true;
			temp.skillName=SkillPath+"Earth_"+type.ToString();
			skillObj=Instantiate (go, transform.position,Quaternion.identity) as GameObject;
			Vector3 scale=skillObj.transform.localScale;
			if(AttackDir.x<0)
			{
				scale.x=-scale.x;
			}
			if(AttackDir.y>0)
			{
				scale.y=-scale.y;
			}
			skillObj.transform.localScale=scale;
			Skills skill=skillObj.GetComponent<Skills>();
			
			skill.SetInfo(temp);
			skill.Dir = direction;
			AttackCoolOff=0.2f;
			skill.character=gameObject;
			//skill.ApplyDamage=true;
		}
			break;
		case 2:
		{
			if(stats.Mana<5)
				return;
			stats.Mana-=5;
			//direct.z = direction.y/ direction.x * 180 / 3.142f;
			skillsInfo temp=new skillsInfo();
			//temp.damage=4;
			temp.damage=4*stats.Str+2*equippedWeapon.damage;
			temp.castTime=0.4f;
			temp.type=0;
			temp.knockback=5.0f;
			temp.ConstantDam=true;
			temp.skillName=SkillPath+"Earth_"+type.ToString();
			Vector3 t=Camera.main.ScreenToWorldPoint(Input.mousePosition);
			t.z=0;
			skillObj=Instantiate (go,t,Quaternion.identity) as GameObject;
			if(AttackDir.x<0)
			{
				Vector3 scale=skillObj.transform.localScale;
				scale.x=-scale.x;
				skillObj.transform.localScale=scale;
			}
			Skills skill=skillObj.GetComponent<Skills>();
			
			skill.SetInfo(temp);
			skill.Dir = direction;
			AttackCoolOff=0.8f;
			skill.character=gameObject;
			//skill.ApplyDamage=true;
		}
			break;
		case 3:
		{
			if(stats.Mana<7)
				return;
			stats.Mana-=7;
			//direct.z = direction.y/ direction.x * 180 / 3.142f;
			skillObj=Instantiate (go, transform.position+direction*1,Quaternion.identity) as GameObject;
			Skills skill=skillObj.GetComponent<Skills>();
			
			skillsInfo temp=new skillsInfo();
			//temp.damage=2;
			temp.damage=2*stats.Str+equippedWeapon.damage*1.2f;
			temp.castTime=0.1f;
			temp.type=1;
			temp.pierceNumber=3;
			temp.knockback=0.2f;
			temp.ConstantDam=false;
			temp.skillName=SkillPath+"Earth_"+type.ToString();
			skill.SetInfo(temp);
			skill.Dir = direction;
			AttackCoolOff=0.8f;
			skill.character=gameObject;
		}
			break;
		case 4:
		{
			
			if(stats.Mana<6)
				return;
			stats.Mana-=6;
			//direct.z = direction.y/ direction.x * 180 / 3.142f;
			skillObj=Instantiate (go, transform.position,Quaternion.identity) as GameObject;
			Skills skill=skillObj.GetComponent<Skills>();
			
			skillsInfo temp=new skillsInfo();
			//temp.damage=1;
			temp.damage=1*stats.Str+0.8f*equippedWeapon.damage;
			temp.castTime=2.0f;
			temp.type=2;
			temp.knockback=1.0f;
			temp.ConstantDam=true;
			temp.velocity=direction;
			temp.skillName=SkillPath+"Fire_"+type.ToString();
			skill.SetInfo(temp);
			skill.Dir = direction;
			AttackCoolOff=0.5f;
			skill.character=gameObject;
		}
			break;
		}
		anim.SetTrigger ("attack");
		anim.SetInteger ("AtkType", type);
		skillGO = skillObj;
		skillScript = skillGO.GetComponent<Skills> ();
		//skillObj.transform.SetParent (transform);
	}


	void Attack_Fire(int type,Vector3 AttackDir)
	{
		direction = AttackDir;
		GameObject go=null;
		GameObject skillObj = null;
		if(type!=1)
			go=Resources.Load(SkillPath+"Fire_"+type.ToString())as GameObject;
		switch (type) {
		case 1:
		{
			//direct.z = direction.y/ direction.x * 180 / 3.142f;
			skillsInfo temp=new skillsInfo();
			//temp.damage=4;
			temp.damage=4*stats.Str+2*equippedWeapon.damage;
			temp.castTime=0.4f;
			temp.type=0;
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
			AttackCoolOff=0.2f;
			skill.character=gameObject;
		}
			break;
		case 2:
		{
			
			if(stats.Mana<3)
				return;
			stats.Mana-=3;
			//direct.z = direction.y/ direction.x * 180 / 3.142f;
			skillObj=Instantiate (go, transform.position,Quaternion.identity) as GameObject;
			Skills skill=skillObj.GetComponent<Skills>();
			if(AttackDir.x>0)
			{
				
			}
			
			skillsInfo temp=new skillsInfo();
			//temp.damage=1;
			temp.damage=1*stats.Str+0.8f*equippedWeapon.damage;
			temp.castTime=0.5f;
			temp.type=0;
			temp.knockback=1.0f;
			temp.ConstantDam=true;
			temp.skillName=SkillPath+"Fire_"+type.ToString();
			skill.SetInfo(temp);
			skill.Dir = direction;
			AttackCoolOff=0.8f;
			skill.character=gameObject;

		}
			break;
		case 3:
		{
			if(stats.Mana<1)
				return;
			stats.Mana-=1;
			//direct.z = direction.y/ direction.x * 180 / 3.142f;
			skillObj=Instantiate (go, transform.position,Quaternion.identity) as GameObject;
			Skills skill=skillObj.GetComponent<Skills>();
			
			skillsInfo temp=new skillsInfo();
			//temp.damage=1;
			temp.damage=1*stats.Str+0.8f*equippedWeapon.damage;
			temp.castTime=2.0f;
			temp.type=2;
			temp.knockback=1.0f;
			temp.ConstantDam=false;
			temp.velocity=direction;
			temp.skillName=SkillPath+"Fire_"+type.ToString();
			skill.SetInfo(temp);
			skill.Dir = direction;
			skill.ApplyDamage=true;
			skill.deathOnDamage=true;
			skill.singleFrameDamage=false;
			AttackCoolOff=0.1f;
			skill.character=gameObject;
		}
			break;
		case 4:
		{
			if(stats.Mana<8)
				return;
			stats.Mana-=8;
			//direct.z = direction.y/ direction.x * 180 / 3.142f;
			skillsInfo temp=new skillsInfo();
			//temp.damage=4;
			temp.damage=4*stats.Str+2*equippedWeapon.damage;
			temp.castTime=0.4f;
			temp.type=0;
			temp.knockback=5.0f;
			temp.ConstantDam=true;
			temp.skillName=SkillPath+"Fire_"+type.ToString();
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
			AttackCoolOff=0.6f;
			skill.character=gameObject;
			//skill.ApplyDamage=true;
		}
			break;
		}
		anim.SetTrigger("attack");
		anim.SetInteger ("AtkType", 1);
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
