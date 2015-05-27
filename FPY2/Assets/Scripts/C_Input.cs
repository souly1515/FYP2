using UnityEngine;
using System.Collections;

public class C_Input: MonoBehaviour {
	
	C_Base baseScript;
	public LayerMask Interactables;
	public float clickRadius;
	// Use this for initialization
	void Start () {
		baseScript = GetComponent<C_Base> ();
		clickRadius = 0.5f;
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
				Collider2D target=Physics2D.OverlapCircle(Input.mousePosition,clickRadius,Interactables);
				if(target)
				{
					if(target.gameObject.layer==LayerMask.NameToLayer("Pickable"))
					{
						Weapon temp=target.GetComponent<Weapon>();
						//target.GetComponent<Weapon>()=gameObject.GetComponent<C_Base>().myWeap;
					}
				}
				else
					MoveMe ();

			}
			//end

			//basic mechanics: attacks with mouse

			//end
		}
		if(Input.GetMouseButtonDown(1))
		{
			if(!baseScript.inAttackAnimation)
				baseScript.Attack(1,Input.mousePosition);
		}

		//keyboard stuff
		if(Input.GetKeyDown(KeyCode.Space))
		{
			if(baseScript.weaponType==1)
				baseScript.weaponType=2;
			else
				baseScript.weaponType=1;
		}
		if(Input.GetKeyDown(KeyCode.I))
		{

		}
	}
	void MoveMe()
	{
		Vector2 moveTo = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		baseScript.SetNextPos(moveTo);
		baseScript.moving=true;
	}
}
