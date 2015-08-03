using UnityEngine;
using System.Collections;
using System;

public enum WeaponTypes
{
	ATTACK,
	DEFENCE,
	SUPPORT
}

[System.Serializable]
public class weaponInfo
{
	public float damage;
	public WeaponTypes wtype;
	public string spriteName;
	public int inventorySizeX;
	public int inventorySizeY;
	public int inventoryX;//xpos in inventory
	public int inventoryY;//ypos in inventory
}

[System.Serializable]
public class Weapon:MonoBehaviour{
	public weaponInfo info;
	SpriteRenderer sRenderer;
	string lastSheetName;
	Sprite[] subsprite;
	void Start()
	{
		
		lastSheetName = "";
		sRenderer = GetComponent<SpriteRenderer> ();
	}
	void LateUpdate()
	{
		
		if (lastSheetName != info.spriteName) {
			subsprite = Resources.LoadAll<Sprite>("Weapons/" + info.spriteName)as Sprite[];
			
			string curSprite = sRenderer.sprite.name;
			if(subsprite!=null)
			{
				Sprite nsprite = Array.Find (subsprite, item => item.name == curSprite);
				if (nsprite) {
					lastSheetName=info.spriteName;
					sRenderer.sprite = nsprite;
				}
			}
		} else {
			string curSprite = sRenderer.sprite.name;
			Sprite nsprite = Array.Find (subsprite, item => item.name == curSprite);
			if (nsprite) {
				sRenderer.sprite = nsprite;
			}
		}
	}
}
