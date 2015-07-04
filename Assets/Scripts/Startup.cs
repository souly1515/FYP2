using UnityEngine;
using System.Collections;

public class Startup : MonoBehaviour {

	static Startup()
	{
		PlayerPrefs.SetInt ("PlayerHealth", 300);
	}
}
