using UnityEngine;
using System.Collections;

public class SkillPooler : MonoBehaviour {
	Queue pool;
	ArrayList usedpool;
	public GameObject skillPrefab;
	public int startPoolSize;
	// Use this for initialization
	void Start () {
		pool = new Queue ();
		usedpool = new ArrayList ();
		GameObject temp;
		for(int i=0;i<startPoolSize;++i)
		{
			temp=Instantiate(skillPrefab)as GameObject;
			pool.Enqueue(temp);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	GameObject Pull()
	{
		GameObject temp;
		if (pool.Count > 0) {
			temp = pool.Dequeue () as GameObject;
		} else {
			temp = Instantiate (skillPrefab)as GameObject;
		}
		usedpool.Add (temp);
		temp.SetActive (true);
		return temp;
	}
	void Deactivate(GameObject temp)
	{
		usedpool.Remove (temp);
		pool.Enqueue (temp);
		temp.SetActive (false);
	}
}
