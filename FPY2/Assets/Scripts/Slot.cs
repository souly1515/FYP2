using UnityEngine;
using System.Collections;

public class Slot : MonoBehaviour {
	public bool occupied;
	public Weapon weapon;
	public int x;
	public int y;
	Weapon lastWeapon;
	public SpriteRenderer sprite;
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
		else if (weapon != lastWeapon) {

			switch(weapon.info.wtype)
			{
			case WeaponTypes.OH_MACE:
				sprite.color = new Color (1.0f, 0.0f, 0.0f);
				break;
			}
			lastWeapon=weapon;
		}

	}
}
