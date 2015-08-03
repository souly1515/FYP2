using UnityEngine;
using System.Collections;

public class Lion_Roar : MonoBehaviour {
	public bool RoarOver = false;
	Animator anim;
	DamageOnContact damScript;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		damScript = GetComponent<DamageOnContact> ();
	}
	
	// Update is called once per frame
	void Update () {
		damScript.enabled=true;

		if (RoarOver) {
			anim.SetBool("RoarOver",true);
		}
		AnimatorStateInfo t = anim.GetCurrentAnimatorStateInfo (0);
		if (RoarOver && t.IsTag ("End") && t.normalizedTime >= 1) {
			Destroy(gameObject);
		}
	}
}
