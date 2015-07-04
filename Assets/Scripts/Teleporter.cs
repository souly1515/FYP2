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
			PlayerPrefs.SetInt("PlayerHealth",temp.stats.Health);
			PlayerPrefs.Save();
			Application.LoadLevel(CurrentLevel+"_"+(++currentLevelNum).ToString());
		}
	}
}
