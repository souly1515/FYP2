using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {
	public Slot[,] slots;
	int maxX=12;
	int maxY=8;
	public bool showInventory;
	public GameObject slotPrefab;
	public GameObject weaponInInventory;
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
	public void ItemRemoved(weaponInfo target)
	{
		for (int i=target.inventoryX; i<target.inventoryX+target.inventorySizeX; ++i) {
			for(int j=target.inventoryY;j<target.inventoryY+target.inventorySizeY;++j)
			{
				slots[i,j].occupied=false;
			}
		}
	}
	public void ItemRemoved (int x, int y)
	{
	
		if (slots [x, y].occupied) {
			//GameObject temp=slots[x,y].weapon;
			weaponInfo target = slots[x,y].weaponScript;
			for (int i=target.inventoryX; i<target.inventoryX+target.inventorySizeX; ++i) {
				for(int j=target.inventoryX;j<target.inventoryY+target.inventorySizeY;++j)
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
						for(int j2=j;j2<j+sizeY&&pass;++j2)
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
			//add item if there is
			//probably remove the item from the scene
			if (AddItem (target.info, posX, posY)) {
				//remove item from scene
				Destroy (target.gameObject);
				return true;

			}
			else
			{
				return false;
			}
		} else {
			Debug.Log ("Failed due to lack of vaild space");
			return false;
		}
	}

	public bool AddItem(weaponInfo target,int posX,int posY)//this is for adding items
	{
		if (posX < 0 || posX + (target.inventorySizeX - 1) >= maxX || posY < 0 || posY + (target.inventorySizeY - 1) >= maxY) {//see if the things exceeds the bounds
			Debug.Log("Failed due to exceeded bounds");
			return false;
		} 
		for (int i=posX; i<posX+(target.inventorySizeX-1); ++i) {
			for(int j=posY;j<posY+(target.inventorySizeY-1);++j)
			{
				if(slots[i,j].occupied)
				{
					Debug.Log("Failed due to overlapping");
					return false;
				}
			}
		}
		target.inventoryX = posX;
		target.inventoryY = posY;
		for (int i=posX; i<posX+target.inventorySizeX; ++i) {
			for(int j=posY;j<posY+target.inventorySizeY;++j)
			{
				slots[i,j].occupied=true;
				slots[i,j].weaponScript=target;
			}
		}
		Vector3 weaPos = slots [Mathf.FloorToInt (target.inventoryX + target.inventorySizeX * 0.5f), Mathf.FloorToInt (target.inventoryY + target.inventorySizeY * 0.5f)].transform.position - new Vector3 (0.25f, 0.25f);
		GameObject temp = Instantiate (weaponInInventory,weaPos,Quaternion.identity)as GameObject;
		return true;
	}

	//x,y is the pos of the item, refx,refy is the moving point
	public bool ItemMoved(weaponInfo target,int x,int y,int refx,int refy)//for moving items only
	{
		if (target == null) {
			Debug.Log("Failed due to null weapon");
			return false;
		
		}if(x-refx<0||x-refx+(target.inventorySizeX-1)>=maxX||y-refy<0||y-refy+(target.inventorySizeY-1)>=maxY)//see if the things exceeds the bounds
		{
			if(target.inventoryX>=0&&target.inventoryX+(target.inventorySizeX-1)<maxX)//set back to the original pos
			{
				if(target.inventoryY>=0&&target.inventoryY+(target.inventorySizeY-1)<maxY)
					ItemMoved (target,target.inventoryX,target.inventoryY,0,0);
			}
			Debug.Log("Failed due to exceeded bounds");
			return false;
		}
		for (int i=x-refx; i<x-refx+(target.inventorySizeX); ++i) {
			for(int j=y-refy;j<y-refy+(target.inventorySizeY);++j)
			{
				if(slots[i,j].occupied)
				{
					ItemMoved (target,target.inventoryX,target.inventoryY,0,0);
					Debug.Log("Failed due to overlapping");
					return false;
				}
			}
		}
		target.inventoryX = x-refx;
		target.inventoryY = y-refy;
		for (int i=x-refx; i<x-refx+target.inventorySizeX; ++i) {
			for(int j=y-refy;j<y-refy+target.inventorySizeY;++j)
			{
				slots[i,j].occupied=true;
				slots[i,j].weaponScript=target;
			}
		}
		Vector3 weaPos = slots [Mathf.FloorToInt (target.inventoryX + target.inventorySizeX * 0.5f), Mathf.FloorToInt (target.inventoryY + target.inventorySizeY * 0.5f)].transform.position - new Vector3 (0.25f, 0.25f);
		GameObject temp = Instantiate (weaponInInventory,weaPos,Quaternion.identity)as GameObject;
		return true;
	}
}
