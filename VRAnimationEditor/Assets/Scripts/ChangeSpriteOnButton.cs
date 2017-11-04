using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ChangeSpriteOnButton : MonoBehaviour, IButtonAxisReciever {

	public List<Sprite> spriteList;
	private int currentSprite = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnRecieveButton (int sourceID, int buttonID, bool buttonState){
		currentSprite++;
		currentSprite = currentSprite % spriteList.Count;

		GetComponent<SpriteRenderer> ().sprite = spriteList [currentSprite];
	}

	public void OnRecieveAxis(int sourceID, int axisID, float axisValue){

	}
}
