using UnityEngine;
using System.Collections;

public class Anim_controller : MonoBehaviour {
	public Sprite weapon1;
	public Sprite weapon2;
	public Sprite weapon3;
	Animator anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
	}
}
