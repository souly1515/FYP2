using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {
	public float barDisplay = 0.0f;
	public GameObject player;
	public RectTransform r;
	C_Base script;

	void Start()
	{
		if (player) {
			script = player.GetComponent<C_Base> ();

		}
		r = GetComponent<RectTransform> ();
	}


	// Update is called once per frame
	void Update () {
		if (script) {
			float scale=script.stats.Health/300.0f;

			Vector3 temp=r.localScale;
			temp.x=scale;
			r.localScale=temp;
		}
	}
}
