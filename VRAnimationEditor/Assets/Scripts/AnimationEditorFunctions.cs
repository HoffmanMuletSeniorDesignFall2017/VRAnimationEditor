using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//using UnityEditorInternal;

public static class AnimationEditorFunctions {	//This static class is basically a collection of useful functions to edit and create animations.
																//	For example, a UI class would know about this library of functions and would call them when it wished to manipulate data

	public static AnimationClip CreateNewAnimation(string newName){	//Creates a new Animation clip and adds it as an asset. Returns the clip.
		AnimationClip newAnimClip = new AnimationClip ();

		//TODO: Make a user-configurable name and destination?

		AssetDatabase.CreateAsset(newAnimClip, string.Concat("Assets/", newName, ".anim"));

		//newAnimClip.legacy = false;
		//newAnimClip.wrapMode = WrapMode.Loop;
		//newAnimClip.wrapMode = WrapMode.PingPong;
		AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(newAnimClip);
		settings.loopTime = true;
		AnimationUtility.SetAnimationClipSettings(newAnimClip, settings);

		return newAnimClip;
	}

    public static void ForceLoop(AnimationClip animClip){
		animClip.legacy = false;
        AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(animClip);
        settings.loopTime = true;
        // How to set loop pose?

        AnimationUtility.SetAnimationClipSettings(animClip, settings);
        animClip.wrapMode = WrapMode.Loop;
    }

	public static GameObject InstantiateWithAnimation(GameObject go, AnimationClip animClip){	//Used after the user has selected a GameObject and an AnimationClip to edit; instantiates the GameObject into the scene and sets it up appropriately.
        ForceLoop(animClip);
		GameObject newGo = GameObject.Instantiate (go);

		if (newGo.GetComponent<Animator> () != null) {
			//We don't like Animator for our purposes; ergo, we get rid of it
			//TODO: Maybe add support for Animator??
			//Object.Destroy(newGo.GetComponent<Animator>());
		}

		/*if (newGo.GetComponent<Animation> () != null) {
			newGo.GetComponent<Animation> ().clip = animClip;
		} else {
			newGo.AddComponent<Animation> ();
			newGo.GetComponent<Animation> ().clip = animClip;
		}
			
		newGo.GetComponent<Animation> ().AddClip (animClip, "default");
		*/

		if (newGo.GetComponent<Animator> () != null) {
		
			UnityEditor.Animations.AnimatorController ac = new UnityEditor.Animations.AnimatorController ();
			ac.AddLayer ("default");
			ac.AddMotion (animClip);
			newGo.GetComponent<Animator> ().runtimeAnimatorController = ac;
		}

		return newGo;
		//TODO: May have to tell an Editor class somewhere that our current object being edited is this newly instantiated GameObject

	}

}
