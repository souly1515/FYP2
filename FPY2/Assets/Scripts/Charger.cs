using UnityEngine;
using System.Collections;

public class Charger : E_Base {
	Vector3 targ;//where its charging
	Vector3 charTarg;
	const float chargeCoolOff=0.5f;//after the attack miss how long it will charge for
	
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


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
