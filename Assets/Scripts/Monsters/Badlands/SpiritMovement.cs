using UnityEngine;
using System.Collections;

public class SpiritMovement : MonoBehaviour {
	Vector3 dir;
	public Vector3 nextDir;
	bool changingDir=false;
	public float maxDist=0.2f;
	public float dist;
	public bool stopping = false;
	GameObject nextChild;
	SpiritMovement nextScript;
	float velUpdateInterval=0.05f;
	float timeleft;
	float lerpdir = 0;
	float lerpdist=0;
	// Use this for initialization
	void Start () {
		Transform temp = transform.FindChild ("Spirit");
		if (temp) {
			nextChild = temp.gameObject;
			nextScript=nextChild.GetComponent<SpiritMovement>();
			//nextScript.spdLimit=spdLimit*0.98f;
		}
		timeleft=velUpdateInterval;
		maxDist = 0.2f;
	}

	// Update is called once per frame
	void Update () {
		timeleft -= Time.deltaTime;
		if (timeleft <= 0&&!changingDir) {
			timeleft = velUpdateInterval;
			dir=nextDir;
			lerpdir=0;
		}
		if (changingDir) {
			if(dir==nextDir)
				changingDir=false;
			lerpdir+=Time.deltaTime;
			if(lerpdir>1)
				lerpdir=1;
			dir=Vector3.Lerp(dir,nextDir,lerpdir);
		}

		if(nextScript)
			nextScript.nextDir = dir;

		if (!stopping) {
			lerpdist+=Time.deltaTime;
			if(lerpdist>1)
				lerpdist=1;
			dist=Mathf.Lerp(0,maxDist,lerpdist);
			Vector3 temp=dir*dist;
			transform.localPosition=temp;
			if(nextScript)
				nextScript.stopping=false;
		} else {
			lerpdist-=Time.deltaTime;
			if(lerpdist<0)
				lerpdist=0;
			dist=Mathf.Lerp(0,maxDist,lerpdist);
			Vector3 temp=dir*dist;
			transform.localPosition=temp;
			if(nextScript)
				nextScript.stopping=true;
		}
	}
}
