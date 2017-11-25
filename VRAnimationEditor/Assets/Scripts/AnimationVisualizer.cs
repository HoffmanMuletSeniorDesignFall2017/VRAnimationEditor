using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

public class AnimationVisualizer : Visualizer {

	private AnimationClip currentClip, newClip, bufferClip;
    private AnimationClip[] bufferClips = new AnimationClip[64];
	private GameObject currentGameObject;

	//private List<AnimationCurve> animCurves;
	private List<AnimationCurveVisualizer> animCurves_Visualizers;
	private int lastSelectedAnimCurve_Visualizer = 0;

	public GameObject keyframeWorkArea;	//The keyframe work area for this visualizer.	(NOTE: just the visual component needs to be on the gameobject; we'll add the script in Start() of this class
	public GameObject keyframeObject;	//The keyframe gameobject that will be the symbol for keyframes

	public TextMesh title;
	public TextMesh values;
	//private string valuesText;


	public MocapController moCon;
	private bool wantToCapture = false;
    private int clipSwitch = 0;

	private static readonly string[] humanoidProperties = {
		"Spine Front-Back", "Spine Left-Right", "Spine Twist Left-Right", "Chest Front-Back", "Chest Left-Right", "Chest Twist Left-Right",
		"UpperChest Front-Back", "UpperChest Left-Right", "UpperChest Twist Left-Right", "Neck Nod Down-Up", "Neck Tilt Left-Right", "Neck Turn Left-Right",
		"Head Nod Down-Up", "Head Tilt Left-Right", "Head Turn Left-Right", "Left Eye Down-Up", "Left Eye In-Out", "Right Eye Down-Up", "Right Eye In-Out",
		"Jaw Close", "Jaw Left-Right", "Left Upper Leg Front-Back", "Left Upper Leg In-Out", "Left Upper Leg Twist In-Out", "Left Lower Leg Stretch", 
		"Left Lower Leg Twist In-Out", "Left Foot Up-Down", "Left Foot Twist In-Out", "Left Toes Up-Down", 
		"Right Upper Leg Front-Back", "Right Upper Leg In-Out", "Right Upper Leg Twist In-Out", "Right Lower Leg Stretch", 
		"Right Lower Leg Twist In-Out", "Right Foot Up-Down", "Right Foot Twist In-Out", "Right Toes Up-Down", 
		"Left Shoulder Down-Up", "Left Shoulder Front-Back", "Left Arm Down-Up", "Left Arm Front-Back", "Left Arm Twist In-Out", "Left Forearm Stretch",
		"Left Forearm Twist In-Out", "Left Hand Down-Up", "Left Hand In-Out", 
		"Right Shoulder Down-Up", "Right Shoulder Front-Back", "Right Arm Down-Up", "Right Arm Front-Back", "Right Arm Twist In-Out", "Right Forearm Stretch",
		"Right Forearm Twist In-Out", "Right Hand Down-Up", "Right Hand In-Out", 
		"LeftHand.Thumb.1 Stretched", "LeftHand.Thumb.Spread", "LeftHand.Thumb.2 Stretched", "LeftHand.Thumb.3 Stretched", 
		"LeftHand.Index.1 Stretched", "LeftHand.Index.Spread", "LeftHand.Index.2 Stretched", "LeftHand.Index.3 Stretched", 
		"LeftHand.Middle.1 Stretched", "LeftHand.Middle.Spread", "LeftHand.Middle.2 Stretched", "LeftHand.Middle.3 Stretched", 
		"LeftHand.Ring.1 Stretched", "LeftHand.Ring.Spread", "LeftHand.Ring.2 Stretched", "LeftHand.Ring.3 Stretched", 
		"LeftHand.Little.1 Stretched", "LeftHand.Little.Spread", "LeftHand.Little.2 Stretched", "LeftHand.Little.3 Stretched", 
		"RightHand.Thumb.1 Stretched", "RightHand.Thumb.Spread", "RightHand.Thumb.2 Stretched", "RightHand.Thumb.3 Stretched", 
		"RightHand.Index.1 Stretched", "RightHand.Index.Spread", "RightHand.Index.2 Stretched", "RightHand.Index.3 Stretched", 
		"RightHand.Middle.1 Stretched", "RightHand.Middle.Spread", "RightHand.Middle.2 Stretched", "RightHand.Middle.3 Stretched", 
		"RightHand.Ring.1 Stretched", "RightHand.Ring.Spread", "RightHand.Ring.2 Stretched", "RightHand.Ring.3 Stretched", 
		"RightHand.Little.1 Stretched", "RightHand.Little.Spread", "RightHand.Little.2 Stretched", "RightHand.Little.3 Stretched", 

	};

	public void SetCurrentClipAndGameObject(AnimationClip animClip, GameObject go){
		currentClip = animClip;
		title.text = animClip.name;
		currentGameObject = go;

        newClip = new AnimationClip();
        newClip.name = currentClip.name;

        //Perform a deep copy
        for (int i = 0; i < AnimationUtility.GetCurveBindings(currentClip).Length; i++)
        {
            newClip.SetCurve(AnimationUtility.GetCurveBindings(currentClip)[i].path, AnimationUtility.GetCurveBindings(currentClip)[i].type, AnimationUtility.GetCurveBindings(currentClip)[i].propertyName, AnimationUtility.GetEditorCurve(currentClip, AnimationUtility.GetCurveBindings(currentClip)[i]));
        }

        for(int j = 0; j < bufferClips.Length; j++)
        {
            bufferClips[j] = new AnimationClip();
            bufferClips[j].name = currentClip.name;

            //Perform a deep copy
            for (int i = 0; i < AnimationUtility.GetCurveBindings(currentClip).Length; i++)
            {
                bufferClips[j].SetCurve(AnimationUtility.GetCurveBindings(currentClip)[i].path, AnimationUtility.GetCurveBindings(currentClip)[i].type, AnimationUtility.GetCurveBindings(currentClip)[i].propertyName, AnimationUtility.GetEditorCurve(currentClip, AnimationUtility.GetCurveBindings(currentClip)[i]));
            }
        }

        //currentClip = bufferClips[bufferClips.Length - 1];

        if (keyframeWorkArea.GetComponent<KeyframeWorkArea> ().timelineVisualizer != null) {
			keyframeWorkArea.GetComponent<KeyframeWorkArea> ().timelineVisualizer.animator = currentGameObject.GetComponent<Animator> ();
		} else {
			Debug.LogError ("Please set up the keyframeWorkArea to have a timeline Visualizer, please!");
		}

		RefreshCurves ();
	}

