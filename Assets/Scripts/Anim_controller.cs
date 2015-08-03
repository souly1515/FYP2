using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;


public class Anim_controller : MonoBehaviour {
	public string sheetName;
	SpriteRenderer sRenderer;
	public GameObject weaponGO;
	Sprite[] subsprite;
	Sprite[] skillDisplaySprite;
	public string lastSheetName;
	Image[] skill;
	public bool Fire;
	// Use this for initialization
	void Start () {
		Fire = false;
		skillDisplaySprite = Resources.LoadAll<Sprite> ("Skills/icons_skills");
		lastSheetName = "";
		sRenderer = weaponGO.GetComponent<SpriteRenderer>();
		GameObject t=GameObject.FindGameObjectWithTag ("SkillDisplay");
		skill = t.GetComponentsInChildren<Image> ();
		string panelName;
		for(int i=1;i<5;i++)
		{
			if(Fire)
			{
				panelName="Fire_"+i.ToString();
			}
			else
				panelName="Earth_"+i.ToString();
			Sprite nsprite = Array.Find (skillDisplaySprite, item => item.name == panelName);
			skill[i-1].sprite=nsprite;
		}
		subsprite = Resources.LoadAll<Sprite>("Weapons/" + sheetName)as Sprite[];

	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (lastSheetName != sheetName) {
			string panelName;
			for(int i=1;i<5;i++)
			{
				if(Fire)
				{
					panelName="Fire_"+i.ToString();
				}
				else
					panelName="Earth_"+i.ToString();
				Sprite nsprite = Array.Find (skillDisplaySprite, item => item.name == panelName);
				skill[i-1].sprite=nsprite;
			}
			subsprite = Resources.LoadAll<Sprite>("Weapons/" + sheetName)as Sprite[];
		
			string curSprite = sRenderer.sprite.name;
			if(subsprite!=null)
			{
				Sprite nsprite = Array.Find (subsprite, item => item.name == curSprite);
				if (nsprite) {
					lastSheetName=sheetName;
					sRenderer.sprite = nsprite;
				}
			}
		} else {
			string curSprite = sRenderer.sprite.name;
			Sprite nsprite = Array.Find (subsprite, item => item.name == curSprite);
			if (nsprite) {
				sRenderer.sprite = nsprite;
			}
		}
	}
}
