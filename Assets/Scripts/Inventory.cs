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
		ItemMoved (temp,0,0,0,0);
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
	public bool PickItem(Weapon target)
	{
		//get weapon size
		int sizeX = target.info.inventorySizeX;
		int sizeY = target.info.inventorySizeY;
		bool validSpace=false;
		int posX, posY;
		posX = posY = 0;
		//check if there is an empty space large enough for this
		for (int i=0; i<maxX&&!validSpace; ++i) {
			for(int j=0;j<maxY&&!validSpace;++j)
			{
				if(!slots[i,j].occupied)
				{
					bool pass=true;
					for(int i2=i;i2<i+sizeX&&pass;++i2)//check if the empty space found is large enough
					{
						for(int j2=i;j2<i+sizeY&&pass;++j2)
						{
							if(slots[i2,j2].occupied)//there is something in where the weapon should be
								pass=false;
						}
					}
					if(pass)//if it is then there is a valid space
					{
						validSpace=true;
						posX=i;
						posY=j;
					}
				}
			}
		}
		if (validSpace) {
			if(AddItem(target,posX,posY))
			{
				//remove item from scene
				Destroy(target.gameObject);

			}
			//add item if there is
			//probably remove the item from the scene
			return true;
		}
		else
			return false;
	}

	public bool AddItem(Weapon target,int posX,int posY)//this is for adding items
	{
		if (posX < 0 || posX + (target.info.inventorySizeX - 1) >= maxX || posY < 0 || posY + (target.info.inventorySizeY - 1) >= maxY) {//see if the things exceeds the bounds
			return false;
		} 
		for (int i=posX; i<posX+(target.info.inventorySizeX-1); ++i) {
			for(int j=posY;j<posY+(target.info.inventorySizeY-1);++j)
			{
				if(slots[i,j].occupied)
				{
					return false;
				}
			}
		}
		target.info.inventoryX = posX;
		target.info.inventoryY = posY;
		for (int i=posX; i<posX+target.info.inventorySizeX; ++i) {
			for(int j=posY;j<posY+target.info.inventorySizeY;++j)
			{
				slots[i,j].occupied=true;
				slots[i,j].weaponScript=target.gameObject.GetComponent<Weapon>().info;
			}
		}
		return true;
	}

	//x,y is the pos of the item, refx,refy is the moving point
	public bool ItemMoved(Weapon target,int x,int y,int refx,int refy)//for moving items only
	{
		if (target==null)
			return false;
		if(x-refx<0||x-refx+(target.info.inventorySizeX-1)>=maxX||y-refy<0||y-refy+(target.info.inventorySizeY-1)>=maxY)//see if the things exceeds the bounds
		{
			if(target.info.inventoryX>=0&&target.info.inventoryX+(target.info.inventorySizeX-1)<maxX)//set back to the original pos
			{
				if(target.info.inventoryY>=0&&target.info.inventoryY+(target.info.inventorySizeY-1)<maxY)
					ItemMoved (target,target.info.inventoryX,target.info.inventoryY,0,0);
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
								ItemMoved (target,target.info.inventoryX,target.info.inventoryY,0,0);
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