	public AnimationClip GetCurrentClip(){
        return bufferClips[clipSwitch];//currentClip;
	}

	public void RefreshCurves(){
		//Animation Curve Setup
		//animCurves.Clear ();

		for (int i = 0; i < animCurves_Visualizers.Count; i++) {
			animCurves_Visualizers [i].Clear ();
			GameObject.Destroy (animCurves_Visualizers [i].gameObject);
		}

		animCurves_Visualizers.Clear ();
		keyframeWorkArea.GetComponent<KeyframeWorkArea> ().RefreshBounds (0f);
		values.text = "";

		for (int i = 0; i < AnimationUtility.GetCurveBindings (bufferClips[clipSwitch]).Length; i++) {      //currentClip

			//animCurves.Add (AnimationUtility.GetEditorCurve (currentClip, AnimationUtility.GetCurveBindings (currentClip) [i]));

			//Add a visualizer for each curve
			GameObject dummyGameObject = new GameObject();
			dummyGameObject.AddComponent<AnimationCurveVisualizer> ();

			AnimationCurveVisualizer acv = dummyGameObject.GetComponent<AnimationCurveVisualizer> ();
			//AnimationCurveVisualizer acv = ScriptableObject.CreateInstance<AnimationCurveVisualizer>();//new AnimationCurveVisualizer();

			acv.curveNumber = i;
            acv.animCurve = AnimationUtility.GetEditorCurve(bufferClips[clipSwitch], AnimationUtility.GetCurveBindings(bufferClips[clipSwitch])[i]); //animCurves [i];

            acv.keyframeObject = keyframeObject;
			acv.keyframeWorkArea= keyframeWorkArea.GetComponent<KeyframeWorkArea>();
			acv.parentAnimVisualizer = this;

            if (bufferClips[clipSwitch].isHumanMotion)
            {
                string objectAnimated = AnimationUtility.GetCurveBindings(bufferClips[clipSwitch])[i].propertyName;

                acv.isHumanoid = true;

                //Debug.Log (AnimationUtility.GetCurveBindings (currentClip) [i].type);

                if (objectAnimated.Substring(objectAnimated.Length - 2, 1) == ".")
                {
                    //Then it is a basic bone property
                    objectAnimated = objectAnimated.Substring(0, objectAnimated.Length - 3);        //Gets rid of "T.x" or whatever
                    HumanBodyBones theBone = GetBoneFromString(objectAnimated);
                    Transform nodeTransform = currentGameObject.GetComponent<Animator>().GetBoneTransform(theBone);

                    if (nodeTransform.GetChild(nodeTransform.childCount - 1) != null)
                    {

                        acv.associatedNodeVisualizer = nodeTransform.GetChild(nodeTransform.childCount - 1).gameObject;     //Assumes Node marker will always be the last child
                                                                                                                            //acv.associatedNodeVisualizer.GetComponent<ModelNodeController>().SetAssociatedVisualizer(acv);			//Link the acv and the node marker both ways (so they can both talk to each other)
                        acv.associatedNodeVisualizer.GetComponent<ModelNodeController>().AddAssociatedCurveVisualizer(acv);

                        acv.associatedNodeVisualizer.GetComponent<ModelNodeController>().SetMainVisualizer(this);               //Makes it so the node visualizer can talk to this guy too
                    }
                }
                else
                {

                    currentClip.SetCurve(AnimationUtility.GetCurveBindings(currentClip)[i].path, AnimationUtility.GetCurveBindings(currentClip)[i].type, AnimationUtility.GetCurveBindings(currentClip)[i].propertyName, null);

                    continue;

                    //It is a muscle property, so we specify what type it is... laboriously.
                    if (objectAnimated == humanoidProperties[0])
                    {
                        //Spine Front-Back
                        acv.curveType = AnimationCurveVisualizer.ACVType.RotX;
                    }
                    else if (objectAnimated == humanoidProperties[1])
                    {
                        //Spine Left-Right
                        acv.curveType = AnimationCurveVisualizer.ACVType.Other;
                    }
                    else if (objectAnimated == humanoidProperties[2])
                    {
                        //Spine Twist Left-Right
                        acv.curveType = AnimationCurveVisualizer.ACVType.Other;
                    }
                    else
                    {
                        acv.curveType = AnimationCurveVisualizer.ACVType.Other;
                    }


                    //It is a muscle property, so we set the corresponding node.

                    HumanBodyBones theBone = GetBoneFromString("Hips");     //Default...

                    if (objectAnimated == humanoidProperties[0] || objectAnimated == humanoidProperties[1] || objectAnimated == humanoidProperties[2])
                    {

                        theBone = GetBoneFromString("Spine");
                    }
                    else if (objectAnimated == humanoidProperties[3] || objectAnimated == humanoidProperties[4] || objectAnimated == humanoidProperties[5])
                    {
                        theBone = GetBoneFromString("Chest");
                    }
                    else if (objectAnimated == humanoidProperties[6] || objectAnimated == humanoidProperties[7] || objectAnimated == humanoidProperties[8])
                    {
                        theBone = GetBoneFromString("UpperChest");
                    }
                    else if (objectAnimated == humanoidProperties[9] || objectAnimated == humanoidProperties[10] || objectAnimated == humanoidProperties[11])
                    {
                        theBone = GetBoneFromString("Neck");
                    }
                    else if (objectAnimated == humanoidProperties[12] || objectAnimated == humanoidProperties[13] || objectAnimated == humanoidProperties[14])
                    {
                        theBone = GetBoneFromString("Head");
                    }
                    else if (objectAnimated == humanoidProperties[15] || objectAnimated == humanoidProperties[16])
                    {
                        theBone = GetBoneFromString("LeftEye");
                    }
                    else if (objectAnimated == humanoidProperties[17] || objectAnimated == humanoidProperties[18])
                    {
                        theBone = GetBoneFromString("RightEye");
                    }
                    else if (objectAnimated == humanoidProperties[19] || objectAnimated == humanoidProperties[20])
                    {
                        theBone = GetBoneFromString("Jaw");
                    }
                    else if (objectAnimated == humanoidProperties[21] || objectAnimated == humanoidProperties[22] || objectAnimated == humanoidProperties[23])
                    {
                        theBone = GetBoneFromString("LeftUpperLeg");
                    }
                    else if (objectAnimated == humanoidProperties[24] || objectAnimated == humanoidProperties[25])
                    {
                        theBone = GetBoneFromString("LeftLowerLeg");
                    }
                    else if (objectAnimated == humanoidProperties[26] || objectAnimated == humanoidProperties[27])
                    {
                        theBone = GetBoneFromString("LeftFoot");
                    }
                    else if (objectAnimated == humanoidProperties[28])
                    {
                        theBone = GetBoneFromString("LeftToes");
                    }
                    else if (objectAnimated == humanoidProperties[29] || objectAnimated == humanoidProperties[30] || objectAnimated == humanoidProperties[31])
                    {
                        theBone = GetBoneFromString("RightUpperLeg");
                    }
                    else if (objectAnimated == humanoidProperties[32] || objectAnimated == humanoidProperties[33])
                    {
                        theBone = GetBoneFromString("RightLowerLeg");
                    }
                    else if (objectAnimated == humanoidProperties[34] || objectAnimated == humanoidProperties[35])
                    {
                        theBone = GetBoneFromString("RightFoot");
                    }
                    else if (objectAnimated == humanoidProperties[36])
                    {
                        theBone = GetBoneFromString("RightToes");
                    }
                    else if (objectAnimated == humanoidProperties[37] || objectAnimated == humanoidProperties[38])
                    {
                        theBone = GetBoneFromString("LeftShoulder");
                    }
                    else if (objectAnimated == humanoidProperties[39] || objectAnimated == humanoidProperties[40] || objectAnimated == humanoidProperties[41])
                    {
                        theBone = GetBoneFromString("LeftUpperArm");
                    }
                    else if (objectAnimated == humanoidProperties[42] || objectAnimated == humanoidProperties[43])
                    {
                        theBone = GetBoneFromString("LeftLowerArm");
                    }
                    else if (objectAnimated == humanoidProperties[44] || objectAnimated == humanoidProperties[45])
                    {
                        theBone = GetBoneFromString("LeftHand");
                    }
                    else if (objectAnimated == humanoidProperties[46] || objectAnimated == humanoidProperties[47])
                    {
                        theBone = GetBoneFromString("RightShoulder");
                    }
                    else if (objectAnimated == humanoidProperties[48] || objectAnimated == humanoidProperties[49] || objectAnimated == humanoidProperties[50])
                    {
                        theBone = GetBoneFromString("RightUpperArm");
                    }
                    else if (objectAnimated == humanoidProperties[51] || objectAnimated == humanoidProperties[52])
                    {
                        theBone = GetBoneFromString("RightLowerArm");
                    }
                    else if (objectAnimated == humanoidProperties[53] || objectAnimated == humanoidProperties[54])
                    {
                        theBone = GetBoneFromString("RightHand");
                    }
                    else if (objectAnimated == humanoidProperties[55] || objectAnimated == humanoidProperties[56])
                    {
                        theBone = GetBoneFromString("LeftThumbProximal");
                    }
                    else if (objectAnimated == humanoidProperties[57])
                    {
                        theBone = GetBoneFromString("LeftThumbIntermediate");
                    }
                    else if (objectAnimated == humanoidProperties[58])
                    {
                        theBone = GetBoneFromString("LeftThumbDistal");
                    }
                    else if (objectAnimated == humanoidProperties[59] || objectAnimated == humanoidProperties[60])
                    {
                        theBone = GetBoneFromString("LeftIndexProximal");
                    }
                    else if (objectAnimated == humanoidProperties[61])
                    {
                        theBone = GetBoneFromString("LeftIndexIntermediate");
                    }
                    else if (objectAnimated == humanoidProperties[62])
                    {
                        theBone = GetBoneFromString("LeftIndexDistal");
                    }
                    else if (objectAnimated == humanoidProperties[63] || objectAnimated == humanoidProperties[64])
                    {
                        theBone = GetBoneFromString("LeftMiddleProximal");
                    }
                    else if (objectAnimated == humanoidProperties[65])
                    {
                        theBone = GetBoneFromString("LeftMiddleIntermediate");
                    }
                    else if (objectAnimated == humanoidProperties[66])
                    {
                        theBone = GetBoneFromString("LeftMiddleDistal");
                    }
                    else if (objectAnimated == humanoidProperties[67] || objectAnimated == humanoidProperties[68])
                    {
                        theBone = GetBoneFromString("LeftRingProximal");
                    }
                    else if (objectAnimated == humanoidProperties[69])
                    {
                        theBone = GetBoneFromString("LeftRingIntermediate");
                    }
                    else if (objectAnimated == humanoidProperties[70])
                    {
                        theBone = GetBoneFromString("LeftRingDistal");
                    }
                    else if (objectAnimated == humanoidProperties[71] || objectAnimated == humanoidProperties[72])
                    {
                        theBone = GetBoneFromString("LeftLittleProximal");
                    }
                    else if (objectAnimated == humanoidProperties[73])
                    {
                        theBone = GetBoneFromString("LeftLittleIntermediate");
                    }
                    else if (objectAnimated == humanoidProperties[74])
                    {
                        theBone = GetBoneFromString("LeftLittleDistal");
                    }
                    else if (objectAnimated == humanoidProperties[75] || objectAnimated == humanoidProperties[76])
                    {
                        theBone = GetBoneFromString("RightThumbProximal");
                    }
                    else if (objectAnimated == humanoidProperties[77])
                    {
                        theBone = GetBoneFromString("RightThumbIntermediate");
                    }
                    else if (objectAnimated == humanoidProperties[78])
                    {
                        theBone = GetBoneFromString("RightThumbDistal");
                    }
                    else if (objectAnimated == humanoidProperties[79] || objectAnimated == humanoidProperties[80])
                    {
                        theBone = GetBoneFromString("RightIndexProximal");
                    }
                    else if (objectAnimated == humanoidProperties[81])
                    {
                        theBone = GetBoneFromString("RightIndexIntermediate");
                    }
                    else if (objectAnimated == humanoidProperties[82])
                    {
                        theBone = GetBoneFromString("RightIndexDistal");
                    }
                    else if (objectAnimated == humanoidProperties[83] || objectAnimated == humanoidProperties[84])
                    {
                        theBone = GetBoneFromString("RightMiddleProximal");
                    }
                    else if (objectAnimated == humanoidProperties[85])
                    {
                        theBone = GetBoneFromString("RightMiddleIntermediate");
                    }
                    else if (objectAnimated == humanoidProperties[86])
                    {
                        theBone = GetBoneFromString("RightMiddleDistal");
                    }
                    else if (objectAnimated == humanoidProperties[87] || objectAnimated == humanoidProperties[88])
                    {
                        theBone = GetBoneFromString("RightRingProximal");
                    }
                    else if (objectAnimated == humanoidProperties[89])
                    {
                        theBone = GetBoneFromString("RightRingIntermediate");
                    }
                    else if (objectAnimated == humanoidProperties[90])
                    {
                        theBone = GetBoneFromString("RightRingDistal");
                    }
                    else if (objectAnimated == humanoidProperties[91] || objectAnimated == humanoidProperties[92])
                    {
                        theBone = GetBoneFromString("RightLittleProximal");
                    }
                    else if (objectAnimated == humanoidProperties[93])
                    {
                        theBone = GetBoneFromString("RightLittleIntermediate");
                    }
                    else if (objectAnimated == humanoidProperties[94])
                    {
                        theBone = GetBoneFromString("RightLittleDistal");
                    }

                    Transform nodeTransform = currentGameObject.GetComponent<Animator>().GetBoneTransform(theBone);

                    if (nodeTransform != null)
                    {   //Could be null if the avatar doesn't support this bone
                        if (nodeTransform.GetChild(nodeTransform.childCount - 1) != null)
                        {   //This is just in case

                            acv.associatedNodeVisualizer = nodeTransform.GetChild(nodeTransform.childCount - 1).gameObject;     //Assumes Node marker will always be the last child
                                                                                                                                //acv.associatedNodeVisualizer.GetComponent<ModelNodeController>().SetAssociatedVisualizer(acv);			//Link the acv and the node marker both ways (so they can both talk to each other)
                            acv.associatedNodeVisualizer.GetComponent<ModelNodeController>().AddAssociatedCurveVisualizer(acv);

                            acv.associatedNodeVisualizer.GetComponent<ModelNodeController>().SetMainVisualizer(this);               //Makes it so the node visualizer can talk to this guy too
                        }
                    }
                }
            }

            else
            {   //Not a humanoid animation

                Transform nodeTransform;
				if(AnimationUtility.GetCurveBindings (bufferClips[clipSwitch]) [i].path != ""){
					nodeTransform = currentGameObject.transform.Find(AnimationUtility.GetCurveBindings (bufferClips[clipSwitch]) [i].path);
				}
				else {
					nodeTransform = currentGameObject.transform;
				}

                if(nodeTransform != null) { 
				    acv.associatedNodeVisualizer = nodeTransform.GetChild (nodeTransform.childCount - 1).gameObject;        //Assumes Node marker will always be the last child
                                                                                                                            //acv.associatedNodeVisualizer.GetComponent<ModelNodeController>().SetAssociatedVisualizer(acv);			//Link the acv and the node marker both ways (so they can both talk to each other)
                
                    acv.associatedNodeVisualizer.GetComponent<ModelNodeController>().AddAssociatedCurveVisualizer(acv);


                    acv.associatedNodeVisualizer.GetComponent<ModelNodeController>().SetMainVisualizer(this);               //Makes it so the node visualizer can talk to this guy too
                }

                string propertyName = AnimationUtility.GetCurveBindings(bufferClips[clipSwitch])[i].propertyName;

                if (propertyName == "m_LocalPosition.x")
                    acv.curveType = AnimationCurveVisualizer.ACVType.PosX;
                else if (propertyName == "m_LocalPosition.y")
                    acv.curveType = AnimationCurveVisualizer.ACVType.PosY;
                else if (propertyName == "m_LocalPosition.z")
                    acv.curveType = AnimationCurveVisualizer.ACVType.PosZ;
                else if (propertyName == "m_LocalRotation.w")
                    acv.curveType = AnimationCurveVisualizer.ACVType.RotW;
                else if (propertyName == "m_LocalRotation.x")
                    acv.curveType = AnimationCurveVisualizer.ACVType.RotX;
                else if (propertyName == "m_LocalRotation.y")
                    acv.curveType = AnimationCurveVisualizer.ACVType.RotY;
                else if (propertyName == "m_LocalRotation.z")
                    acv.curveType = AnimationCurveVisualizer.ACVType.RotZ;
                else
                {
                    acv.curveType = AnimationCurveVisualizer.ACVType.Other;
                }


            }

            acv.Refresh ();


			animCurves_Visualizers.Add (acv);
		}

		for (int i = 0; i < animCurves_Visualizers.Count; i++) {
			if(i == 0)
				values.text = AnimationUtility.GetCurveBindings (bufferClips[clipSwitch]) [i].propertyName + "\n";
			else
				values.text += AnimationUtility.GetCurveBindings (bufferClips[clipSwitch]) [i].propertyName + "\n";
			//Then would do stuff to actually draw keyframes, etc.
		}

	}

