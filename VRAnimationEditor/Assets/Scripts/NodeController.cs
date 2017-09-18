using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeController : MonoBehaviour, IPointerReciever {
	public GameObject[] rings, arrows;

	void Start(){
		SetAxisVisibility (false);
	}

	void Update(){
		
	}

	private void SetAxisVisibility(bool isVisible){
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
		SetAxisVisibility (true);
	}

	public void OnPointerExit(int pointerID){
		SetAxisVisibility (false);
	}

	public void OnButtonDown(int pointerID, int buttonID){
	}

	public void OnButtonUp(int pointerID, int buttonID){
		
	}
}
