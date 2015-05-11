using UnityEngine;
using System.Collections;

public class E_Stats
{
	public int health;
	public float defense;
	public float attack;
	public float moveSpd;
	public float atkSpd;
	public float minAggro;
	public float maxAggro;
};

public enum E_States
{
	STATE_NONE=0,
	IDLE_MOVE,
	IDLE_NOTMOVE,
	TRACKING,
	TRACKING_STOP,
	ATTACK,
	STATE_TOTAL
}

public class E_Base : MonoBehaviour {
	public E_States states=new E_States();
	public E_Stats stats=new E_Stats();
	public int currentHealth;
	Vector2 originalPos;
	Vector2 lastIdlePos;
	float timeLeft;//for timings with state machine
	// Use this for initialization
	void Start () {
		stats.health = 5;
		originalPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		currentHealth = stats.health;
	}

	public void ApplyDamage(float attack)
	{
		//apply armor application
		stats.health -= (int)attack;
		Debug.Log ("Damaged\n");
		if (stats.health <= 0) {
			gameObject.SetActive (false);
		}
	}

	void ChangeState()
	{
		switch (states) {
		case E_States.IDLE_MOVE:

			break;
		case E_States.IDLE_NOTMOVE:

			break;
		}
	}

}
