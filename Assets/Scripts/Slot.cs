using UnityEngine;
using System.Collections;

public class Slot : MonoBehaviour {
	public bool occupied;
	//public GameObject weapon;
	public int x;
	public int y;
	weaponInfo lastWeapon;
	public SpriteRenderer sprite;
	public weaponInfo weaponScript;
	// Use this for initialization
	void Start () {
		sprite = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!occupied) {
			sprite.color=new Color(1.0f,1.0f,1.0f);
			lastWeapon=null;
		}
		else if (weaponScript != lastWeapon) {
			if(weaponScript!=null)
			{
				//weaponScript = weapon.GetComponent<Weapon> ().info;
				switch(weaponScript.wtype)
				{
				case WeaponTypes.ATTACK:
					sprite.color = new Color (1.0f, 0.0f, 0.0f);
					break;
				case WeaponTypes.DEFENCE:
					sprite.color=new Color(0.0f,1.0f,0.0f);
					break;
				}
				lastWeapon=weaponScript;
			}
		}

	}
}
