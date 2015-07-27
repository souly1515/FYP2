using UnityEngine;
using System.Collections;

public class SpiritMovementHead : MonoBehaviour {
	public Vector3 nextPos;
	Vector3 transpos;//use this pos for as the next pos
	Vector3 dir;
	public Vector3 lastPos;
	Vector3 LastNextPos;
	public float spd;
	public float maxSpd=3.0f;
	public float acc=1.0f;
	float lerpTimeLastPos = 0;
	GameObject nextChild;
	SpiritMovement nextScript;
	// Use this for initialization
	void Start () {
		Transform temp = transform.FindChild ("Spirit");
		if (temp) {
			nextChild = temp.gameObject;
			nextScript=nextChild.GetComponent<SpiritMovement>();
		}
		LastNextPos = nextPos;

	}

	// Update is called once per frame
	void Update () {
		if (LastNextPos != nextPos) {
			LastNextPos=nextPos;
			lastPos=transpos;
			lerpTimeLastPos=0;
		}
		lerpTimeLastPos += Time.deltaTime;
		transpos = Vector3.Lerp (lastPos, nextPos, lerpTimeLastPos);
		if (transpos != transform.position) {
			dir = transpos - transform.position;
			if (dir.sqrMagnitude < 1f) {
				if(dir.sqrMagnitude<0.0005f)
					transform.position=transpos;
				else
				{
					if((nextPos-transform.position).sqrMagnitude<0.0005f)
					{
						dir.Normalize ();
						spd-=acc*Time.deltaTime;
						if(spd<0.2f)
							spd=0.2f;
						transform.position+=dir*spd*Time.deltaTime;
						nextScript.stopping = true;
					}
					else
					{
						dir.Normalize ();
						spd += acc * Time.deltaTime;
						if(spd>maxSpd)
							spd=maxSpd;
						transform.position+=dir*spd*Time.deltaTime;
						if(nextScript)
						{
							nextScript.stopping=false;
							nextScript.nextDir=-dir;
						}
					}
				}
			} else {
				dir.Normalize ();
				spd += acc * Time.deltaTime;
				if(spd>maxSpd)
					spd=maxSpd;
				transform.position+=dir*spd*Time.deltaTime;
				if(nextScript)
				{
					nextScript.stopping=false;
					nextScript.nextDir=-dir;
				}
			}
		} else {
			if(nextPos!=transform.position)
			{
				spd=0;
				if(nextScript)
					nextScript.stopping=true;
			}
		}
	}
}
