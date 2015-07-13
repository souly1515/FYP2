using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {
	public Slot[,] slots;
	int maxX=12;
	int maxY=8;
	public bool showInventory;
	public GameObject slotPrefab;
	public GameObject weaponTemp;
	float inventoryTrans;//transition between not active and active
	// Use this for initialization
	void Start () {
		slots = new Slot[maxX, maxY];
		for(int i=0;i<maxX;i++)
		{
			for(int j=0;j<maxY;j++)
			{
				GameObject thing = Instantiate (slotPrefab,new Vector3(i*0.5f,j*0.5f),Quaternion.identity)as GameObject;
				thing.transform.parent=transform;
				thing.GetComponent<Slot>().x=i;
				thing.GetComponent<Slot>().y=j;
				slots[i,j]=thing.GetComponent<Slot>();
			}
		}
		GameObject tempGo=Instantiate(weaponTemp,new Vector3(0,0),Quaternion.identity)as GameObject;
		Weapon temp = tempGo.GetComponent<Weapon> ();
		temp.info.wtype = WeaponTypes.OH_MACE;
		temp.info.inventorySizeX = 2;
		temp.info.inventorySizeY = 2;
		ItemAdded (temp,0,0,0,0);
		showInventory = true;
		inventoryTrans = 0.0f;

	}
	
	// Update is called once per frame
	void Update () {
		if (inventoryTrans>0) {
			if(showInventory)
			{
				//move the inventory
			}
			else{

			}
		}
		if (!showInventory&&inventoryTrans<=0) {
			//dont render the inventory
		}
	}
	public void ItemRemoved(Weapon target)
	{
		for (int i=target.info.inventoryX; i<target.info.inventoryX+target.info.inventorySizeX; ++i) {
			for(int j=target.info.inventoryY;j<target.info.inventoryY+target.info.inventorySizeY;++j)
			{
				slots[i,j].occupied=false;
			}
		}
	}
	public void ItemRemoved (int x, int y)
	{
	
		if (slots [x, y].occupied) {
			GameObject temp=slots[x,y].weapon;
			Weapon target = temp.GetComponent<Weapon>();
			for (int i=target.info.inventoryX; i<target.info.inventoryX+target.info.inventorySizeX; ++i) {
				for(int j=target.info.inventoryX;j<target.info.inventoryY+target.info.inventorySizeY;++j)
				{
					slots[i,j].occupied=false;
				}
			}
		}
	}
	public bool AddItem(Weapon target)
	{
		//get weapon size
		//check if there is an empty space large enough for this
			//add item if there is
			//probably remove the item from the scene
			//return true
		//else
			//return false
		return false;
	}
	public bool ItemAdded(Weapon target,int x,int y,int refx,int refy)//for moving items only
	{
		if (target==null)
			return false;
		if(x-refx<0||x-refx+(target.info.inventorySizeX-1)>=maxX||y-refy<0||y-refy+(target.info.inventorySizeY-1)>=maxY)
		{
			if(target.info.inventoryX>=0&&target.info.inventoryX+(target.info.inventorySizeX-1)<maxX)//set back to the original pos
			{
				if(target.info.inventoryY>=0&&target.info.inventoryY+(target.info.inventorySizeY-1)<maxY)
					ItemAdded (target,target.info.inventoryX,target.info.inventoryY,0,0);
			}
			return false;
		}
		for (int i=x-refx; i<x-refx+(target.info.inventorySizeX-1); ++i) {
			for(int j=y-refy;j<y-refy+(target.info.inventorySizeY-1);++j)
			{
				if(slots[i,j].occupied)
				{
					if(target!=slots[i,j].weapon)//check if its going to overwrite itself
					{
						if(target.info.inventoryX>=0&&target.info.inventoryX+(target.info.inventorySizeX-1)<maxX)
						{
							if(target.info.inventoryY>=0&&target.info.inventoryY+(target.info.inventorySizeY-1)<maxY)
								ItemAdded (target,target.info.inventoryX,target.info.inventoryY,0,0);
						}
					}
					return false;
				}
			}
		}
		target.info.inventoryX = x-refx;
		target.info.inventoryY = y-refy;
		for (int i=x-refx; i<x-refx+target.info.inventorySizeX; ++i) {
			for(int j=y-refy;j<y-refy+target.info.inventorySizeY;++j)
			{
				slots[i,j].occupied=true;
				slots[i,j].weapon=target.gameObject;
			}
		}
		return true;
	}
}
