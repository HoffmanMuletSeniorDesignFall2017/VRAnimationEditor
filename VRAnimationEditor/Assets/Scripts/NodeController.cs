using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeController : MonoBehaviour, IPointerReciever {
	public GameObject[] rings, arrows;
	private bool isSelected = false;
	private bool hasPointerFocus = false;

	private AnimationCurveVisualizer associatedVisualizer = null;

	void Start(){
		SetAxisVisibility (false);
	}

	void Update(){
		//TODO: Move this to a single instance of some controller class to save time
		if (isSelected && Input.GetKeyDown (KeyCode.N)) {
			if (associatedVisualizer != null) {
				associatedVisualizer.RecordValuesOfAssociatedNode ();
			}
		}
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

	public void OnButtonDown(int pointerID, int buttonID){
		isSelected = !isSelected;
		if (!hasPointerFocus) {
			SetAxisVisibility (isSelected);
		}
	}

	public void OnButtonUp(int pointerID, int buttonID){
		
	}
}
