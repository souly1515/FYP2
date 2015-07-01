using UnityEngine;
using System.Collections;

public class DamageOnContact : MonoBehaviour {
	public bool HitOnce = false;
	public int damage=10;
	void OnTriggerEnter2D(Collider2D col)
	{
		if (enabled) {
			if (col) {
				if (col.gameObject.layer == LayerMask.NameToLayer ("Default")) {
					C_Base temp = col.gameObject.GetComponent<C_Base> ();
					if(temp)
					{
						temp.Damage (damage);
						if(HitOnce)
							enabled=false;
					}
				}
			}
		}
	}
	void OnCollisionEnter2D(Collision2D col)
	{
		if (enabled) {
				if (col.gameObject.layer == LayerMask.NameToLayer ("Default")) {
					C_Base temp = col.gameObject.GetComponent<C_Base> ();
					if(temp)
					{
						temp.Damage (damage);
						if(HitOnce)
							enabled=false;
					}
				}
		}
	}
}
