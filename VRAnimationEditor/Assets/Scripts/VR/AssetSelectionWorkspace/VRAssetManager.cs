using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class VRAssetManager
{
    
    public static List<AnimationClip> GetProjectAnimationClips()
    {
        List<AnimationClip> animationClips = new List<AnimationClip>();
        // Search asset database by type for animation clips, get their GUIDs.
        string[] animGuids = AssetDatabase.FindAssets("t:AnimationClip");
        for (int i = 0; i < animGuids.Length; i++)
        {
            // Get animation clip asset path from GUID.
            string animPath = AssetDatabase.GUIDToAssetPath(animGuids[i]);
            // Load animation clip.
            AnimationClip anim = AssetDatabase.LoadAssetAtPath<AnimationClip>(animPath);
            // Store animation clip in list.
            animationClips.Add(anim);
        }
        return animationClips;
    }

    public static List<GameObject> GetSkinnedModels()
    {
        List<GameObject> models = new List<GameObject>();
        // Get GameObject GUIDs from asset database.
        string[] gameObjGuids = AssetDatabase.FindAssets("t:GameObject");
        for (int i = 0; i < gameObjGuids.Length; i++)
        {
            // Get GameObject path from GUID.
            string gameObjPath = AssetDatabase.GUIDToAssetPath(gameObjGuids[i]);
            // Load GameObject.
            GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(gameObjPath);
            // If the GameObject is a skinned model, add it to the list.
            if (IsSkinnedModel(obj))
            {
                models.Add(obj);
            }
        }
        return models;
    }

    // Check if a GameObject is a 3D model.
    private static bool IsSkinnedModel(GameObject obj)
    {
        // If it has a SkinnedMeshRenderer component, it is a model.
        if (obj.GetComponent<SkinnedMeshRenderer>() != null)
        {
            return true;
        }
        // Check children for above skinned mesh renderer..
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            if (IsSkinnedModel(obj.transform.GetChild(i).gameObject))
            {
                return true;
            }
        }
        return false;
    }
}
