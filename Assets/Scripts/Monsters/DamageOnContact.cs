using UnityEngine;
using System.Collections;

public class DamageOnContact : MonoBehaviour {
	public bool HitOnce = false;
	public int damage=10;
	public float DamageInterval = 0.2f;
	float TimeLeft = 0;

	void Update()
	{
		TimeLeft -= Time.deltaTime;
	}
	void OnTriggerStay2D(Collider2D col)
	{
		if (enabled) 
		{
			if (col)
			{
				if (col.gameObject.layer == LayerMask.NameToLayer ("Default")) 
				{
					C_Base temp = col.gameObject.GetComponent<C_Base> ();
					if(temp)
					{
						if (HitOnce)
						{
							temp.Damage (damage);
							enabled = false;
						}
						else
						{
							if(TimeLeft <= 0)
							{
								temp.Damage(damage);
								TimeLeft = DamageInterval;
							}
						}
					}
				}
			}
		}
	}
	void OnCollisionStay2D(Collision2D col)
	{
		if (enabled) 
		{
			if (col.gameObject.layer == LayerMask.NameToLayer ("Default")) 
			{
				C_Base temp = col.gameObject.GetComponent<C_Base> ();
				if(temp)
				{
					if (HitOnce)
					{
						temp.Damage(damage);
						enabled = false;
					}
					else
					{
						if (TimeLeft <= 0)
						{
							temp.Damage(damage);
							TimeLeft = DamageInterval;
						}
					}
				}
			}
		}
	}
	void OnCollisionEnter2D(Collision2D col)
	{
		if (enabled) 
		{
			if (col.gameObject.layer == LayerMask.NameToLayer ("Default")) 
			{
				C_Base temp = col.gameObject.GetComponent<C_Base> ();
				if(temp)
				{
					if (HitOnce)
					{
						temp.Damage(damage);
						enabled = false;
					}
					else
					{
						if (TimeLeft <= 0)
						{
							temp.Damage(damage);
							TimeLeft = DamageInterval;
						}
					}
				}
			}
		}
	}
	void OnTriggerEnter2D(Collider2D col)
	{
		if (enabled) 
		{
			if (col) 
			{
				if (col.gameObject.layer == LayerMask.NameToLayer ("Default")) 
				{
					C_Base temp = col.gameObject.GetComponent<C_Base> ();
					if(temp)
					{
						if (HitOnce)
						{
							temp.Damage(damage);
							enabled = false;
						}
						else
						{
							if (TimeLeft <= 0)
							{
								temp.Damage(damage);
								TimeLeft = DamageInterval;
							}
						}
					}
				}
			}
		}
	}
}
