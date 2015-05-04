using UnityEngine;
using System.Collections;

public class C_Stats
{
	public int Str;//strength
	public int Dex;//dexterity
	public int Int;//intelligence
	public int End;//endurance
	public int Wil;//willpower, potentially haki thing
	public int moveSpd;//move speed
}

public class C_Base: MonoBehaviour {
	public Vector2 nextPos;
	public C_Stats stats;
	public float moveSpd;
	public bool moving;
	Rigidbody2D rb;
	//debug
	public float cameraSpd;
	public Vector2 CurrentPos;
	//end of debug

	//inventory

	//equipment

	//

	public void SetNextPos(Vector2 newPos) {
		nextPos = newPos;
	}

	// Use this for initialization
	void Start () {
		cameraSpd = 0.5f;
		moveSpd = 4;
		moving = false;
		rb = GetComponent<Rigidbody2D> ();
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "inpassable") {
			moving = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		//basic mechanics: movement with mouse
		if (moving) {
			float step = moveSpd * Time.deltaTime;

			Vector3 direction=(Vector3)(nextPos)-transform.position;
			direction.Normalize();

			rb.velocity=direction*moveSpd;

			Vector2 temp2=(Vector2)(transform.position)-nextPos;
			if(temp2.SqrMagnitude()<0.1)
			{
				moving=false;
				rb.velocity=new Vector2(0,0);
			}

			//basic mechanics: camera movement with character
			Vector3 temp = transform.position;
			temp.z = -10;
			//Camera.main.transform.position = Vector3.Lerp (Camera.main.transform.position, temp, cameraSpd);

			float xDif = transform.position.x - Camera.main.transform.position.x;

			if (Mathf.Abs (xDif) < 5) {
				xDif = 0;
			} else if (xDif > 0) {
				xDif -= 5;
			} else {
				xDif += 5;
			}

			float yDif = transform.position.y - Camera.main.transform.position.y;

			if (Mathf.Abs (yDif) < 3) {
				yDif = 0;
			} else if (yDif > 0) {
				yDif -= 3;
			} else {
				yDif += 3;
			}
			Camera.main.transform.position += new Vector3 (xDif, yDif, 0);
			//end of basic mechanics: camera movement with character

			CurrentPos = transform.position;
			//end of basic mechanics: movement with mouse
		}
	}
}
