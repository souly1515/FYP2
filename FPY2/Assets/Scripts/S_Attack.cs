using UnityEngine;
using System.Collections;

public class S_Attack : MonoBehaviour {

	public float timeLeft;
	private bool end;
	public float rawDamage;

	// Use this for initialization
	void Start () {
		end = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (timeLeft <= 0) {
			end=true;
		}
		if (end == true) {
			gameObject.SetActive (false);
		} else {
			timeLeft-=Time.deltaTime*0.001f;
		}
	}

	void OnCollisionStay(Collision col)
	{
		if(timeLeft<=0)
		{
			col.gameObject.GetComponent<E_Base>().ApplyDamage(rawDamage);
		}
	}
}
