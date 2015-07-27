using UnityEngine;
using System.Collections;

public class SpiritAI : MonoBehaviour {
	public Vector3 CircleTarget;
	public float dist=1.0f;
	public float life=7.0f;
	public float angle;//used to find the real angle
	public GameObject target;
	public float acc=0.05f;
	public bool released=false;
	public bool d;
	SpiritMovementHead move;
	Animator anim;
	// Use this for initialization
	void Start () {
		move = GetComponent<SpiritMovementHead> ();
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!released) {
			angle+=Time.deltaTime*acc;
			if(angle>1)
				angle=0;
			float a=0;
			if(d)
				a=Mathf.Lerp(0,2*Mathf.PI,angle);//legit angle
			else
				a=Mathf.Lerp(2*Mathf.PI,0,angle);//legit angle

			Vector3 t=new Vector3();
			t.x=Mathf.Cos(a);
			t.y=Mathf.Sin(a);
			t*=dist;
			if(t.y>CircleTarget.y)
				t.z=-1;
			else
				t.z=0;
			if(move)
				move.nextPos=CircleTarget+t;
		} else {
			AnimatorStateInfo t=anim.GetCurrentAnimatorStateInfo(0);
			life-=Time.deltaTime;
			if(life<=0)
			{
				move.enabled=false;
				anim.SetTrigger("Explode");
			}
			else{
				
				move.nextPos=target.transform.position;
			}
			if(t.IsTag("Explode")&&t.normalizedTime>=1&&life<=0)
			{
				Destroy(gameObject);
			}
		}
	}

	public void Shoot(GameObject nTarget)
	{
		released = true;
		target = nTarget;
	}
}
