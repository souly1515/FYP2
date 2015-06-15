using UnityEngine;
using System.Collections;
using System;


public class MapSetup : MonoBehaviour {
	public int mapWidth_units;
	public int mapHeight_units;
	public float h_offset;//tiles next to each other's horizontal offset
	public float v_offset;//tiles next to each other's vertical offset
	public float z_offset;//tiles ontop of each others' vertical offset
	public GameObject tilePrefab;
	public GameObject wallPrefab;
	Sprite[] subsprite;
	public string SheetName;
	// Use this for initialization
	void Start () {
		GameObject BaseObject=Instantiate (tilePrefab,new Vector3(mapWidth_units*-0.5f*h_offset,mapHeight_units*-0.5f*v_offset),Quaternion.identity) as GameObject;
		string path = "Environments/" + SheetName;
		subsprite = Resources.LoadAll<Sprite> (path) as Sprite[];
		for(int i=mapWidth_units;i>0;--i)
		{
			for(int j=0;j<mapHeight_units;++j)
			{
				GameObject thing=Instantiate (tilePrefab,new Vector3((i+j)*h_offset,-(i-j)*v_offset,1),Quaternion.identity) as GameObject;
				SpriteRenderer rend=thing.GetComponent<SpriteRenderer>();
				int num=UnityEngine.Random.Range(1,11);
				int rot=UnityEngine.Random.Range (0,2);
				if(rot==0)
				{
					rot=-1;
				}
				Vector3 scale=thing.transform.localScale;
				scale.x=rot*scale.x;
				string another="fTile"+(num);
				Sprite t = Array.Find (subsprite, item => item.name == another);
				rend.sprite=t;
				thing.transform.localScale=scale;
				thing.transform.parent=BaseObject.transform;
				if(i==6&&j>=0&&j<5)
				{
					thing=Instantiate (wallPrefab,new Vector3((i+j)*h_offset,-(i-j)*v_offset+z_offset*1,1-1*0.01f),Quaternion.identity) as GameObject;
					thing.GetComponent<WallBehavior>().zpos=1-1*0.01f;
					thing.layer=LayerMask.NameToLayer("Background");
					//thing.transform.parent=BaseObject.transform;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
