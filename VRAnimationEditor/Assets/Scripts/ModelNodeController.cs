using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelNodeController : MonoBehaviour, IPointerReciever, IButtonAxisReciever, IGrabReciever, ITouchReciever {
    static PoseManager poseManager;

	public GameObject[] rings, arrows;
	private bool isSelected = false;
	private bool hasPointerFocus = false;
    private Transform boneNode, boneNodeParent;
    private GameObject grabOwner;
    private int grabbedSiblingIndex;
    private List<int> touchingInteractors;

	void Start(){
		SetAxisVisibility (false);
        boneNode = transform.parent;
        boneNodeParent = boneNode.parent;
        touchingInteractors = new List<int>();
        if (poseManager == null)
        {
            poseManager = GameObject.Find("Pose Manager").GetComponent<PoseManager>();
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
        if (!isSelected && touchingInteractors.Count == 0) {
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

    public void OnGrab(GameObject grabber){
        grabbedSiblingIndex = boneNode.GetSiblingIndex();
        boneNode.parent = grabber.transform;
        grabOwner = grabber;
        poseManager.OnPoseEditStart(boneNode);
    }

    public void OnRelease(GameObject grabber){
        if (grabber == grabOwner)
        {

            boneNode.parent = boneNodeParent;
            boneNode.SetSiblingIndex(grabbedSiblingIndex);
            grabOwner = null;
            poseManager.OnPoseEditFinish(boneNode);
        }
    }

    public void OnTouchEnter(int interactorId, int touchId){
        if (!touchingInteractors.Contains(interactorId))
        {
            touchingInteractors.Add(interactorId);
        }
        SetAxisVisibility(true);
    }

    public void OnTouchExit(int interactorId, int touchId){
        if (touchingInteractors.Contains(interactorId))
        {
            touchingInteractors.Remove(interactorId);
        }
        if (!hasPointerFocus && !isSelected && touchingInteractors.Count == 0)
        {
            SetAxisVisibility(false);
        }
    }

    public GameObject GetGameObject(){
        return gameObject;
    }
}
