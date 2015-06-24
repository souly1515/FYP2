using UnityEngine;
using System.Collections;
using System;

public class Anim_controller : MonoBehaviour {
	public string sheetName;
	SpriteRenderer sRenderer;
	public GameObject weaponGO;
	Sprite[] subsprite;
	public string lastSheetName;
	string lastSprite;
	// Use this for initialization
	void Start () {
		lastSheetName = "";
		sRenderer = weaponGO.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (lastSheetName != sheetName) {
			subsprite = Resources.LoadAll<Sprite>("Weapons/" + sheetName)as Sprite[];
		
			string curSprite = sRenderer.sprite.name;
			if(subsprite!=null)
			{
				Sprite nsprite = Array.Find (subsprite, item => item.name == curSprite);
				if (nsprite) {
					lastSheetName=sheetName;
					lastSprite=curSprite;
					sRenderer.sprite = nsprite;
				}
			}
			else{
				int p=0;
			}
		} else {
			string curSprite = sRenderer.sprite.name;
			Sprite nsprite = Array.Find (subsprite, item => item.name == curSprite);
			if (nsprite) {
				lastSprite=curSprite;
				sRenderer.sprite = nsprite;
			}
		}
	}
}
