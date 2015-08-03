using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour {
	string CurrentLevel;
	int currentLevelNum;
	// Use this for initialization
	void Start () {
		CurrentLevel = Application.loadedLevelName;
		string[] temp=new string[2];
		temp = CurrentLevel.Split ('_');
		currentLevelNum = int.Parse( temp [1]);
		CurrentLevel = temp [0];
	}


	void OnTriggerEnter2D(Collider2D col)
	{
		C_Base temp = col.gameObject.GetComponent<C_Base> ();
		if(temp)
		{
			//should offload this to new function in player
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

			PlayerPrefs.SetString("LastLevel",CurrentLevel+"_"+(currentLevelNum+1).ToString()	);

			PlayerPrefs.Save();
			Application.LoadLevel(CurrentLevel+"_"+(++currentLevelNum).ToString());
		}
	}
}
