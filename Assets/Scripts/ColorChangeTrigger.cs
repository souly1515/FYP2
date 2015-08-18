using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ColorChangeTrigger : MonoBehaviour {
	public float sValue=0.0f;
	public float eValue=1.0f;
	public bool triggered=false;
	public float transist=0.0f;
	Image img;
	public float timeleft=5.0f;
	StartLevel s;
	// Use this for initialization
	void Start () {
		img = GetComponent<Image> ();
		s = GetComponent<StartLevel> ();
	}
	
	// Update is called once per frame
	void Update () {
		timeleft -= Time.deltaTime;
		if (timeleft <= 0)
			triggered = true;
		if (s)
			s.enabled = false;
		if(triggered)
		{
			if(s)
				s.enabled=true;
			transist+=Time.deltaTime*0.25f;
			if(transist>=1)
				transist=1;
			Color t=new Color(1,1,1);
			t.a=Mathf.Lerp(sValue,eValue,transist);
			img.color=t;
		}
	}
}
