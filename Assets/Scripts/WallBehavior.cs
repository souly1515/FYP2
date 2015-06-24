using UnityEngine;
using System.Collections;

public class WallBehavior : MonoBehaviour {
	public LayerMask others;
	public float zpos=0;
	Collider2D[] nearby;
	float newA;
	// Use this for initialization
	void Start () {
		nearby = new Collider2D[20];
	}
	
	// Update is called once per frame
	void Update () {
		
		SpriteRenderer temp= gameObject.GetComponent<SpriteRenderer>();
		Color tempColor=temp.color;
		Collider2D temp2 = Physics2D.OverlapCircle (gameObject.transform.position, 1, others);
		int num = Physics2D.OverlapCircleNonAlloc (gameObject.transform.position, 5, nearby);
		float dist = 10.0f;
		float nearestA = 1.0f;
		for (int i=0; i<num; i++) {
			
			float dist2=(nearby[i].gameObject.transform.position-gameObject.transform.position).magnitude;
			if(nearby[i].gameObject.GetComponent<SpriteRenderer>().color.a<nearestA)
			{
				nearestA=nearby[i].gameObject.GetComponent<SpriteRenderer>().color.a;
				//if(dist2<dist)
					dist=dist2;
			}
		}

		if(temp2)
		{
			if(Vector3.Dot(transform.position-temp2.gameObject.transform.position,new Vector3(-0.5f,0.5f))<0)
			{
					newA=0.3f;
					tempColor.a=Mathf.Lerp(tempColor.a,newA,0.1f);
					temp.color=tempColor;
					gameObject.transform.position = new Vector3(gameObject.transform.position.x,gameObject.transform.position.y,-1);

					return;
			}
		}
		newA = nearestA+(dist*0.1f)+0.1f;
		tempColor.a = Mathf.Lerp (tempColor.a, newA, 0.1f);
		//tempColor.a = 1.0f;
		temp.color = tempColor;
		gameObject.transform.position = new Vector3(gameObject.transform.position.x,gameObject.transform.position.y,zpos);
	}
}
