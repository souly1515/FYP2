using UnityEngine;
using System.Collections;

public struct buff
{
	bool disabled;
	float DoT;
	string name;
}

public struct skillsInfo
{
	public float damage;
	public float rangeY;
	public float rangeX;
	public float knockback;
	public float castTime;
	public buff buffType;
	public bool isProjectile;
	public Vector2 velocity;
	public int pierceNumber;
	public int endType;//hardcoded like 1 = no end, 2 = explosive ending
}

public class Skills : MonoBehaviour {

	public skillsInfo info;
	bool ApplyDamage=false;
	public LayerMask enemy;
	// Use this for initialization
	void Start () {
	}

	public void SetInfo(skillsInfo newInfo)
	{
		info = newInfo;
		gameObject.transform.localScale=new Vector3(info.rangeX,info.rangeY,0);
	}

	// Update is called once per frame
	void Update () {
		if (info.castTime <= 0) {
			if(ApplyDamage)
			{
				//collision testing and attack enemies
				gameObject.SetActive(false);
				//DestroyObject(gameObject);
			}
			else
			{
				ApplyDamage=true;
			}
		} else 
		{
			info.castTime-=Time.deltaTime;
		}

	}
	void OnCollision(Collider col)
	{
		Debug.Log ("skills tracked an object\n");
		if (col.gameObject.layer == enemy) {
			Debug.Log ("skills is damaging an enemy\n");
			E_Base E=col.gameObject.GetComponent<E_Base>();
			E.ApplyDamage(info.damage);
		}
	}
}
