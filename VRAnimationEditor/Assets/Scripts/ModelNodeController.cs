using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelNodeController : MonoBehaviour, IPointerReciever, IButtonAxisReciever, IGrabReciever {
	const int NUM_HUMAN_BONES = 55;

	public GameObject[] rings, arrows;
	public GameObject masterObject;

	private bool isSelected = false;
	private bool hasPointerFocus = false;
    private Transform boneNode, boneNodeParent, dummyNode, dummyNode2;
    private GameObject grabOwner;

	private AnimationCurveVisualizer associatedVisualizer = null;
	private List<AnimationCurveVisualizer> associatedCurveVisualizers;
	private AnimationVisualizer mainVisualizer = null;

	void Start(){
		SetAxisVisibility (false);
        boneNode = transform.parent;
        boneNodeParent = boneNode.parent;

        dummyNode = new GameObject().transform;
        dummyNode2 = new GameObject().transform;

		associatedCurveVisualizers = new List<AnimationCurveVisualizer> ();

	}

	void LateUpdate(){
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

        if (grabOwner != null) {
			//Display an interface that corresponds to this particular bone
			//string boneNodeName = boneNode.name;
			//HumanBodyBones theBone = GetBoneFromString(boneNodeName);
			/*
			Transform nodeTransform = masterObject.GetComponent<Animator> ().GetBoneTransform (theBone);

			if (nodeTransform == boneNode) {
				Debug.Log (nodeTransform);
			}*/

			//TODO: Actually save this movement to a keyframe!

			for (int i = 0; i < NUM_HUMAN_BONES; i++) {
				if(boneNode == masterObject.GetComponent<Animator>().GetBoneTransform((HumanBodyBones) i)){
					//Debug.Log((HumanBodyBones) i);
					if (((HumanBodyBones)i) == HumanBodyBones.LeftUpperArm || ((HumanBodyBones)i) == HumanBodyBones.RightUpperArm) {
						//There are three things: Down-Up, Front-Back, Twist In-Out
						//These all essentially correspond to different rotations

						//So here we'll need to take a look at the position of the dummy object and just rotate everything towards it to take care of down-up and front-back
                        //boneNode.LookAt (dummyNode, dummyNode.up);
                        dummyNode2.LookAt (dummyNode, dummyNode.up);


                        //boneNode.localEulerAngles.Set(boneNode.localEulerAngles.x, boneNode.localEulerAngles.y + 90, boneNode.localEulerAngles.z);




					} else if (((HumanBodyBones)i) == HumanBodyBones.LeftLowerArm || ((HumanBodyBones)i) == HumanBodyBones.RightLowerArm) {
						//There are two parameters: Stretch, and Twist In-Out
						Vector3 oldRotation = dummyNode2.localRotation.eulerAngles;

						//For Stretch we'll look at the position of the dummy object and rotate everything towards it, but only keep the z rotation
						//boneNode.LookAt (dummyNode.position);
                        dummyNode2.LookAt (dummyNode, dummyNode.up);

						dummyNode2.localRotation.eulerAngles.Set (oldRotation.x, oldRotation.y, dummyNode2.localRotation.eulerAngles.z);

						
					} else if (((HumanBodyBones)i) == HumanBodyBones.LeftShoulder || ((HumanBodyBones)i) == HumanBodyBones.RightShoulder) {
						//There are two parameters: Down-Up, and Front-Back

						//So here we'll need to take a look at the position of the dummy object and just rotate everything towards it to take care of down-up and front-back
						dummyNode2.LookAt (dummyNode.position);
					} else if (((HumanBodyBones)i) == HumanBodyBones.LeftHand || ((HumanBodyBones)i) == HumanBodyBones.RightHand) {
						//There are two parameters: Down-Up, and Front-Back

						//So here we'll need to take a look at the position of the dummy object and just rotate everything towards it to take care of down-up and front-back
						dummyNode2.LookAt (dummyNode.position);
					} else if (((HumanBodyBones)i) == HumanBodyBones.Spine) {
						//There are three things: Left-Right, Front-Back, Twist Left-Right

						//So here we'll need to take a look at the position of the dummy object and just rotate everything towards it to take care of left-right and front-back...?
						dummyNode2.LookAt (dummyNode.position);

						//TODO: Not sure how to deal with twist

					} else if (((HumanBodyBones)i) == HumanBodyBones.Chest) {
						//There are three things: Left-Right, Front-Back, Twist Left-Right

						//So here we'll need to take a look at the position of the dummy object and just rotate everything towards it to take care of left-right and front-back...?
						dummyNode2.LookAt (dummyNode.position);

						//TODO: Not sure how to deal with twist

					} else if (((HumanBodyBones)i) == HumanBodyBones.UpperChest) {
						//There are three things: Left-Right, Front-Back, Twist Left-Right

						//So here we'll need to take a look at the position of the dummy object and just rotate everything towards it to take care of left-right and front-back...?
						dummyNode2.LookAt (dummyNode.position);

						//TODO: Not sure how to deal with twist

					} else if (((HumanBodyBones)i) == HumanBodyBones.Neck) {
						//There are three things: Nod Down-Up, Turn Left-Right, Tilt Left-Right

						//So here we'll need to take a look at the position of the dummy object and just rotate everything towards it to take care of left-right and front-back...?
						dummyNode2.LookAt (dummyNode.position);

						//TODO: Not sure how to deal with tilt

					} else if (((HumanBodyBones)i) == HumanBodyBones.Head) {
						//There are three things: Nod Down-Up, Turn Left-Right, Tilt Left-Right

						//So here we'll need to take a look at the position of the dummy object and just rotate everything towards it to take care of left-right and front-back...?
						dummyNode2.LookAt (dummyNode.position);

						//TODO: Not sure how to deal with tilt

					} else if (((HumanBodyBones)i) == HumanBodyBones.LeftEye || ((HumanBodyBones)i) == HumanBodyBones.RightEye) {
						//There are two things: Down-Up, and In-Out

						//So here we'll need to take a look at the position of the dummy object and just rotate everything towards it to take care of left-right and front-back...?
						dummyNode2.LookAt (dummyNode.position);

					} else if (((HumanBodyBones)i) == HumanBodyBones.Jaw) {
						//There are two things: Close, and Left-Right

						//TODO

					} else if (((HumanBodyBones)i) == HumanBodyBones.LeftUpperLeg || ((HumanBodyBones)i) == HumanBodyBones.RightUpperLeg) {
						//There are three things: Front-Back, In-Out, and Twist In-Out

						//So here we'll need to take a look at the position of the dummy object and just rotate everything towards it to take care of front-back and in-out...?
						dummyNode2.LookAt (dummyNode.position);

						//TODO: Not sure how to deal with twist
					} else if (((HumanBodyBones)i) == HumanBodyBones.LeftLowerLeg || ((HumanBodyBones)i) == HumanBodyBones.RightLowerLeg) {
						//There are two parameters: Stretch, and Twist In-Out
						Vector3 oldRotation = dummyNode2.localRotation.eulerAngles;

						//For Stretch we'll look at the position of the dummy object and rotate everything towards it, but only keep the z rotation
						dummyNode2.LookAt (dummyNode.position);

						dummyNode2.localRotation.eulerAngles.Set (oldRotation.x, oldRotation.y, dummyNode2.localRotation.eulerAngles.z);

						//TODO: For twist... I'm not sure yet?
					} else if (((HumanBodyBones)i) == HumanBodyBones.LeftFoot || ((HumanBodyBones)i) == HumanBodyBones.RightFoot) {
						//There are two parameters: Up-Down, and Twist In-Out
						Vector3 oldRotation = dummyNode2.localRotation.eulerAngles;

						//For Up-Down we'll look at the position of the dummy object and rotate everything towards it, but only keep the z rotation
						dummyNode2.LookAt (dummyNode.position);

						dummyNode2.localRotation.eulerAngles.Set (oldRotation.x, oldRotation.y, dummyNode2.localRotation.eulerAngles.z);

						//TODO: For twist... I'm not sure yet?
					} else if (((HumanBodyBones)i) == HumanBodyBones.LeftToes || ((HumanBodyBones)i) == HumanBodyBones.RightToes) {
						//There is one parameter: Up-Down
						Vector3 oldRotation = dummyNode2.localRotation.eulerAngles;

						//For Up-Down we'll look at the position of the dummy object and rotate everything towards it, but only keep the z rotation
						dummyNode2.LookAt (dummyNode.position);

						dummyNode2.localRotation.eulerAngles.Set (oldRotation.x, oldRotation.y, dummyNode2.localRotation.eulerAngles.z);
					} else if (((HumanBodyBones) i) == HumanBodyBones.LeftThumbProximal || ((HumanBodyBones)i) == HumanBodyBones.RightThumbProximal || ((HumanBodyBones)i) == HumanBodyBones.LeftIndexProximal || ((HumanBodyBones)i) == HumanBodyBones.RightIndexProximal || ((HumanBodyBones)i) == HumanBodyBones.LeftMiddleProximal || ((HumanBodyBones)i) == HumanBodyBones.RightMiddleProximal || ((HumanBodyBones)i) == HumanBodyBones.LeftRingProximal || ((HumanBodyBones)i) == HumanBodyBones.RightRingProximal || ((HumanBodyBones)i) == HumanBodyBones.LeftLittleProximal || ((HumanBodyBones)i) == HumanBodyBones.RightLittleProximal) {
						//There are two parameters: .Spread, and .1 Stretched
						Vector3 oldRotation = dummyNode2.localRotation.eulerAngles;

						//For Stretched we'll look at the position of the dummy object and rotate everything towards it, but only keep the z rotation
						dummyNode2.LookAt (dummyNode.position);

						dummyNode2.localRotation.eulerAngles.Set (oldRotation.x, oldRotation.y, dummyNode2.localRotation.eulerAngles.z);

						//TODO: For spread...?
					} else if (((HumanBodyBones) i) == HumanBodyBones.LeftThumbIntermediate || ((HumanBodyBones)i) == HumanBodyBones.RightThumbIntermediate || ((HumanBodyBones)i) == HumanBodyBones.LeftIndexIntermediate || ((HumanBodyBones)i) == HumanBodyBones.RightIndexIntermediate || ((HumanBodyBones)i) == HumanBodyBones.LeftMiddleIntermediate || ((HumanBodyBones)i) == HumanBodyBones.RightMiddleIntermediate || ((HumanBodyBones)i) == HumanBodyBones.LeftRingIntermediate || ((HumanBodyBones)i) == HumanBodyBones.RightRingIntermediate || ((HumanBodyBones)i) == HumanBodyBones.LeftLittleIntermediate || ((HumanBodyBones)i) == HumanBodyBones.RightLittleIntermediate) {
						//There is one parameter: .2 Stretched
						Vector3 oldRotation = dummyNode2.localRotation.eulerAngles;

						//For Stretched we'll look at the position of the dummy object and rotate everything towards it, but only keep the z rotation
						dummyNode2.LookAt (dummyNode.position);

						dummyNode2.localRotation.eulerAngles.Set (oldRotation.x, oldRotation.y, dummyNode2.localRotation.eulerAngles.z);

					} else if (((HumanBodyBones) i) == HumanBodyBones.LeftThumbDistal || ((HumanBodyBones)i) == HumanBodyBones.RightThumbDistal || ((HumanBodyBones)i) == HumanBodyBones.LeftIndexDistal || ((HumanBodyBones)i) == HumanBodyBones.RightIndexDistal || ((HumanBodyBones)i) == HumanBodyBones.LeftMiddleDistal || ((HumanBodyBones)i) == HumanBodyBones.RightMiddleDistal || ((HumanBodyBones)i) == HumanBodyBones.LeftRingDistal || ((HumanBodyBones)i) == HumanBodyBones.RightRingDistal || ((HumanBodyBones)i) == HumanBodyBones.LeftLittleDistal || ((HumanBodyBones)i) == HumanBodyBones.RightLittleDistal) {
						//There is one parameter: .3 Stretched
						Vector3 oldRotation = dummyNode2.localRotation.eulerAngles;

						//For Stretched we'll look at the position of the dummy object and rotate everything towards it, but only keep the z rotation
						dummyNode2.LookAt (dummyNode.position);

						dummyNode2.localRotation.eulerAngles.Set (oldRotation.x, oldRotation.y, dummyNode2.localRotation.eulerAngles.z);

					} 
				}
			}

			//masterObject.GetComponent<Animator> ().GetBoneTransform (theBone);
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


	/*public void SetAssociatedVisualizer(AnimationCurveVisualizer acv){
		associatedVisualizer = acv;
	}*/

	public void AddAssociatedCurveVisualizer(AnimationCurveVisualizer acv){
		associatedCurveVisualizers.Add (acv);
	}

	public void SetMainVisualizer(AnimationVisualizer av){
		mainVisualizer = av;
	}

    public void OnGrab(GameObject grabber){

        dummyNode.position = grabber.transform.position;
        dummyNode.rotation = grabber.transform.rotation;
	
		dummyNode.parent = grabber.transform;

        dummyNode2.position = boneNode.position;
        dummyNode2.rotation = boneNode.rotation;

        if(boneNode == masterObject.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.LeftUpperArm) || boneNode == masterObject.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.RightUpperArm))
        dummyNode2.Rotate(new Vector3(0, -90 + boneNode.localEulerAngles.y, 0));

        boneNode.parent = dummyNode2;

        //boneNode.parent = grabber.transform;
        grabOwner = grabber;


    }

    public void OnRelease(GameObject grabber){
        if (grabber == grabOwner)
        {

            boneNode.parent = boneNodeParent;
            grabOwner = null;
        }
    }

	public HumanBodyBones GetBoneFromString(string s){
		switch (s) {
		case "Root":		//TODO: Maybe change
			return HumanBodyBones.Hips;
		case "Chest":
			return HumanBodyBones.Chest;
		case "Head":
			return HumanBodyBones.Head;
		case "Hips":
			return HumanBodyBones.Hips;
		case "Jaw":
			return HumanBodyBones.Jaw;
		case "LastBone":
			return HumanBodyBones.LastBone;
		case "LeftEye":
			return HumanBodyBones.LeftEye;
		case "LeftFoot":
			return HumanBodyBones.LeftFoot;
		case "LeftHand":
			return HumanBodyBones.LeftHand;
		case "LeftIndexDistal":
			return HumanBodyBones.LeftIndexDistal;
		case "LeftIndexIntermediate":
			return HumanBodyBones.LeftIndexIntermediate;
		case "LeftIndexProximal":
			return HumanBodyBones.LeftIndexProximal;
		case "LeftLittleDistal":
			return HumanBodyBones.LeftLittleDistal;
		case "LeftLittleIntermediate":
			return HumanBodyBones.LeftLittleIntermediate;
		case "LeftLittleProximal":
			return HumanBodyBones.LeftLittleProximal;
		case "LeftLowerArm":
			return HumanBodyBones.LeftLowerArm;
		case "LeftLowerLeg":
			return HumanBodyBones.LeftLowerLeg;
		case "LeftMiddleDistal":
			return HumanBodyBones.LeftMiddleDistal;
		case "LeftMiddleIntermediate":
			return HumanBodyBones.LeftMiddleIntermediate;
		case "LeftMiddleProximal":
			return HumanBodyBones.LeftMiddleProximal;
		case "LeftRingDistal":
			return HumanBodyBones.LeftRingDistal;
		case "LeftRingIntermediate":
			return HumanBodyBones.LeftRingIntermediate;
		case "LeftRingProximal":
			return HumanBodyBones.LeftRingProximal;
		case "LeftShoulder":
			return HumanBodyBones.LeftShoulder;
		case "LeftThumbDistal":
			return HumanBodyBones.LeftThumbDistal;
		case "LeftThumbIntermediate":
			return HumanBodyBones.LeftThumbIntermediate;
		case "LeftThumbProximal":
			return HumanBodyBones.LeftThumbProximal;
		case "LeftToes":
			return HumanBodyBones.LeftToes;
		case "LeftUpperArm":
			return HumanBodyBones.LeftUpperArm;
		case "LeftUpperLeg":
			return HumanBodyBones.LeftUpperLeg;
		case "Neck":
			return HumanBodyBones.Neck;
		case "RightEye":
			return HumanBodyBones.RightEye;
		case "RightFoot":
			return HumanBodyBones.RightFoot;
		case "RightHand":
			return HumanBodyBones.RightHand;
		case "RightIndexDistal":
			return HumanBodyBones.RightIndexDistal;
		case "RightIndexIntermediate":
			return HumanBodyBones.RightIndexIntermediate;
		case "RightIndexProximal":
			return HumanBodyBones.RightIndexProximal;
		case "RightLittleDistal":
			return HumanBodyBones.RightLittleDistal;
		case "RightLittleIntermediate":
			return HumanBodyBones.RightLittleIntermediate;
		case "RightLittleProximal":
			return HumanBodyBones.RightLittleProximal;
		case "RightLowerArm":
			return HumanBodyBones.RightLowerArm;
		case "RightLowerLeg":
			return HumanBodyBones.RightLowerLeg;
		case "RightMiddleDistal":
			return HumanBodyBones.RightMiddleDistal;
		case "RightMiddleIntermediate":
			return HumanBodyBones.RightMiddleIntermediate;
		case "RightMiddleProximal":
			return HumanBodyBones.RightMiddleProximal;
		case "RightRingDistal":
			return HumanBodyBones.RightRingDistal;
		case "RightRingIntermediate":
			return HumanBodyBones.RightRingIntermediate;
		case "RightRingProximal":
			return HumanBodyBones.RightRingProximal;
		case "RightShoulder":
			return HumanBodyBones.RightShoulder;
		case "RightThumbDistal":
			return HumanBodyBones.RightThumbDistal;
		case "RightThumbIntermediate":
			return HumanBodyBones.RightThumbIntermediate;
		case "RightThumbProximal":
			return HumanBodyBones.RightThumbProximal;
		case "RightToes":
			return HumanBodyBones.RightToes;
		case "RightUpperArm":
			return HumanBodyBones.RightUpperArm;
		case "RightUpperLeg":
			return HumanBodyBones.RightUpperLeg;
		case "Spine":
			return HumanBodyBones.Spine;
		case "UpperChest":
			return HumanBodyBones.UpperChest;
		default:
			break;
		}
		return HumanBodyBones.Hips;
	}

}
