using UnityEngine;
using System.Collections;

public struct buff
{
	bool disabled;
	float DoT;
	string name;
}

[System.Serializable]
public struct skillsInfo
{
	public float damage;
	public float knockback;
	public float castTime;
	public buff buffType;
	public bool ConstantDam;//constantly damage
	public bool isProjectile;
	public Vector2 velocity;
	public int pierceNumber;
	public int endType;//hardcoded like 1 = no end, 2 = explosive ending
	public string skillName;
}

public class Skills : MonoBehaviour {
	public skillsInfo info;
	public bool ApplyDamage=false;
	public bool appliedDamage=false;//to test if damage step was done
	public LayerMask enemy;
	public Vector2 Dir;
	public Animator anim;
	public float timeLeft;
	public bool SkillOver=false;
	bool resetTime=false;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}

	public void SetInfo(skillsInfo newInfo)
	{
		info = newInfo;
	}

	// Update is called once per frame
	void Update () {
		timeLeft -= Time.deltaTime;
		if (resetTime) {
			timeLeft = info.castTime;
			resetTime=false;
		}

		if (ApplyDamage) {
			appliedDamage = true;
			ApplyDamage=false;
			SkillOver=true;
		}
		if (info.isProjectile) {
			//*
			info.castTime-=Time.deltaTime;
			if(info.pierceNumber>0&&info.castTime<=0)
			{
				GameObject go=Resources.Load(info.skillName)as GameObject;
				GameObject skillObj=Instantiate (go, transform.position+(Vector3)Dir*1,Quaternion.identity) as GameObject;
				Skills skill=skillObj.GetComponent<Skills>();

				skillsInfo temp=new skillsInfo();
				temp.damage=1;
				temp.castTime=0.1f;
				temp.isProjectile=true;
				temp.pierceNumber=info.pierceNumber-1;
				temp.knockback=10;
				temp.skillName=info.skillName;
				skill.Dir=Dir;
				skill.SetInfo(temp);
				info.pierceNumber=0;
			}
			//*/
		}
		if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime>=1) {
			if (appliedDamage) {
				//collision testing and attack enemies
				DestroyObject(gameObject);
			} else {
				anim.SetBool("Stop",true);
				ApplyDamage = true;
			}
		}

	}
	void OnTriggerStay2D(Collider2D col)
	{
		if (info.ConstantDam) {
			if(timeLeft>0)
				return;
		}
		if (ApplyDamage||info.ConstantDam) {
			if ((enemy.value & (1 << col.gameObject.layer)) > 0) {
				E_Base E = col.gameObject.GetComponent<E_Base> ();
				if(E)
				{
					E.ApplyDamage (info.damage);
					E.KnockBack (info.knockback, Dir,0);
					resetTime=true;
				}
			}
		}
	}
}
