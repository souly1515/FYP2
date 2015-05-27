using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {
	Slot[,] slots;
	public bool showInventory;
	public GameObject slotPrefab;
	float inventoryTrans;//transition between not active and active
	// Use this for initialization
	void Start () {
		slots = new Slot[12, 8];
		for(int i=0;i<12;i++)
		{
			for(int j=0;j<8;j++)
			{
				GameObject thing = Instantiate (slotPrefab,new Vector3(i*0.5f,j*0.5f),Quaternion.identity)as GameObject;
				thing.transform.parent=transform;
				slots[i,j]=thing.GetComponent<Slot>();
			}
		}
		showInventory = false;
		inventoryTrans = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (inventoryTrans>0) {
			if(showInventory)
			{
				//move the object
			}
			else{

			}
		}
		if (showInventory||inventoryTrans>0) {

		}
	}
	public void ItemRemoved(Weapon target)
	{
		for (int i=target.info.inventoryX; i<target.info.inventoryX+target.info.inventorySizeX; ++i) {
			for(int j=target.info.inventoryX;j<target.info.inventoryY+target.info.inventorySizeY;++j)
			{
				slots[i,j].occupied=false;
			}
		}
	}
	public bool ItemAdded(Weapon target)
	{
		for (int i=target.info.inventoryX; i<target.info.inventoryX+target.info.inventorySizeX; ++i) {
			for(int j=target.info.inventoryX;j<target.info.inventoryY+target.info.inventorySizeY;++j)
			{
				if(slots[i,j].occupied)
				{
					return false;
				}
			}
		}
		
		for (int i=target.info.inventoryX; i<target.info.inventorySizeX; ++i) {
			for(int j=target.info.inventoryX;j<target.info.inventorySizeY;++j)
			{
				slots[i,j].occupied=true;
				slots[i,j].weapon=target;
			}
		}
		return true;
	}
}
