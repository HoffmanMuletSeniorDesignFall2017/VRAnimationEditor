using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class AnimationEditorFunctions {	//This static class is basically a collection of useful functions to edit and create animations.
																//	For example, a UI class would know about this library of functions and would call them when it wished to manipulate data

	public static AnimationClip CreateNewAnimation(string newName){	//Creates a new Animation clip and adds it as an asset. Returns the clip.
		AnimationClip newAnimClip = new AnimationClip ();

		//TODO: Make a user-configurable name and destination?

		AssetDatabase.CreateAsset(newAnimClip, string.Concat("Assets/", newName, ".anim"));

		return newAnimClip;
	}

}