    public void RefreshNewCurves()
    {
        //Animation Curve Setup
        int oldCount = animCurves_Visualizers.Count;

        EditorCurveBinding[] thingers =  AnimationUtility.GetCurveBindings(currentClip);

        for (int i = oldCount; i < AnimationUtility.GetCurveBindings(currentClip).Length; i++)
        {

            //animCurves.Add(AnimationUtility.GetEditorCurve(currentClip, AnimationUtility.GetCurveBindings(currentClip)[i]));

            //Add a visualizer for each curve
            GameObject dummyGameObject = new GameObject();
            dummyGameObject.AddComponent<AnimationCurveVisualizer>();

            AnimationCurveVisualizer acv = dummyGameObject.GetComponent<AnimationCurveVisualizer>();
            //AnimationCurveVisualizer acv = ScriptableObject.CreateInstance<AnimationCurveVisualizer>();//new AnimationCurveVisualizer();

            acv.curveNumber = i;
            acv.animCurve = AnimationUtility.GetEditorCurve(currentClip, AnimationUtility.GetCurveBindings(currentClip)[i]); //animCurves[i];
            acv.keyframeObject = keyframeObject;
            acv.keyframeWorkArea = keyframeWorkArea.GetComponent<KeyframeWorkArea>();
            acv.parentAnimVisualizer = this;

                Transform nodeTransform;
                if (AnimationUtility.GetCurveBindings(currentClip)[i].path != "")
                {
                    nodeTransform = currentGameObject.transform.Find(AnimationUtility.GetCurveBindings(currentClip)[i].path);
                }
                else
                {
                    nodeTransform = currentGameObject.transform;
                }

                if (nodeTransform != null)
                {
                    acv.associatedNodeVisualizer = nodeTransform.GetChild(nodeTransform.childCount - 1).gameObject;        //Assumes Node marker will always be the last child
                                                                                                                           //acv.associatedNodeVisualizer.GetComponent<ModelNodeController>().SetAssociatedVisualizer(acv);			//Link the acv and the node marker both ways (so they can both talk to each other)

                    acv.associatedNodeVisualizer.GetComponent<ModelNodeController>().AddAssociatedCurveVisualizer(acv);


                    acv.associatedNodeVisualizer.GetComponent<ModelNodeController>().SetMainVisualizer(this);               //Makes it so the node visualizer can talk to this guy too
                }

                string propertyName = AnimationUtility.GetCurveBindings(currentClip)[i].propertyName;

                if (propertyName == "m_LocalPosition.x")
                    acv.curveType = AnimationCurveVisualizer.ACVType.PosX;
                else if (propertyName == "m_LocalPosition.y")
                    acv.curveType = AnimationCurveVisualizer.ACVType.PosY;
                else if (propertyName == "m_LocalPosition.z")
                    acv.curveType = AnimationCurveVisualizer.ACVType.PosZ;
                else if (propertyName == "m_LocalRotation.w")
                    acv.curveType = AnimationCurveVisualizer.ACVType.RotW;
                else if (propertyName == "m_LocalRotation.x")
                    acv.curveType = AnimationCurveVisualizer.ACVType.RotX;
                else if (propertyName == "m_LocalRotation.y")
                    acv.curveType = AnimationCurveVisualizer.ACVType.RotY;
                else if (propertyName == "m_LocalRotation.z")
                    acv.curveType = AnimationCurveVisualizer.ACVType.RotZ;
                else
                {
                    acv.curveType = AnimationCurveVisualizer.ACVType.Other;
                }

            acv.Refresh();


            animCurves_Visualizers.Add(acv);
        }

        for (int i = oldCount; i < animCurves_Visualizers.Count; i++)
        {
            if (i == 0)
                values.text = AnimationUtility.GetCurveBindings(currentClip)[i].propertyName + "\n";
            else
                values.text += AnimationUtility.GetCurveBindings(currentClip)[i].propertyName + "\n";
            //Then would do stuff to actually draw keyframes, etc.
        }

    }

