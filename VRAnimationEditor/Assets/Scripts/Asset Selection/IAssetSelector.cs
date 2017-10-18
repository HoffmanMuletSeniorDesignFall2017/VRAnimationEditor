using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public abstract class IAssetSelector : MonoBehaviour
{
    protected const int MaxComponentSearchDepth = 2;
    protected const int MinAnimModelDepth = 2;

	// Interface methods.
    public abstract void RequestSelectedModel(IAssetRequester requester);
    public abstract void RequestSelectedModel(IAssetRequester requester, AnimationClip animClipFilter);
    public abstract void RequestSelectedAnimationClip(IAssetRequester requester);
    public abstract void RequestSelectedAnimationClip(IAssetRequester requester, GameObject modelFilter);

	// Methods for accessing asset information from database.

	protected static List<GameObject> GetAllAnimModels(bool requireSkinnedMesh = true){
        List<GameObject> animModels = new List<GameObject>();
        // Get GUIDs of all GameObjects in database.
        string[] gameObjGuids = AssetDatabase.FindAssets("t:GameObject");
        for (int i = 0; i < gameObjGuids.Length; i++)
        {
            // Get actual GameObject.
            string gameObjPath = AssetDatabase.GUIDToAssetPath(gameObjGuids[i]);
            GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(gameObjPath);
            // Only check for skinned mesh if we want to filter it.
            // Filter out models without a skinned mesh renderer (probably not meant to be animated).
            if(!requireSkinnedMesh || HasSkinnedMeshRenderer(obj.transform)){
                // Filter out models that are too shallow (no bones).
                if(HasMinDepth(obj.transform, MinAnimModelDepth)){
                    // Add gameobjects that pass filters.
                    animModels.Add(obj);
                }
            }
        }
        return animModels;
    }

	protected static List<AnimationClip> GetAllAnimationClips(){
        List<AnimationClip> animClips = new List<AnimationClip>();
        string[] animGuids = AssetDatabase.FindAssets("t:AnimationClip");
        for (int i = 0; i < animGuids.Length; i++)
        {
            string animPath = AssetDatabase.GUIDToAssetPath(animGuids[i]);
            AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(animPath);
            animClips.Add(clip);
        }
        return animClips;
    }

	protected static List<GameObject> GetCompatibleAnimModels(AnimationClip animClip){
        EditorCurveBinding[] editorCurves = AnimationUtility.GetCurveBindings(animClip);
        string debug = "";
        for (int i = 0; i < editorCurves.Length; i++)
        {
            debug += editorCurves[i].path + "\r\n";
        }
        Debug.Log(debug);

        return GetAllAnimModels();
    }

	protected static bool HasSkinnedMeshRenderer(Transform obj, int hierarchyDepth = 0){
        // Check if there is skinned mesh renderer on this gameobject.
        if (obj.GetComponent<SkinnedMeshRenderer>() != null)
        {
            return true;
        }
        // Check if there is a skinned mesh renderer on this gameobject's children.
        // Pass incremented hierarcy depth to limit search depth.
        if (hierarchyDepth < MaxComponentSearchDepth)
        {
            for (int i = 0; i < obj.childCount; i++)
            {
                if(HasSkinnedMeshRenderer(obj.GetChild(i), hierarchyDepth + 1)){
                    return true;
                }
            }
        }
        return false;
    }

	protected static bool HasMinDepth(Transform obj, int minDepth, int currentDepth = 0){
        if (currentDepth >= minDepth)
        {
            return true;
        }
        for (int i = 0; i < obj.childCount; i++)
        {
            if (HasMinDepth(obj.GetChild(i), minDepth, currentDepth + 1))
            {
                return true;
            }
        }
        return false;
    }
}
