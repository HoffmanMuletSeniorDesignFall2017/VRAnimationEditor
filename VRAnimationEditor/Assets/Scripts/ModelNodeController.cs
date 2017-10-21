using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelNodeController : MonoBehaviour, IPointerReciever, IButtonAxisReciever {
	public GameObject[] rings, arrows;
	private bool isSelected = false;
	private bool hasPointerFocus = false;

	void Start(){
		SetAxisVisibility (false);
	}

	void Update(){
		
	}

	public void SetAxisVisibility(bool isVisible){
		for (int i = 0; i < 3; i++) {
			SetVisibility (rings [i], isVisible);
			SetVisibility (arrows [i], isVisible);
		}
	}

	private void SetVisibility(GameObject obj, bool visibility){
		for (int i = 0; i < obj.transform.childCount; i++) {
			SetVisibility (obj.transform.GetChild (i).gameObject, visibility);
		}
		MeshRenderer meshRend = obj.GetComponent<MeshRenderer> ();
		if (meshRend != null) {
			meshRend.enabled = visibility;
		}
	}

	public void OnPointerEnter(int pointerID){
		hasPointerFocus = true;
		SetAxisVisibility (true);
	}

	public void OnPointerExit(int pointerID){
		hasPointerFocus = false;
		if (!isSelected) {
			SetAxisVisibility (false);
		}
	}

    public void OnRecieveButton(int sourceID, int buttonID, bool buttonState){
        if (buttonState == true)
        {
            isSelected = !isSelected;
            if (!hasPointerFocus) {
                SetAxisVisibility (isSelected);
            }
        }         		
	}

    public void OnRecieveAxis(int sourceID, int axisID, float axisValue){
        
    }
}