    public void TogglePlayAnimation(){
		if (currentGameObject.GetComponent<Animator> ().GetFloat("PlaySpeed") > 0 || currentGameObject.GetComponent<Animator>().GetFloat("PlaySpeed") < 0) {
            //currentGameObject.GetComponent<Animator> ().speed = 0;
            currentGameObject.GetComponent<Animator>().SetFloat("PlaySpeed", 0);
            //currentGameObject.GetComponent<Animator> ().enabled = false;
		} else {
            //currentGameObject.GetComponent<Animator> ().speed = 1;
            currentGameObject.GetComponent<Animator>().SetFloat("PlaySpeed", 1);
            //currentGameObject.GetComponent<Animator> ().enabled = true;
		}
	}

	public void PauseAnimation(){
		currentGameObject.GetComponent<Animator> ().speed = 0;
	}

    public void PlayAnimationAtSpeed(float newSpeed)
    {

        //currentGameObject.GetComponent<Animator>().speed = newSpeed;
        if(currentGameObject.GetComponent<Animator>() != null)
            currentGameObject.GetComponent<Animator>().SetFloat("PlaySpeed", newSpeed);
       
        
    }

    public void thisIsWhatWeGot(AnimationClip clip, EditorCurveBinding binding, AnimationUtility.CurveModifiedType deleted){
        int jaksdjfksjdfkjsdf = 7;

        //AnimationCurve curveThing = AnimationUtility.GetEditorCurve(clip, binding);

        //int jdjdjdjdjdjdjdjdj = 383838;

        /*EditorCurveBinding[] thingers = AnimationUtility.GetCurveBindings(clip);

        for (int j = 0; j < thingers.Length; j++)
        {
            AnimationCurve curveThing = AnimationUtility.GetEditorCurve(clip, thingers[j]);
         
        }*/
    }

