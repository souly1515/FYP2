using UnityEngine;
using System.Collections;

public class Slimeling :E_Base {
	public Vector3 velocity;
	float lerpProgress;
	bool initialised=false;
	Vector3 originalVel;


	// Use this for initialization
	protected override void Start ()
	{
		base.stats.health = 1;
	}


	protected override void Attack_State ()
	{
	}

	protected override void Idle_State ()
	{
	}

	protected override void Tracking_State ()
	{
	}

	protected override void Update ()
	{
		if (!initialised) {
			originalVel=velocity;
			initialised=true;
		}
		lerpProgress += Time.deltaTime * 0.333f;
		velocity = Vector3.Lerp (originalVel, Vector3.zero, lerpProgress);
		if (velocity.sqrMagnitude < 0.1)
			velocity = Vector3.zero;
		transform.position += velocity * Time.deltaTime;
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if((base.Tracks.value & (1 << col.gameObject.layer)) > 0)
		{
			C_Base script=col.gameObject.GetComponent<C_Base>();
			if(script)
			{
				buffs b= new buffs();
				b.buffName="slimeSlow";
				b.duration=5.0f;
				b.effect=0.1f;
				b.type=buffs.buffTypes.SLOW;
				script.buffList.Add(b);
				if(buffs.stackDic.ContainsKey(b.buffName))
					buffs.stackDic[b.buffName]++;
				else
					buffs.stackDic.Add(b.buffName,1);

				//means its on the player
				Destroy(gameObject);
			}
		}
	}
}
