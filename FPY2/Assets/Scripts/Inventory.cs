using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {
	public Slot[,] slots;
	int maxX=12;
	int maxY=8;
	public bool showInventory;
	public GameObject slotPrefab;
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
		Weapon temp =new Weapon();
		temp.info.wtype = WeaponTypes.OH_MACE;
		temp.info.inventorySizeX = 2;
		temp.info.inventorySizeY = 2;
		ItemAdded (temp,0,0);
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
			Weapon target = slots[x,y].weapon;
			for (int i=target.info.inventoryX; i<target.info.inventoryX+target.info.inventorySizeX; ++i) {
				for(int j=target.info.inventoryX;j<target.info.inventoryY+target.info.inventorySizeY;++j)
				{
					slots[i,j].occupied=false;
				}
			}
		}
	}
	public bool ItemAdded(Weapon target,int x,int y)
	{
		if (target==null)
			return false;
		if(x<0||x+target.info.inventorySizeX>=maxX||y<0||y+target.info.inventorySizeY>=maxY)
		{
			if(target.info.inventoryX>=0&&target.info.inventoryX+target.info.inventorySizeX<maxX)
			{
				if(target.info.inventoryY>=0&&target.info.inventoryY+target.info.inventorySizeY<maxY)
					ItemAdded (target,target.info.inventoryX,target.info.inventoryY);
			}
			return false;
		}
		for (int i=x; i<x+target.info.inventorySizeX; ++i) {
			for(int j=y;j<y+target.info.inventorySizeY;++j)
			{
				if(slots[i,j].occupied)
				{
					if(target!=slots[i,j].weapon)//check if its going to overwrite itself
					{
						if(target.info.inventoryX>=0&&target.info.inventoryX<maxX)
						{
							if(target.info.inventoryY>=0&&target.info.inventoryY<maxY)
								ItemAdded (target,target.info.inventoryX,target.info.inventoryY);
						}
						ItemAdded (target,target.info.inventoryX,target.info.inventoryY);
					}
					return false;
				}
			}
		}
		target.info.inventoryX = x;
		target.info.inventoryY = y;
		for (int i=x; i<x+target.info.inventorySizeX; ++i) {
			for(int j=y;j<y+target.info.inventorySizeY;++j)
			{
				slots[i,j].occupied=true;
				slots[i,j].weapon=target;
			}
		}
		return true;
	}
}