    // Use this for initialization
    void Awake () {
        //animCurves = new List<AnimationCurve> ();
        AnimationUtility.onCurveWasModified += thisIsWhatWeGot;

		animCurves_Visualizers = new List<AnimationCurveVisualizer> ();

		if (keyframeWorkArea.GetComponent<KeyframeWorkArea> () == null) {
			keyframeWorkArea.AddComponent<KeyframeWorkArea> ();

		}

		if (moCon == null) {
			gameObject.AddComponent<MocapController> ();
			moCon = gameObject.GetComponent<MocapController> ();
		}

        values.gameObject.transform.parent = keyframeWorkArea.GetComponent<KeyframeWorkArea>().keyframeSectionObject.transform;

		this.enabled = false;		//We don't need update so we disable this
	}
	
	// Update is called once per frame
	void Update () {
		//TODO: Get rid of all of this; update is not needed
		//TODO: Get rid of this HACK

		//for (int i = 0; i < animCurves.Count; i++) {
		//	if(i == 0)
		//		values.text = AnimationUtility.GetCurveBindings (currentClip) [i].propertyName + "\n";
		//	else
		//		values.text += AnimationUtility.GetCurveBindings (currentClip) [i].propertyName + "\n";
		//	//Then would do stuff to actually draw keyframes, etc.
		//}

		////-----END HACK
		//for (int i = 0; i < animCurves.Count; i++) {
		//	if (animCurves_Visualizers [i].needsToRefresh) {



		//		//We have to keep track of the current time because SetCurve resets it :(
		//		float currentTime = keyframeWorkArea.GetComponent<KeyframeWorkArea>().timelineVisualizer.GetAnimatorTime();

		//		//Debug.Log (currentTime);

		//		StartCoroutine (UpdateAnimationCurveAndResume (AnimationUtility.GetCurveBindings (currentClip) [i].path, AnimationUtility.GetCurveBindings (currentClip) [i].type, AnimationUtility.GetCurveBindings (currentClip) [i].propertyName, animCurves [i], currentTime));
		//		//currentClip.SetCurve (AnimationUtility.GetCurveBindings (currentClip) [i].path, AnimationUtility.GetCurveBindings (currentClip) [i].type, AnimationUtility.GetCurveBindings (currentClip) [i].propertyName, animCurves [i]);

		//		//keyframeWorkArea.GetComponent<KeyframeWorkArea> ().timelineVisualizer.ChangeTime (currentTime);

		//		//keyframeWorkArea.GetComponent<KeyframeWorkArea>().timelineVisualizer.animator.Play (keyframeWorkArea.GetComponent<KeyframeWorkArea>().timelineVisualizer.animator.GetCurrentAnimatorStateInfo (0).shortNameHash, 0, currentTime);
		//		//RefreshCurves();
		//		animCurves_Visualizers[i].needsToRefresh = false;
		//	}
		//}

		////Have the last updated curve thing here
		//for (int i = 0; i < animCurves.Count; i++) {
		//	if (animCurves_Visualizers [i].selected == true) {
		//		lastSelectedAnimCurve_Visualizer = i;
		//		break;
		//	}
		//}
	}

