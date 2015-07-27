using UnityEngine;
using System.Collections;

public class Lion_Roar : MonoBehaviour {
	public bool RoarOver = false;
	Animator anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (RoarOver) {
			anim.SetBool("RoarOver",true);
		}
		AnimatorStateInfo t = anim.GetCurrentAnimatorStateInfo (0);
		if (RoarOver && t.IsTag ("End") && t.normalizedTime >= 1) {
			Destroy(gameObject);
		}
	}
}
