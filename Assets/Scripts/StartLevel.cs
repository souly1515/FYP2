using UnityEngine;
using System.Collections;

public class StartLevel: MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartGame()
	{
		//PlayerPrefs.SetFloat("PlayerHealth",temp.stats.Health);
		//PlayerPrefs.Save();
		string temp = PlayerPrefs.GetString ("LastLevel");
		if (temp == "")
			Application.LoadLevel ("Level_1");
		else
			Application.LoadLevel(temp);
	}
	public void ExitGame()
	{
		Application.Quit ();
	}
}