    public void RefreshAnimationCurve(int i) {

        //We have to keep track of the current time because SetCurve resets it :(
        float currentTime = keyframeWorkArea.GetComponent<KeyframeWorkArea>().timelineVisualizer.GetAnimatorTime();

        /*
        if (clipSwitch == false) {
            newClip.SetCurve(AnimationUtility.GetCurveBindings(currentClip)[i].path, AnimationUtility.GetCurveBindings(currentClip)[i].type, AnimationUtility.GetCurveBindings(currentClip)[i].propertyName, animCurves[i]);

            keyframeWorkArea.GetComponent<KeyframeWorkArea>().timelineVisualizer.ChangeClip(newClip);

            keyframeWorkArea.GetComponent<KeyframeWorkArea>().timelineVisualizer.ChangeTime(currentTime);

            currentClip.SetCurve(AnimationUtility.GetCurveBindings(currentClip)[i].path, AnimationUtility.GetCurveBindings(currentClip)[i].type, AnimationUtility.GetCurveBindings(currentClip)[i].propertyName, animCurves[i]);

             clipSwitch = !clipSwitch;
        } else{

            currentClip.SetCurve(AnimationUtility.GetCurveBindings(currentClip)[i].path, AnimationUtility.GetCurveBindings(currentClip)[i].type, AnimationUtility.GetCurveBindings(currentClip)[i].propertyName, animCurves[i]);

            keyframeWorkArea.GetComponent<KeyframeWorkArea>().timelineVisualizer.ChangeClip(currentClip);

            keyframeWorkArea.GetComponent<KeyframeWorkArea>().timelineVisualizer.ChangeTime(currentTime);

            newClip.SetCurve(AnimationUtility.GetCurveBindings(currentClip)[i].path, AnimationUtility.GetCurveBindings(currentClip)[i].type, AnimationUtility.GetCurveBindings(currentClip)[i].propertyName, animCurves[i]);


            clipSwitch = !clipSwitch;
        }*/

        //StartCoroutine(UpdateAnimationCurve(AnimationUtility.GetCurveBindings(currentClip)[i].path, AnimationUtility.GetCurveBindings(currentClip)[i].type, AnimationUtility.GetCurveBindings(currentClip)[i].propertyName, animCurves_Visualizers[i].animCurve, currentTime));

        UpdateAnimationCurve(AnimationUtility.GetCurveBindings(bufferClips[clipSwitch])[i].path, AnimationUtility.GetCurveBindings(bufferClips[clipSwitch])[i].type, AnimationUtility.GetCurveBindings(bufferClips[clipSwitch])[i].propertyName, animCurves_Visualizers[i].animCurve, currentTime);


        //-------
        //StartCoroutine (UpdateAnimationCurveAndResume (AnimationUtility.GetCurveBindings (currentClip) [i].path, AnimationUtility.GetCurveBindings (currentClip) [i].type, AnimationUtility.GetCurveBindings (currentClip) [i].propertyName, animCurves [i], currentTime));
        //-------

        //currentClip.SetCurve (AnimationUtility.GetCurveBindings (currentClip) [i].path, AnimationUtility.GetCurveBindings (currentClip) [i].type, AnimationUtility.GetCurveBindings (currentClip) [i].propertyName, animCurves [i]);

        //keyframeWorkArea.GetComponent<KeyframeWorkArea> ().timelineVisualizer.ChangeTime (currentTime);

        //keyframeWorkArea.GetComponent<KeyframeWorkArea>().timelineVisualizer.animator.Play (keyframeWorkArea.GetComponent<KeyframeWorkArea>().timelineVisualizer.animator.GetCurrentAnimatorStateInfo (0).shortNameHash, 0, currentTime);
        //RefreshCurves();
        animCurves_Visualizers[i].needsToRefresh = false;
	}

