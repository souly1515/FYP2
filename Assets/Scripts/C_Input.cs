using UnityEngine;
using System.Collections;

public class C_Input: MonoBehaviour {
	
	C_Base baseScript;
	public LayerMask Interactables;
	public float clickRadius;
	public GameObject inventoryGO;
	public Inventory inventory;
	public bool holdingItem;
	public weaponInfo holdingWeapon;
	public GameObject weaponSprite;
	public int refX,refY;
	Animator anim;

	// Use this for initialization
	void Start () {
		baseScript = GetComponent<C_Base> ();
		clickRadius = 0.1f;
		//inventory = inventoryGO.GetComponent<Inventory> ();
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (baseScript.dead) {
			if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime>=0)
			{

			}
			return;
		}
		if (weaponSprite) {
			Vector3 t=Camera.main.ScreenToWorldPoint(Input.mousePosition);
			t.z=0;


			t.x+=0.25f-refX*0.5f;
			t.y+=0.25f-refY*0.5f;

			weaponSprite.transform.position=t;
		}
		if (Input.GetMouseButton (0)) {
			//skills and weapons 1: casting weapons skills

			//test if there are any skills on cast step

				//skills and weapons 1: weapon specific skills
				//end

			//end

			//else

			//basic mechanics: movement with mouse
			//check if mouse pos is not on top of obstacle or enemy
			
			if (!baseScript.inAttackAnimation)//should not be able to do anything
			{
				Vector2 point=Camera.main.ScreenToWorldPoint(Input.mousePosition);
				Collider2D[] targetArray=Physics2D.OverlapCircleAll(point,clickRadius,Interactables);
				if(targetArray.Length>0)
				{
					foreach(Collider2D target in targetArray)
					{
						if(target.gameObject.layer==LayerMask.NameToLayer("UI"))
						{
							if(!holdingItem)
							{
								Slot temp=target.gameObject.GetComponent<Slot>();
								if(temp)
								{
									int x,y;
									x=temp.x;
									y=temp.y;
									if(temp.occupied)
									{
										holdingWeapon=temp.weaponScript;
										//Weapon w_script=holdingWeapon.GetComponent<Weapon>();
										holdingItem=true;
										refX=x-holdingWeapon.inventoryX;
										refY=y-holdingWeapon.inventoryY;
										inventory.ItemRemoved(holdingWeapon);
									}
								}
							}
							if(target.gameObject.tag=="InventoryWeapon")
							{
								weaponSprite=target.gameObject;
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
				if(holdingWeapon!=(null))
				{
					Collider2D[] targetArray=Physics2D.OverlapCircleAll(Camera.main.ScreenToWorldPoint(Input.mousePosition),clickRadius,Interactables);
					bool failed=true;
					foreach(Collider2D target in targetArray)
					{
						if(target&&failed)
						{
							if(target.gameObject.layer==LayerMask.NameToLayer("UI"))
							{
								Slot temp=target.gameObject.GetComponent<Slot>();
								if(temp)
								{
									if(holdingWeapon!=(null))
									{
										//Weapon w_script=holdingWeapon.GetComponent<Weapon>();
										inventory.ItemMoved(holdingWeapon,temp.x,temp.y,refX,refY);
										if(weaponSprite)
										{
											Destroy(weaponSprite);
											weaponSprite=null;
										}
										failed=false;
									}
								}
							}

						}
					}
					if(failed)
					{
						//Weapon w_script=holdingWeapon.GetComponent<Weapon>();
						inventory.AddItem(holdingWeapon,0,0);
						if(weaponSprite)
						{
							Destroy(weaponSprite);
							weaponSprite=null;
						}
					}
					holdingWeapon=null;
				}
				holdingItem=false;
			}
			else{
				Vector2 point=Camera.main.ScreenToWorldPoint(Input.mousePosition);
				Collider2D target=Physics2D.OverlapCircle(point,clickRadius,Interactables);
				if(target)
				{
					if(target.gameObject.layer==LayerMask.NameToLayer("Pickable"))
					{
						Weapon temp=target.GetComponent<Weapon>();
						inventory.PickItem(temp);
						//target.GetComponent<Weapon>()=gameObject.GetComponent<C_Base>().myWeap;
					}
				}
			}
		}
		//if(Input.GetMouseButtonDown(1))
		if(Input.GetKeyDown(KeyCode.Q))
		{
			baseScript.Attack(1,1,Input.mousePosition);
		}
		//keyboard stuff
		if(Input.GetKeyDown(KeyCode.I))
		{

		}
		if (Input.GetKeyDown (KeyCode.W)) {
			baseScript.Attack(2,1,Input.mousePosition);
		}
		if (Input.GetKeyDown (KeyCode.E)) {
			baseScript.Attack(3,1,Input.mousePosition);
		}
		if(Input.GetKeyDown(KeyCode.A))
		{
			baseScript.Attack(1,2,Input.mousePosition);
		}
		if(Input.GetKeyDown(KeyCode.S))
	    {
			baseScript.Attack(2,2,Input.mousePosition);
		}
	}
	void MoveMe()
	{
		Vector2 moveTo = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		baseScript.SetNextPos(moveTo);
		baseScript.moving=true;
	}
}
