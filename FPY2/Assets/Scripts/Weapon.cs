using UnityEngine;
using System.Collections;

public enum WeaponTypes
{
	OH_SWORD,
	OH_MACE
}

[System.Serializable]
public struct weaponInfo
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
