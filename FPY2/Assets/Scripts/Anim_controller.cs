using UnityEngine;
using System.Collections;

public class Anim_controller : MonoBehaviour {
	public string spriteName;
	SpriteRenderer sRenderer;
	public GameObject weaponGO;
	// Use this for initialization
	void Start () {
		sRenderer = weaponGO.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		Resources.LoadAll ("");
		string curSprite = sRenderer.sprite.name;
		if (weapon1.name == curSprite) {
			sRenderer.sprite=weapon1;
		}
		else if (weapon2.name == curSprite) {
			sRenderer.sprite=weapon2;
		}
		else if (weapon3.name == curSprite) {
			sRenderer.sprite=weapon3;
		}

	}
}