    int limbo = 0;



    void UpdateAnimationCurve(string path, System.Type type, string propertyName, AnimationCurve animCurve, float resumeTime)
    {
        /*
        if (clipSwitch == 0)
        {

            newClip.SetCurve(path, type, propertyName, animCurve);

            keyframeWorkArea.GetComponent<KeyframeWorkArea>().timelineVisualizer.ChangeClip(newClip);

            keyframeWorkArea.GetComponent<KeyframeWorkArea>().timelineVisualizer.ChangeTime(resumeTime);

            bufferClip.SetCurve(path, type, propertyName, animCurve);

            clipSwitch = 1;
        }
        else if (clipSwitch == 1)
        {

            bufferClip.SetCurve(path, type, propertyName, animCurve);

            keyframeWorkArea.GetComponent<KeyframeWorkArea>().timelineVisualizer.ChangeClip(bufferClip);

            keyframeWorkArea.GetComponent<KeyframeWorkArea>().timelineVisualizer.ChangeTime(resumeTime);

            currentClip.SetCurve(path, type, propertyName, animCurve);


            clipSwitch = 2;
        }
        else
        {
            currentClip.SetCurve(path, type, propertyName, animCurve);

            keyframeWorkArea.GetComponent<KeyframeWorkArea>().timelineVisualizer.ChangeClip(currentClip);

            keyframeWorkArea.GetComponent<KeyframeWorkArea>().timelineVisualizer.ChangeTime(resumeTime);

            newClip.SetCurve(path, type, propertyName, animCurve);

            clipSwitch = 0;
        }*/

        limbo++;

        if (limbo > 30)
        {
            int jaksdjfkjdjf = 4;

        }

        EditorCurveBinding[] thingers = AnimationUtility.GetCurveBindings(bufferClips[clipSwitch]);

        for (int j = 0; j < thingers.Length; j++)
        {
            AnimationCurve curveThing = AnimationUtility.GetEditorCurve(bufferClips[clipSwitch], thingers[j]);
            if (curveThing.keys.Length > 3)
            {
                int jkjkk = 7857;
            }
        }

        int index = 0;

        for (int j = 0; j < thingers.Length; j++)
        {
            if(thingers[j].path == path && thingers[j].propertyName == propertyName && thingers[j].type == type)
            {
                index = j;
                break;
            }
        }

        //currentClip.ClearCurves();

        AnimationCurve newCurve = new AnimationCurve(animCurve.keys);

        if (propertyName == "m_LocalPosition.y")
        {

            List<AnimationCurve> listOfCurves = new List<AnimationCurve>();

            for(int j = 0; j < thingers.Length; j++)
            {
                if (j != index)
                {
                    listOfCurves.Add(AnimationUtility.GetEditorCurve(bufferClips[clipSwitch], thingers[j]));
                }
                else
                {
                    listOfCurves.Add(newCurve);
                }
            }

            bufferClips[clipSwitch].ClearCurves();

            for (int j = 0; j < thingers.Length; j++)
            {
                bufferClips[clipSwitch].SetCurve(thingers[j].path, thingers[j].type, thingers[j].propertyName, listOfCurves[j]);
            }

            //AnimationCurve oldX = AnimationUtility.GetEditorCurve(currentClip, thingers[index - 1]);
            //AnimationCurve oldZ = AnimationUtility.GetEditorCurve(currentClip, thingers[index + 1]);

            //bufferClips[clipSwitch].SetCurve(path, type, "m_LocalPosition", null);

            //bufferClips[clipSwitch].ClearCurves();

            //AnimationUtility.GetEditorCurve(currentClip, thingers[index]).keys = animCurve.keys;

            //yield return null;

            //thingers = AnimationUtility.GetCurveBindings(bufferClips[clipSwitch]);

            //for (int j = 0; j < thingers.Length; j++)
            //{
            //    AnimationCurve curveThing = AnimationUtility.GetEditorCurve(bufferClips[clipSwitch], thingers[j]);
            //}

            //for (int j = 0; j < 1000; j++)
            //{
            //    //bufferClips[clipSwitch].SetCurve(path, type, propertyName, newCurve);
            //    //bufferClips[clipSwitch].SetCurve("", typeof(Transform), "localPosition.y", newCurve);
            //    //currentClip.SetCurve(path, type, propertyName, newCurve);
            //}


            //bufferClips[clipSwitch].SetCurve(path, type, "localPosition.y", newCurve);
            //currentClip.SetCurve(path, type, propertyName, newCurve);
            //bufferClips[clipSwitch].SetCurve(path, type, "localPosition.x", oldX);
            //bufferClips[clipSwitch].SetCurve(path, type, "localPosition.z", oldZ);*/

            //bufferClips[clipSwitch].SetCurve(path, type, "m_LocalPosition", animCurve);

        }

        //yield return null;

        thingers = AnimationUtility.GetCurveBindings(bufferClips[clipSwitch]);

        for (int j = 0; j < thingers.Length; j++)
        {

            AnimationCurve curveThing = AnimationUtility.GetEditorCurve(bufferClips[clipSwitch], thingers[j]);
            if(j == 1)
            {
                //curveThing.RemoveKey(1);
                //bufferClips[clipSwitch].SetCurve("", typeof(Transform), "m_LocalPosition.y", curveThing);
               
            }
        }

        //keyframeWorkArea.GetComponent<KeyframeWorkArea>().timelineVisualizer.ChangeClip(bufferClips[clipSwitch]);

        //keyframeWorkArea.GetComponent<KeyframeWorkArea>().timelineVisualizer.ChangeTime(resumeTime);
        //keyframeWorkArea.GetComponent<KeyframeWorkArea>().timelineVisualizer.ChangeTime((resumeTime + Time.deltaTime / animCurve[animCurve.length - 1].time) % 1.0f);

        for (int j = 0; j < thingers.Length; j++)
        {
            AnimationCurve curveThing = AnimationUtility.GetEditorCurve(bufferClips[clipSwitch], thingers[j]);
        }

        //currentClip = bufferClips [clipSwitch];

        for (int j = 0; j < thingers.Length; j++)
        {
            AnimationCurve curveThing = AnimationUtility.GetEditorCurve(bufferClips[clipSwitch], thingers[j]);
        }

        //clipSwitch = (clipSwitch + 1) % bufferClips.Length;

        for(int i = 0; i < bufferClips.Length - 1; i++)     //bufferClips.Length - 1
        {
            if (propertyName == "m_LocalPosition.y")
            {
                //AnimationCurve oldX = AnimationUtility.GetEditorCurve(currentClip, thingers[index - 1]);
                //AnimationCurve oldZ = AnimationUtility.GetEditorCurve(currentClip, thingers[index + 1]);

                //bufferClips[(clipSwitch + i) % bufferClips.Length].ClearCurves();


                //AnimationUtility.GetEditorCurve(bufferClips[(clipSwitch + i) % bufferClips.Length], thingers[index]).keys = animCurve.keys;
                for (int j = 0; j < 1000; j++)
                {
                    //bufferClips[(clipSwitch + i) % bufferClips.Length].SetCurve(path, type, propertyName, newCurve);
                    //bufferClips[(clipSwitch + i) % bufferClips.Length].SetCurve("", typeof(Transform), "localPosition.y", newCurve);
                }

                //yield return null;


                //bufferClips[(clipSwitch + i) % bufferClips.Length].SetCurve(path, type, "localPosition.y", newCurve);

                /*
                bufferClips[clipSwitch].SetCurve(path, type, "localPosition.y", animCurve);
                bufferClips[clipSwitch].SetCurve(path, type, "localPosition.x", oldX);
                bufferClips[clipSwitch].SetCurve(path, type, "localPosition.z", oldZ);*/

                //bufferClips[clipSwitch].SetCurve(path, type, "m_LocalPosition", animCurve);

            }
        }


        //yield return null;
    }

