using UnityEngine;
using System.Collections;

public class C_Input: MonoBehaviour {
	
	C_Base baseScript;
	public LayerMask Interactables;
	public float clickRadius;
	public GameObject inventoryGO;
	public Inventory inventory;
	public bool holdingItem;
	public GameObject holdingWeapon;
	public int refX,refY;
	Animator anim;

	// Use this for initialization
	void Start () {
		baseScript = GetComponent<C_Base> ();
		clickRadius = 0.1f;
		inventory = inventoryGO.GetComponent<Inventory> ();
		anim = GetComponent<Animator> ();
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
				Vector2 point=Camera.main.ScreenToWorldPoint(Input.mousePosition);
				Collider2D target=Physics2D.OverlapCircle(point,clickRadius,Interactables);
				if(target)
				{
					if(target.gameObject.layer==LayerMask.NameToLayer("Pickable"))
					{
						Weapon temp=target.GetComponent<Weapon>();
						//target.GetComponent<Weapon>()=gameObject.GetComponent<C_Base>().myWeap;
					}
					else if(target.gameObject.layer==LayerMask.NameToLayer("UI"))
					{
						Slot temp=target.gameObject.GetComponent<Slot>();
						if(temp)
						{
							int x,y;
							x=temp.x;
							y=temp.y;
							if(temp.occupied)
							{
								holdingWeapon=temp.weapon;
								Weapon w_script=holdingWeapon.GetComponent<Weapon>();
								holdingItem=true;
								refX=x-w_script.info.inventoryX;
								refY=y-w_script.info.inventoryY;
								inventory.ItemRemoved(w_script);

							}

						}
					}
				}
				else
					MoveMe ();

			}
			//end

			//basic mechanics: attacks with mouse

			//end
		}
		if (Input.GetMouseButtonUp (0)) {
			if(holdingItem==true)
			{
				Collider2D target=Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition),clickRadius,Interactables);
				bool failed=false;
				if(target)
				{
					if(target.gameObject.layer==LayerMask.NameToLayer("UI"))
					{
						Slot temp=target.gameObject.GetComponent<Slot>();
						if(temp)
						{
							if(holdingWeapon)
							{
								Weapon w_script=holdingWeapon.GetComponent<Weapon>();
								inventory.ItemAdded(w_script,temp.x,temp.y,refX,refY);
							}
						}
						else
							failed=true;
					}
					else
						failed=true;
				}
				else 
					failed=true;
				if(failed)
				{
					Weapon w_script=holdingWeapon.GetComponent<Weapon>();
					inventory.ItemAdded(w_script,-1,-1,0,0);
				}
				holdingWeapon=null;
			}
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
		if (Input.GetKeyDown (KeyCode.Q)) {
			baseScript.attackAnimation();
		}
	}
	void MoveMe()
	{
		Vector2 moveTo = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		baseScript.SetNextPos(moveTo);
		baseScript.moving=true;
	}
}
