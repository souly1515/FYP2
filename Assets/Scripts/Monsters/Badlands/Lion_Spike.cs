using UnityEngine;
using System.Collections;

public class Lion_Spike : MonoBehaviour {
	Animator anim;
	public float timeLeft;
	DamageOnContact s;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		s = GetComponent<DamageOnContact> ();
	}
	
	// Update is called once per frame
	void Update () {
		AnimatorStateInfo t = anim.GetCurrentAnimatorStateInfo (0);
		if (t.IsTag ("End") && t.normalizedTime >= 1) {
			Destroy (gameObject);
		} else if (timeLeft > 0) {
			timeLeft -= Time.deltaTime;
		} else {
			s.enabled=true;
			anim.SetTrigger("Attack");
		}
	}
}
