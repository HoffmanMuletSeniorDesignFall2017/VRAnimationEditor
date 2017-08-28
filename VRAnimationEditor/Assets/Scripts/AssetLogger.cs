using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class AssetLogger : MonoBehaviour {
	public bool loadAssets;
	public List<AnimationClip> animationClips;
	public List<GameObject> models;
	// Use this for initialization
	void Start () {
		animationClips = new List<AnimationClip> ();
		models = new List<GameObject> ();
		LoadAssets ();
		loadAssets = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (loadAssets) {
			LoadAssets ();
			loadAssets = false;
		}
	}

	private void LoadAssets(){
		LoadAnimations ();
		LoadModels ();
	}

	// Load all animations in the project.
	private void LoadAnimations(){
		// Clear previous list of animations.
		animationClips.Clear ();
		// Search asset database by type for animation clips, get their GUIDs.
		string[] animGuids = AssetDatabase.FindAssets ("t:AnimationClip");
		for (int i = 0; i < animGuids.Length; i++) {
			// Get animation clip asset path.
			string animPath = AssetDatabase.GUIDToAssetPath (animGuids [i]);
			// Load animation clip.
			AnimationClip anim = AssetDatabase.LoadAssetAtPath<AnimationClip> (animPath);
			// Store animation clip in list.
			animationClips.Add (anim);
		}
	}

	private void LoadModels(){
		models.Clear ();
		string[] modelGuids = AssetDatabase.FindAssets ("t:GameObject");
		for (int i = 0; i < modelGuids.Length; i++) {
			string modelPath = AssetDatabase.GUIDToAssetPath (modelGuids [i]);
			GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject> (modelPath);
			if (IsModel (obj)) {
				models.Add (obj);
			}
		}
	}

	private bool IsModel(GameObject obj){
		if (obj.GetComponent<SkinnedMeshRenderer> () != null) {
			return true;
		}
		if (obj.GetComponent<MeshRenderer> () != null) {
			return true;
		}
		bool isModel = false;
		for (int i = 0; i < obj.transform.childCount && !isModel; i++) {
			isModel |= IsModel (obj.transform.GetChild (i).gameObject);
		}
		return isModel;
	}
}
