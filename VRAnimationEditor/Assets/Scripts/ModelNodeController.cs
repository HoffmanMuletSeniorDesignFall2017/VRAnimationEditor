using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelNodeController : MonoBehaviour, IPointerReciever, IButtonAxisReciever {
	public GameObject[] rings, arrows;
	private bool isSelected = false;
	private bool hasPointerFocus = false;

	private AnimationCurveVisualizer associatedVisualizer = null;
	private AnimationVisualizer mainVisualizer = null;

	void Start(){
		SetAxisVisibility (false);
	}

	void Update(){
		//TODO: Move this to a single instance of some controller class to save time
		if (isSelected && Input.GetKeyDown (KeyCode.N)) {
			if (associatedVisualizer != null) {
				associatedVisualizer.RecordValuesOfAssociatedNode ();
			} else {
				return;
				//TODO: Make associated animation curves and stuffs
				AnimationClip ac = mainVisualizer.GetCurrentClip();

				float currentTime = mainVisualizer.keyframeWorkArea.GetComponent<KeyframeWorkArea> ().timelineVisualizer.GetComponent<TimelineVisualizer> ().GetAnimatorTime ();

				AnimationCurve newCurve = new AnimationCurve();
				Keyframe key1 = new Keyframe();
				Keyframe key2 = new Keyframe();
				Keyframe key3 = new Keyframe();

				key1.time = 0f;
				key1.value = transform.position.x;

				key2.time = currentTime*2f;		//
				key2.value = transform.position.x;

				key3.time = currentTime;
				key3.value = transform.position.x;

				newCurve.AddKey (key1);
				newCurve.AddKey (key2);

				//animClip.SetCurve("", typeof(Transform), "localPosition.y", testCurve);
				ac.SetCurve("", typeof(Transform), "localPosition.y", newCurve);
				//animClip.SetCurve("", typeof(Transform), "localScale.y", testCurve);

				mainVisualizer.RefreshCurves ();
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

	public void SetAssociatedVisualizer(AnimationCurveVisualizer acv){
		associatedVisualizer = acv;
	}

	public void SetMainVisualizer(AnimationVisualizer av){
		mainVisualizer = av;
	}
}
