using UnityEngine;
using System.Collections;

public class BossTest : MonoBehaviour {
	public GameObject boss1;
	public GameObject boss2;
	public C_Base c;
	public float timeleft=5;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!boss1 && !boss2) {
			if(timeleft>0)
				timeleft-=Time.deltaTime;
			else
			{
				string CurrentLevel = Application.loadedLevelName;
				string[] temp=new string[2];
				temp = CurrentLevel.Split ('_');
				int currentLevelNum = int.Parse( temp [1]);
				CurrentLevel = temp [0];
				
				//should offload this to new function in player
				PlayerPrefs.SetFloat("PlayerHealth",c.stats.Health);
				PlayerPrefs.SetFloat ("PlayerMana",c.stats.Mana);
				PlayerPrefs.SetFloat ("PlayerStr",c.stats.Str);
				PlayerPrefs.SetFloat ("PlayerDex",c.stats.Dex);
				PlayerPrefs.SetFloat("PlayerEnd",c.stats.End);
				PlayerPrefs.SetFloat("PlayerInt",c.stats.Int);
				PlayerPrefs.SetFloat ("PlayerExp",c.stats.Exp);
				PlayerPrefs.SetInt("GotSave",1);
				
				PlayerPrefs.SetFloat ("PlayerWeaponDamage",c.equippedWeapon.damage);
				PlayerPrefs.SetString ("PlayerWeapon",c.equippedWeapon.spriteName);
				if(c.equippedWeapon.wtype==WeaponTypes.ATTACK)
					PlayerPrefs.SetInt("PlayerWeaponType",1);
				else
					PlayerPrefs.SetInt("PlayerWeaponType",2);
				
				PlayerPrefs.SetString("LastLevel",CurrentLevel+"_"+(currentLevelNum+1).ToString()	);
				if(currentLevelNum==7)
				{
					Application.LoadLevel("MainLevel");
					return;
				}
				PlayerPrefs.Save();
				Application.LoadLevel(CurrentLevel+"_"+(++currentLevelNum).ToString());
			}
		}
	}
}
