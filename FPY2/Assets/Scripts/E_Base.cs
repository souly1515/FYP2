using UnityEngine;
using System.Collections;

public class E_Stats
{
	public int health;
};

public class E_Base : MonoBehaviour {

	public E_Stats stats;
	// Use this for initialization
	void Start () {
		stats.health = 5;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void ApplyDamage(float attack)
	{
		//apply armor application
		stats.health -= (int)attack;
		if (stats.health <= 0) {
			gameObject.SetActive (false);
		}
	}
}
