using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
struct propInfo
{
	public Vector2 pos;
	public int type;
}

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
	int[,] mapArray;
	public int numberOfPonds;
	public float waterFrequency;//max 0.2
	public float smallPropFreq=0.8f;
	public float propFreq=0.4f;
	public float off;
	public float basenum;
	public IList propList=new ArrayList();
	GameObject smallPropParent;
	GameObject PropParent;

	// Use this for initialization
	void Start () {
		GameObject BaseObject=Instantiate (tilePrefab,new Vector3(mapWidth_units*-0.5f*h_offset,mapHeight_units*-0.5f*v_offset),Quaternion.identity) as GameObject;

		GenerateMap ();

		string path = "Environments/" + SheetName;
		subsprite = Resources.LoadAll<Sprite> (path) as Sprite[];

		GeneratePropInfo ();
		
		GeneratePropInstances ();

		for(int i=mapWidth_units;i>0;--i)
		{
			for(int j=0;j<mapHeight_units;++j)
			{
				GameObject thing=Instantiate (tilePrefab,new Vector3((i+j)*h_offset,-(i-j)*v_offset,1),Quaternion.identity) as GameObject;

				SetTileTextures(thing,i,j);
				
				int rot=UnityEngine.Random.Range (0,2);
				Vector3 scale=thing.transform.localScale;
				if(rot==0)
				{
					rot=-1;
				}
				scale.x=rot*scale.x;
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

	void SetTileTextures(GameObject thing,int i,int j)
	{
		SpriteRenderer rend=thing.GetComponent<SpriteRenderer>();
		int tileType=mapArray[i-1,j];
		switch(tileType)
		{
		case 0:
		{
			int num=UnityEngine.Random.Range(1,7);
			string another="fTile"+(num);
			Sprite t = Array.Find (subsprite, item => item.name == another);
			rend.sprite=t;
		}
			break;
		case 1:
		{	
			int num=UnityEngine.Random.Range(1,3);
			string another="fWater"+(num);
			Sprite t = Array.Find (subsprite, item => item.name == another);
			rend.sprite=t;
		}
			break;
		case 2:
		{
			int num=UnityEngine.Random.Range(1,3);
			string another="fMud"+(num);
			Sprite t = Array.Find (subsprite, item => item.name == another);
			rend.sprite=t;
		}
			break;
		}
		
	}

	void GenerateMap()
	{
		mapArray = new int[mapWidth_units, mapHeight_units];
		//considering each pond is on average 7x7 units large
		//a frequency of 0.5 would mean an average of 5 ponds(around 35x35 tiles of pond) in a 70x70 map
		
		//cal the frequency with ((wf*total size(width)/7)+(wf*totalSize(height)/7))/2
		//rand between +-20% of the frequency
		
		float freq = ((waterFrequency * mapWidth_units * 0.14286f) + (waterFrequency * mapHeight_units * 0.14286f)) *0.5f;//base number of ponds
		basenum = freq;
		float offset = UnityEngine.Random.Range (0, 41);
		freq *= 0.8f + (offset * 0.1f);
		freq = Mathf.Ceil (freq);
		numberOfPonds = (int)freq;
		off = offset;
		
		Vector2[] pondPos = new Vector2[(int)(freq)];
		
		//for loop that will loop for each pond center
		//do while loop
		//rand the position center of the pond
		//while its too close to center of another pond(within 10 tiles)
		//end of do while loop
		//recursive function that will spread the water here
		//end of for loop
		
		for (int i=0;i<pondPos.Length;++i)
		{
			Vector2 closest=new Vector2();
			Vector2 pond=pondPos[i];
			do{
				pond=new Vector2(UnityEngine.Random.Range(0,mapHeight_units),UnityEngine.Random.Range(0,mapWidth_units));
				foreach (Vector2 nearestpond in pondPos)
				{
					if(nearestpond.sqrMagnitude==0)
						break;
					if(closest.sqrMagnitude==0)
					{
						closest=nearestpond;
					}
					else if((pond-nearestpond).sqrMagnitude<(pond-closest).sqrMagnitude){
						closest=nearestpond;
					}
				}
				if(closest==null)
				{
					break;
				}
			}
			while((closest-pond).sqrMagnitude<196);
			mapArray[(int)pond.x,(int)pond.y]=1;
			growPond((int)pond.x,(int)pond.y,100);
		}
	}
	
	void growPond(int x,int y,float chance)
	{
		for(int i=-1;i<=1;i++)
		{
			for(int j=-1;j<=1;j++)
			{
				if(x+i>=0&&x+i<mapWidth_units)
				{
					if(y+j>=0&&y+j<mapHeight_units)
					{
						int c=UnityEngine.Random.Range(0,101);
						if(c<chance)
						{
							float nChance=UnityEngine.Random.Range(0.5f,0.9f);
							growPond (x+i,y+j,chance*nChance);
							if(chance>30)
								mapArray[x,y]=1;
							else
								if(mapArray[x,y]!=1)
									mapArray[x,y]=2;
						}
					}
				}
				int d=0;
				d+=1;
			}
		}
	}

	void GeneratePropInfo ()
	{
		float smallPropNum;
		smallPropNum = smallPropFreq*mapHeight_units*mapWidth_units;
		float propNum = propFreq * mapHeight_units * mapWidth_units;
		Vector3 tempVec = new Vector3 ();
		for (int i=0; i<smallPropNum; i++) {
			do
				tempVec=new Vector2(UnityEngine.Random.Range(0.0f,(float)mapHeight_units),UnityEngine.Random.Range(0.0f,(float)mapWidth_units));
			while(mapArray[(int)Mathf.Floor(tempVec.x),(int)Mathf.Floor(tempVec.y)]==1);
			{
				propInfo tempProp=new propInfo();
				tempProp.pos=tempVec;
				int tempType=UnityEngine.Random.Range(0,10);
				if(tempType==0)
				{
					tempProp.type=2;
				}
				else{
					tempProp.type=1;
				}
				propList.Add(tempProp);
			}
		}
		for (int i=0; i<propNum; i++) {
			int type=UnityEngine.Random.Range(3,13);
			if(type==9)
			{
				do
					tempVec=new Vector2(UnityEngine.Random.Range(0.0f,(float)mapHeight_units),UnityEngine.Random.Range(0.0f,(float)mapWidth_units));
				while(mapArray[(int)Mathf.Floor(tempVec.x),(int)Mathf.Floor(tempVec.y)]==0);
			}
			else
			{
				do
					tempVec=new Vector2(UnityEngine.Random.Range(0.0f,(float)mapHeight_units),UnityEngine.Random.Range(0.0f,(float)mapWidth_units));
				while(mapArray[(int)Mathf.Floor(tempVec.x),(int)Mathf.Floor(tempVec.y)]!=0);
			}
			propInfo tempProp=new propInfo();
			tempProp.pos=tempVec;
			tempProp.type=type;
			propList.Add(tempProp);
		}

	}

	void GeneratePropInstances()
	{
		smallPropParent=Instantiate (tilePrefab,new Vector3(mapWidth_units*-0.5f*h_offset,mapHeight_units*-0.5f*v_offset),Quaternion.identity) as GameObject;
		PropParent=Instantiate (tilePrefab,new Vector3(mapWidth_units*-0.5f*h_offset,mapHeight_units*-0.5f*v_offset),Quaternion.identity) as GameObject;
		foreach(propInfo temp in propList)
		{
			GameObject thing=Instantiate (tilePrefab,new Vector3((temp.pos.x+temp.pos.y)*h_offset,-(temp.pos.x-temp.pos.y)*v_offset,1),Quaternion.identity) as GameObject;
			SetPropTile(thing,(int)temp.type);
		}
	}

	void SetPropTile(GameObject target,int type)
	{
		SpriteRenderer rend=target.GetComponent<SpriteRenderer>();
		
		Vector3 scale = target.transform.localScale;
		scale = GetRandomScale (type,scale);
		if(type>=1&&type<13)
		{
			int s = UnityEngine.Random.Range (0, 2);
			if (s == 0) {
				scale.x = -scale.x;
			}
			target.transform.localScale = scale;

		}
		if (type >= 1 & type < 3) {
			string another = "grass" + (type);
			Sprite t = Array.Find (subsprite, item => item.name == another);
			rend.sprite = t;
			target.transform.parent=smallPropParent.transform;

		} else if (type >= 3 && type < 7) {
			string another = "rock" + (type-2);
			Sprite t = Array.Find (subsprite, item => item.name == another);
			rend.sprite = t;
			target.transform.parent=PropParent.transform;
		}
		else if(type >= 7 && type < 9)
		{
			string another = "ruins" + (type-6);
			Sprite t = Array.Find (subsprite, item => item.name == another);
			rend.sprite = t;
			target.transform.parent=PropParent.transform;
		}
		else if(type >= 9 && type < 13)
		{
			string another = "tswamp" + (type-8);
			Sprite t = Array.Find (subsprite, item => item.name == another);
			rend.sprite = t;
			target.transform.parent=PropParent.transform;
		}
	}
	Vector3 GetRandomScale(int type,Vector3 scale)
	{
		float xfactor;
		float yfactor;
		if (type == 1 || type == 2) {
			xfactor = UnityEngine.Random.Range (0.5f, 2.0f);
			yfactor = UnityEngine.Random.Range (xfactor * 0.8f, xfactor * 1.2f);
			scale.x *= xfactor;
			scale.y *= yfactor;
		} else if (type == 4 || type == 5) {
			
			xfactor = UnityEngine.Random.Range (0.9f, 1.3f);
			yfactor = UnityEngine.Random.Range (0.9f, 1.3f);
			scale.x *= xfactor;
			scale.y *= yfactor;
		} else if (type == 3 || type == 6) {
			xfactor = UnityEngine.Random.Range (1.0f, 1.3f);
			yfactor = UnityEngine.Random.Range (1.0f, 1.3f);
			scale.x *= xfactor;
			scale.y *= yfactor;

		} else if (type == 9) {
			xfactor = UnityEngine.Random.Range (1.0f, 1.3f);
			yfactor = UnityEngine.Random.Range (0.8f, 1.2f);
			scale.x *= xfactor;
			scale.y *= yfactor;
		} else if (type == 10||type==11) {
			
			xfactor = UnityEngine.Random.Range (1.0f, 1.3f);
			yfactor = UnityEngine.Random.Range (1.0f, 1.1f);
			scale.x*=xfactor;
			scale.y*=yfactor;
		}else if (type == 12) {
			
			xfactor = UnityEngine.Random.Range (0.9f, 1.2f);
			yfactor =	1.0f;
			scale.x*=xfactor;
			scale.y*=yfactor;
		}
		return scale;
	}
}