    public void UpdateCurrentClip(string path, System.Type type, string propertyName, AnimationCurve animCurve, float resumeTime)
    {
        for(int i = 0; i < bufferClips.Length; i++)
        {
            EditorCurveBinding[] thingers = AnimationUtility.GetCurveBindings(bufferClips[i]);

            for (int j = 0; j < thingers.Length; j++)
            {
                AnimationCurve curveThing = AnimationUtility.GetEditorCurve(bufferClips[i], thingers[j]);
            }

            bufferClips[i].SetCurve(path, type, propertyName, animCurve);

            thingers = AnimationUtility.GetCurveBindings(bufferClips[i]);
            for (int j = 0; j < thingers.Length; j++) {
                AnimationCurve curveThing = AnimationUtility.GetEditorCurve(bufferClips[i], thingers[j]);
            }
        }

        keyframeWorkArea.GetComponent<KeyframeWorkArea>().timelineVisualizer.ChangeTime((resumeTime + Time.deltaTime / animCurve[animCurve.length - 1].time) % 1.0f);
    }


    IEnumerator UpdateAnimationCurveAndResume(string path, System.Type type, string propertyName, AnimationCurve animCurve, float resumeTime){

		/*waitFrame = (++waitFrame) % NUM_WAIT_FRAMES;
		if (waitFrame != 0)
			yield return null;
		*/
		//AnimationClip newClip = new AnimationClip ();
		newClip.name = currentClip.name;

        //Perform a deep copy
        for (int i = 0; i < AnimationUtility.GetCurveBindings (currentClip).Length; i++) {
			newClip.SetCurve (AnimationUtility.GetCurveBindings (currentClip) [i].path, AnimationUtility.GetCurveBindings (currentClip) [i].type, AnimationUtility.GetCurveBindings (currentClip) [i].propertyName, AnimationUtility.GetEditorCurve (currentClip, AnimationUtility.GetCurveBindings (currentClip) [i]));
		}

		newClip.SetCurve (path, type, propertyName, animCurve);
		keyframeWorkArea.GetComponent<KeyframeWorkArea> ().timelineVisualizer.ChangeClip (newClip);
		//currentClip.SetCurve (path, type, propertyName, animCurve);
		//yield return new WaitForEndOfFrame ();

		currentClip = newClip;

		//yield return new WaitForEndOfFrame ();

		keyframeWorkArea.GetComponent<KeyframeWorkArea> ().timelineVisualizer.ChangeTime ((resumeTime + Time.deltaTime/animCurve[animCurve.length - 1].time) % 1.0f);
	//	yield return null;
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
					//keyframeWorkArea.GetComponent<KeyframeWorkArea> ().timelineVisualizer.GetComponent<TimelineVisualizer> ().animator.applyRootMotion = false;
					break;
				}
			}
		}
	}

	public void StopMotionCapture(){
		moCon.StopCapturing ();
		//keyframeWorkArea.GetComponent<KeyframeWorkArea> ().timelineVisualizer.GetComponent<TimelineVisualizer> ().animator.applyRootMotion = true;
	}


	public AnimationCurveVisualizer GetAnimCurveVisualizer(int index){
		return animCurves_Visualizers [index];
	}

    public GameObject GetCurrentGameObject()
    {
        return currentGameObject;
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
