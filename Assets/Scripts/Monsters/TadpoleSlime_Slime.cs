using UnityEngine;
using System.Collections;

public class TadpoleSlime_Slime : MonoBehaviour {
	public bool regenerated = true;
	public bool dead = false;
	bool dying = false;
	bool regenerating = false;
	public Animator anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		anim.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(dying)
		{
			if (anim.GetCurrentAnimatorStateInfo (0).normalizedTime>=1) {
				dying = false;
				dead = true;
				anim.SetBool ("Dying", false);
				anim.enabled=false;
			}
		}
		else if(regenerating)
		{
			if (anim.GetCurrentAnimatorStateInfo (0).normalizedTime>=1) {
				regenerating = false;
				regenerated = true;
				anim.SetBool ("Regenerating", false);
				anim.enabled=false;
			}
			
		}
	}

	public void KillSlime(bool front)
	{
		if (regenerated) {
			anim.enabled=true;
			anim.SetBool ("Dying", true);
			anim.SetBool("Front",front);
			regenerated = false;
			dying = true;
		}
	}

	public void RegenerateSlime(bool front)
	{
		if (dead) {
			anim.enabled=true;
			anim.SetBool ("Regenerating", true);
			anim.SetBool("Front",front);
			regenerating = true;
			dead = false;
		}
	}
}
