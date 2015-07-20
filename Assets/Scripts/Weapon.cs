using UnityEngine;
using System.Collections;

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
	public Sprite sprite;
	public int inventorySizeX;
	public int inventorySizeY;
	public int inventoryX;//xpos in inventory
	public int inventoryY;//ypos in inventory
}

[System.Serializable]
public class Weapon:MonoBehaviour{
	public weaponInfo info;
	void Start()
	{
		
	}
	void Update()
	{

	}
}
