using UnityEngine;
using System.Collections;

public class AcidPod : MonoBehaviour {
	public float TimeLeft = 5.0f;
	public GameObject GasSpawnerPrefab = null;
	bool CreatedSpawner = false;

	Animator anim = null;

	// Use this for initialization
	void Start () {
		if(gameObject.GetComponent<Animator>() != null)
		{
			anim = gameObject.GetComponent<Animator>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(TimeLeft <= 0)
		{
			if(anim != null)
			{
				if (!anim.GetCurrentAnimatorStateInfo(0).IsName("PodExplode") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Death"))
				{
					anim.SetTrigger("Explode");
				}
				else if(anim.GetCurrentAnimatorStateInfo(0).IsName("Death") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >=1)
				{
					if(GasSpawnerPrefab != null)
					{
						if (!CreatedSpawner)
						{
							Instantiate(GasSpawnerPrefab, transform.position, Quaternion.identity);
							CreatedSpawner = true;
						}
						Destroy(gameObject);
					}
				}
				else if(!CreatedSpawner && anim.GetCurrentAnimatorStateInfo(0).IsName("PodExplode") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.6)
				{
					Instantiate(GasSpawnerPrefab, transform.position, Quaternion.identity);
					CreatedSpawner = true;
				}
			}
		}
		else
		{
			TimeLeft -= Time.deltaTime;
		}
	}
}
