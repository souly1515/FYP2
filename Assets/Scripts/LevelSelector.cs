using UnityEngine;
using System.Collections;

public class LevelSelector : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			PlayerPrefs.SetInt("GotSave",1);
			PlayerPrefs.SetFloat("PlayerHealth",500);
			PlayerPrefs.SetFloat ("PlayerMana",500);
			PlayerPrefs.SetFloat ("PlayerExp",0);
			PlayerPrefs.SetFloat ("PlayerWeapon",1);
			PlayerPrefs.SetString ("PlayerWeapon","weapon1");
			PlayerPrefs.Save();
			Application.LoadLevel ("Level_1");
		}
		if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			
			PlayerPrefs.SetInt("GotSave",1);
			PlayerPrefs.SetFloat("PlayerHealth",10000);
			PlayerPrefs.SetFloat ("PlayerMana",10000);
			PlayerPrefs.SetFloat ("PlayerExp",150);
			PlayerPrefs.SetFloat ("PlayerWeapon",2);
			PlayerPrefs.SetString ("PlayerWeapon","weapon1");
			PlayerPrefs.Save();
			Application.LoadLevel ("Level_2");
		}
		if(Input.GetKeyDown(KeyCode.Alpha3))
		{
			
			PlayerPrefs.SetInt("GotSave",1);
			PlayerPrefs.SetFloat("PlayerHealth",10000);
			PlayerPrefs.SetFloat ("PlayerMana",10000);
			PlayerPrefs.SetFloat ("PlayerExp",300);
			PlayerPrefs.SetFloat ("PlayerWeapon",2);
			PlayerPrefs.SetString ("PlayerWeapon","weapon1");
			PlayerPrefs.Save();
			Application.LoadLevel ("Level_3");
		}
		if(Input.GetKeyDown(KeyCode.Alpha4))
		{
			PlayerPrefs.SetInt("GotSave",1);
			
			PlayerPrefs.SetFloat("PlayerHealth",10000);
			PlayerPrefs.SetFloat ("PlayerMana",10000);
			PlayerPrefs.SetFloat ("PlayerExp",400);
			PlayerPrefs.SetFloat ("PlayerWeapon",2);
			PlayerPrefs.SetString ("PlayerWeapon","weapon1");
			PlayerPrefs.Save();
			Application.LoadLevel ("Level_4");
		}
		if(Input.GetKeyDown(KeyCode.Alpha5))
		{
			
			PlayerPrefs.SetInt("GotSave",1);
			PlayerPrefs.SetFloat("PlayerHealth",10000);
			PlayerPrefs.SetFloat ("PlayerMana",10000);
			PlayerPrefs.SetFloat ("PlayerExp",500);
			PlayerPrefs.SetFloat ("PlayerWeapon",4);
			PlayerPrefs.SetString ("PlayerWeapon","weapon1");
			PlayerPrefs.Save();
			Application.LoadLevel ("Level_5");
		}
		if(Input.GetKeyDown(KeyCode.Alpha6))
		{
			
			PlayerPrefs.SetInt("GotSave",1);
			PlayerPrefs.SetFloat("PlayerHealth",10000);
			PlayerPrefs.SetFloat ("PlayerMana",10000);
			PlayerPrefs.SetFloat ("PlayerExp",700);
			PlayerPrefs.SetFloat ("PlayerWeapon",5);
			PlayerPrefs.SetString ("PlayerWeapon","weapon1");
			PlayerPrefs.Save();
			Application.LoadLevel ("Level_6");
		}
		if(Input.GetKeyDown(KeyCode.Alpha7))
		{
			
			PlayerPrefs.SetInt("GotSave",1);
			PlayerPrefs.SetFloat("PlayerHealth",10000);
			PlayerPrefs.SetFloat ("PlayerMana",10000);
			PlayerPrefs.SetFloat ("PlayerExp",800);
			PlayerPrefs.SetFloat ("PlayerWeapon",7);
			PlayerPrefs.SetString ("PlayerWeapon","weapon1");
			PlayerPrefs.Save();
			Application.LoadLevel ("Level_7");
		}
	}
}
