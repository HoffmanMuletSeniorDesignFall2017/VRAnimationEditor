using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AnimationVisualizer : Visualizer {

	private AnimationClip currentClip;
	private GameObject currentGameObject;

	private List<AnimationCurve> animCurves;
	private List<AnimationCurveVisualizer> animCurves_Visualizers;
	private int lastSelectedAnimCurve_Visualizer = 0;

	public GameObject keyframeWorkArea;	//The keyframe work area for this visualizer.	(NOTE: just the visual component needs to be on the gameobject; we'll add the script in Start() of this class
	public GameObject keyframeObject;	//The keyframe gameobject that will be the symbol for keyframes

	public TextMesh title;
	public TextMesh values;
	//private string valuesText;


	public MocapController moCon;
	private bool wantToCapture = false;

	public void SetCurrentClipAndGameObject(AnimationClip animClip, GameObject go){
		currentClip = animClip;
		title.text = animClip.name;
		currentGameObject = go;

		if (keyframeWorkArea.GetComponent<KeyframeWorkArea> ().timelineVisualizer != null) {
			keyframeWorkArea.GetComponent<KeyframeWorkArea> ().timelineVisualizer.animator = currentGameObject.GetComponent<Animator> ();
		} else {
			Debug.LogError ("Please set up the keyframeWorkArea to have a timeline Visualizer, please!");
		}

		RefreshCurves ();
	}

	public void RefreshCurves(){
		//Animation Curve Setup
		animCurves.Clear ();

		for (int i = 0; i < animCurves_Visualizers.Count; i++) {
			animCurves_Visualizers [i].Clear ();
			GameObject.Destroy (animCurves_Visualizers [i].gameObject);
		}

		animCurves_Visualizers.Clear ();
		keyframeWorkArea.GetComponent<KeyframeWorkArea> ().RefreshBounds (0f);
		values.text = "";

		for (int i = 0; i < AnimationUtility.GetCurveBindings (currentClip).Length; i++) {

			animCurves.Add (AnimationUtility.GetEditorCurve (currentClip, AnimationUtility.GetCurveBindings (currentClip) [i]));

			//Add a visualizer for each curve
			GameObject dummyGameObject = new GameObject();
			dummyGameObject.AddComponent<AnimationCurveVisualizer> ();

			AnimationCurveVisualizer acv = dummyGameObject.GetComponent<AnimationCurveVisualizer> ();
			//AnimationCurveVisualizer acv = ScriptableObject.CreateInstance<AnimationCurveVisualizer>();//new AnimationCurveVisualizer();

			acv.curveNumber = i;
			acv.animCurve = animCurves [i];
			acv.keyframeObject = keyframeObject;
			acv.keyframeWorkArea= keyframeWorkArea.GetComponent<KeyframeWorkArea>();
			acv.parentAnimVisualizer = this;

			if (currentClip.isHumanMotion) {
				string objectAnimated = AnimationUtility.GetCurveBindings (currentClip) [i].propertyName;
				if (objectAnimated.Substring (objectAnimated.Length - 2, 1) == "."){
					//Then it is a basic bone property
					objectAnimated = objectAnimated.Substring (0, objectAnimated.Length - 3);		//Gets rid of "T.x" or whatever
					HumanBodyBones theBone = GetBoneFromString(objectAnimated);
					Transform nodeTransform = currentGameObject.GetComponent<Animator> ().GetBoneTransform (theBone);

					acv.associatedNodeVisualizer = nodeTransform.GetChild (nodeTransform.childCount - 1).gameObject;		//Assumes Node marker will always be the last child
				}
			}

			acv.Refresh ();


			animCurves_Visualizers.Add (acv);
		}

		for (int i = 0; i < animCurves.Count; i++) {
			if(i == 0)
				values.text = AnimationUtility.GetCurveBindings (currentClip) [i].propertyName + "\n";
			else
				values.text += AnimationUtility.GetCurveBindings (currentClip) [i].propertyName + "\n";
			//Then would do stuff to actually draw keyframes, etc.
		}

	}

	public void TogglePlayAnimation(){
		if (currentGameObject.GetComponent<Animator> ().speed > 0 || currentGameObject.GetComponent<Animator> ().speed < 0) {
			currentGameObject.GetComponent<Animator> ().speed = 0;
		} else {
			currentGameObject.GetComponent<Animator> ().speed = 1;
		}
	}

	public void PauseAnimation(){
		currentGameObject.GetComponent<Animator> ().speed = 0;
	}


	// Use this for initialization
	void Start () {
		animCurves = new List<AnimationCurve> ();
		animCurves_Visualizers = new List<AnimationCurveVisualizer> ();

		if (keyframeWorkArea.GetComponent<KeyframeWorkArea> () == null) {
			keyframeWorkArea.AddComponent<KeyframeWorkArea> ();

		}

		if (moCon == null) {
			gameObject.AddComponent<MocapController> ();
			moCon = gameObject.GetComponent<MocapController> ();
		}

		this.enabled = false;		//We don't need update so we disable this
	}
	
	// Update is called once per frame
	void Update () {
		//TODO: Get rid of all of this; update is not needed
		//TODO: Get rid of this HACK

		for (int i = 0; i < animCurves.Cou	nt; i++) {
			if(i == 0)
				values.text = AnimationUtility.GetCurveBindings (currentClip) [i].propertyName + "\n";
			else
				values.text += AnimationUtility.GetCurveBindings (currentClip) [i].propertyName + "\n";
			//Then would do stuff to actually draw keyframes, etc.
		}

		//-----END HACK
		for (int i = 0; i < animCurves.Count; i++) {
			if (animCurves_Visualizers [i].needsToRefresh) {



				//We have to keep track of the current time because SetCurve resets it :(
				float currentTime = keyframeWorkArea.GetComponent<KeyframeWorkArea>().timelineVisualizer.GetAnimatorTime();

				//Debug.Log (currentTime);

				StartCoroutine (UpdateAnimationCurveAndResume (AnimationUtility.GetCurveBindings (currentClip) [i].path, AnimationUtility.GetCurveBindings (currentClip) [i].type, AnimationUtility.GetCurveBindings (currentClip) [i].propertyName, animCurves [i], currentTime));
				//currentClip.SetCurve (AnimationUtility.GetCurveBindings (currentClip) [i].path, AnimationUtility.GetCurveBindings (currentClip) [i].type, AnimationUtility.GetCurveBindings (currentClip) [i].propertyName, animCurves [i]);

				//keyframeWorkArea.GetComponent<KeyframeWorkArea> ().timelineVisualizer.ChangeTime (currentTime);

				//keyframeWorkArea.GetComponent<KeyframeWorkArea>().timelineVisualizer.animator.Play (keyframeWorkArea.GetComponent<KeyframeWorkArea>().timelineVisualizer.animator.GetCurrentAnimatorStateInfo (0).shortNameHash, 0, currentTime);
				//RefreshCurves();
				animCurves_Visualizers[i].needsToRefresh = false;
			}
		}

		//Have the last updated curve thing here
		for (int i = 0; i < animCurves.Count; i++) {
			if (animCurves_Visualizers [i].selected == true) {
				lastSelectedAnimCurve_Visualizer = i;
				break;
			}
		}
	}

	public void RefreshAnimationCurve(int i){

	}

	IEnumerator UpdateAnimationCurveAndResume(string path, System.Type type, string propertyName, AnimationCurve animCurve, float resumeTime){

		/*waitFrame = (++waitFrame) % NUM_WAIT_FRAMES;
		if (waitFrame != 0)
			yield return null;
		*/
		AnimationClip newClip = new AnimationClip ();
		newClip.name = "Test";

		//Perform a deep copy
		for (int i = 0; i < AnimationUtility.GetCurveBindings (currentClip).Length; i++) {
			newClip.SetCurve (AnimationUtility.GetCurveBindings (currentClip) [i].path, AnimationUtility.GetCurveBindings (currentClip) [i].type, AnimationUtility.GetCurveBindings (currentClip) [i].propertyName, AnimationUtility.GetEditorCurve (currentClip, AnimationUtility.GetCurveBindings (currentClip) [i]));
		}

		newClip.SetCurve (path, type, propertyName, animCurve);
		keyframeWorkArea.GetComponent<KeyframeWorkArea> ().timelineVisualizer.ChangeClip (newClip);
		//currentClip.SetCurve (path, type, propertyName, animCurve);
		//yield return new WaitForEndOfFrame ();
		yield return null;

		currentClip = newClip;

		//keyframeWorkArea.GetComponent<KeyframeWorkArea> ().timelineVisualizer.ChangeTime (resumeTime);
		yield return null;
	}

	public void ToggleMotionCapture(){
		wantToCapture = !wantToCapture;
		if (wantToCapture)
			StartMotionCapture ();
		else
			StopMotionCapture ();
	}

	public void StartMotionCapture(){
		for (int i = 0; i < animCurves_Visualizers.Count; i++) {
			//TODO: Make VR compatible

			//TODO: Support more than one selected curve parameter

			if (animCurves_Visualizers [i].selected) {
				//Debug.Log ("Got that we should do something... checking for associated node thing");
				if(animCurves_Visualizers[i].associatedNodeVisualizer != null){
					//Debug.Log ("About to call moCon.start capture");
					moCon.StartCapturing(animCurves_Visualizers[i].associatedNodeVisualizer, Input.mousePosition, animCurves_Visualizers[i], keyframeWorkArea.GetComponent<KeyframeWorkArea>().timelineVisualizer);
					break;
				}
			}
		}
	}

	public void StopMotionCapture(){
		moCon.StopCapturing ();
	}


	public AnimationCurveVisualizer GetAnimCurveVisualizer(int index){
		return animCurves_Visualizers [index];
	}

	public AnimationCurveVisualizer GetLastAnimCurveVisualizer(){
		return animCurves_Visualizers [lastSelectedAnimCurve_Visualizer];
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
