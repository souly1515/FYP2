using UnityEngine;
using System.Collections;

public class AcidGas : MonoBehaviour {
	public float TimeLeft = 1.0f;
	AcidGasControl control = null;
	Animator anim = null;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		TimeLeft -= Time.deltaTime;
		if(control == null)
		{
			if(transform.parent && transform.parent.GetComponent<AcidGasControl>())
			{
				control = transform.parent.GetComponent<AcidGasControl>();
			}
		}
		else
		{
			if(TimeLeft <= 0)
			{
				control.ChildList.Remove(gameObject);
			}
		}
		if(TimeLeft <= 0)
		{
			if (anim)
			{
				anim.SetTrigger("End");
				if(anim.GetCurrentAnimatorStateInfo(0).IsName("GasDissipitate") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >=1)
				{
					Destroy(gameObject);
				}
			}
			else
			{
				Destroy(gameObject);
			}
		}
	}


}
