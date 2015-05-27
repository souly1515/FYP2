using UnityEngine;
using System.Collections;

public enum WeaponTypes
{
	OH_SWORD,
	OH_MACE
}

public struct weaponInfo
{
	public float damage;
	public WeaponTypes wtype;
	public Sprite sprite;
	public int inventorySizeX;
	public int inventorySizeY;
}

public class Weapon : MonoBehaviour {
	weaponInfo info;
	// Use this for initialization
	void Start () {
		info.sprite = GetComponent<SpriteRenderer> ().sprite;
	}
	// Update is called once per frame
	void Update () {
		if (GetComponent<SpriteRenderer> ().sprite != info.sprite) {
			GetComponent<SpriteRenderer>().sprite=info.sprite;
		}
	}
}
