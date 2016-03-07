using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AcidGasControl : MonoBehaviour {

	public float TimeLeft = 20;

	public float GasLife = 3.0f;

	public int MaxGasClouds = 5;

	float TimeBeforeNewCloud;

	public float CloudSpawnRate = 0.2f;

	public GameObject GasCloudPrefab = null;

	//range in which i wil spawn the clouds
	public Vector3 Range = new Vector3(2.5f, 2.5f);

	public List<GameObject> ChildList = new List<GameObject>();

	// Use this for initialization
	void Start () {
		//TimeBeforeNewCloud = CloudSpawnRate;
	}
	
	// Update is called once per frame
	void Update () {
		TimeLeft -= Time.deltaTime;
		if(TimeLeft > 0)
		{
			if(ChildList.Count < MaxGasClouds)
			{
				if(TimeBeforeNewCloud <=0)
				{
					if(GasCloudPrefab != null)
					{
						Vector3 SpawnPos = new Vector3();

						SpawnPos.x = Random.Range(-Range.x, Range.x);
						SpawnPos.y = Random.Range(-Range.y, Range.y);

						SpawnPos += transform.position;

						GameObject temp = Instantiate(GasCloudPrefab, SpawnPos, Quaternion.identity) as GameObject;

						temp.transform.SetParent(transform);
						TimeBeforeNewCloud = CloudSpawnRate;

						AcidGas gScript = temp.GetComponent<AcidGas>();

						gScript.TimeLeft = Random.Range(GasLife * 0.8f, GasLife * 1.2f);

						ChildList.Add(temp);
					}
				}
				else
				{
					TimeBeforeNewCloud -= Time.deltaTime;
				}
			}
		}
	}
}
