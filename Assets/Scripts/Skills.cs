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
	public float castTime;//should be supplemented by animation duration
	public buff buffType;
	public bool isProjectile;
	public Vector2 velocity;
	public int pierceNumber;
	public int endType;//hardcoded like 1 = no end, 2 = explosive ending
}

public class Skills : MonoBehaviour {
	public skillsInfo info;
	public bool ApplyDamage=false;
	public LayerMask enemy;
	public Vector2 Dir;
	// Use this for initialization
	void Start () {
	}

	public void SetInfo(skillsInfo newInfo)
	{
		info = newInfo;
	}

	// Update is called once per frame
	void Update () {
		if (info.isProjectile) {
			if(info.castTime>0)
			{
				transform.position=transform.position+(Vector3)(info.velocity*Time.deltaTime);
				info.castTime-=Time.deltaTime;

			}
			else{
				gameObject.SetActive(false);
			}
		} else {
			if (info.castTime <= 0) {
				if (ApplyDamage) {
					//collision testing and attack enemies
					gameObject.SetActive (false);
					//DestroyObject(gameObject);
				} else {
					ApplyDamage = true;
				}
			} else {
				info.castTime -= Time.deltaTime;
			}
		}

	}
	void OnTriggerStay2D(Collider2D col)
	{
		if (info.isProjectile) {
			
			if ((enemy.value & (1 << col.gameObject.layer)) > 0) {
				E_Base E = col.gameObject.GetComponent<E_Base> ();
				E.ApplyDamage (info.damage,1.0f);
				E.KnockBack (info.knockback, Dir);
				gameObject.SetActive(false);
			}
		}
		else{

			if (ApplyDamage) {
				if ((enemy.value & (1 << col.gameObject.layer)) > 0) {
					E_Base E = col.gameObject.GetComponent<E_Base> ();
					E.ApplyDamage (info.damage,1.0f);
					E.KnockBack (info.knockback, Dir);
				}
			}
		}
	}
}
