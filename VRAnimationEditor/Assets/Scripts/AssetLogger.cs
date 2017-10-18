using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class AssetLogger
{
	private const int MinModelLevels = 3;

	public static List<AnimationClip> GetAnimations()
    {
		List<AnimationClip> animClips = new List<AnimationClip> ();
        // Search asset database by type for animation clips, get their GUIDs.
        string[] animGuids = AssetDatabase.FindAssets("t:AnimationClip");
        for (int i = 0; i < animGuids.Length; i++)
        {
            // Get animation clip asset path from GUID.
            string animPath = AssetDatabase.GUIDToAssetPath(animGuids[i]);
            // Load animation clip.
            AnimationClip anim = AssetDatabase.LoadAssetAtPath<AnimationClip>(animPath);
            // Store animation clip in list.
			animClips.Add(anim);
        }
		return animClips;
    }

	public static List<GameObject> GetModels()
    {
		List<GameObject> models = new List<GameObject> ();
        // Get GameObject GUIDs from asset database.
        string[] gameObjGuids = AssetDatabase.FindAssets("t:GameObject");
        for (int i = 0; i < gameObjGuids.Length; i++)
        {
            // Get GameObject path from GUID.
            string gameObjPath = AssetDatabase.GUIDToAssetPath(gameObjGuids[i]);
            // Load GameObject.
            GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(gameObjPath);
            // If the GameObject is a model, add it to the list.
            if (IsModel(obj))
            {
                models.Add(obj);
            }
        }
		return models;
    }

    // Check if a GameObject is a 3D model.
	private static bool IsModel(GameObject obj)
    {
		return HasSkinnedMesh (obj) && (GetChildLevelCount (obj) >= MinModelLevels);
    }

	private static bool HasSkinnedMesh(GameObject obj){
		if (obj.GetComponent<SkinnedMeshRenderer> () != null) {
			return true;
		}
		bool hasSkinnedMesh = false;
		for (int i = 0; i < obj.transform.childCount && !hasSkinnedMesh; i++) {
			hasSkinnedMesh |= HasSkinnedMesh (obj.transform.GetChild (i).gameObject);
		}
		return hasSkinnedMesh;
	}

	private static int GetChildLevelCount(GameObject obj){
		int maxChildLevels = 0;
		for (int i = 0; i < obj.transform.childCount; i++) {
			GameObject childObj = obj.transform.GetChild (i).gameObject;
			int childLevels = GetChildLevelCount (childObj);
			maxChildLevels = Mathf.Max (maxChildLevels, childLevels);
		}
		return maxChildLevels + 1;
	}
}
