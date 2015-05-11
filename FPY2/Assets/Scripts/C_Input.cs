using UnityEngine;
using System.Collections;

public class C_Input: MonoBehaviour {
	
	C_Base baseScript;
	// Use this for initialization
	void Start () {
		baseScript = GetComponent<C_Base> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton (0)) {
			//skills and weapons 1: casting weapons skills

			//test if there are any skills on cast step

				//skills and weapons 1: weapon specific skills
				//end

			//end

			//else

			//basic mechanics: movement with mouse

			//check if mouse pos is not on top of obstacle or enemy
			{
				Vector2 moveTo = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				baseScript.SetNextPos(moveTo);
				baseScript.moving=true;
			}
			//end

			//basic mechanics: attacks with mouse

			//end
		}
		if(Input.GetMouseButtonDown(1))
	   {
			//basic skills
			//some math stuff here to cal direction and stuff
			//create attack obj?
			//give attack obj current stats and weapon stats
			//get weapon stats from C_Base
		}

		//keyboard stuff
		if(Input.GetKeyDown(KeyCode.Space))
		{
			baseScript.Attack(0);
		}
	}
}
