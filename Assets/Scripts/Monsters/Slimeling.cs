using UnityEngine;
using System.Collections;

public class Slimeling : MonoBehaviour {
	public Vector3 velocity;
	float lerpProgress;
	public Vector3 originalVel;
	bool initialised=false;
	public LayerMask targets;
	public float life;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
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
		if((targets.value & (1 << col.gameObject.layer)) > 0)
		{
			C_Base script=col.gameObject.GetComponent<C_Base>();
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
