using UnityEngine;
using System.Collections;

public struct slot
{
	bool occupied;
	Weapon weapon;
}
public class Inventory : MonoBehaviour {
	slot[,] slots;
	// Use this for initialization
	void Start () {
		slots=new slot[10,10];
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
