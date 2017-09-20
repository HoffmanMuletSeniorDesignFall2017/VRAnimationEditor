using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// Script for logging information about project assets in editor mode.
[ExecuteInEditMode]
public class AssetLogger : MonoBehaviour
{
    // Used to trigger reloading of info.
    public bool logAssets;
    // List of all animation clips in the project.
    public List<AnimationClip> animationClips;
    // List of all 3D models in the project.
    // I don't think this should include prefabs, though the current logging method does.
    public List<GameObject> models;

    void Start()
    {
        // Instantiate lists.
        animationClips = new List<AnimationClip>();
        models = new List<GameObject>();
        // Log assets 
        LogAssets();
        logAssets = false;
    }

    void Update()
    {
        // Log assets if needed.
        if (logAssets)
        {
            LogAssets();
            logAssets = false;
        }
    }

    // Create lists of assets in project.
    private void LogAssets()
    {
        LogAnimations();
        LogModels();
    }

    // Log all animations in the project.
    private void LogAnimations()
    {
        // Clear previous list of animations.
        animationClips.Clear();
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
    }

    // Log all models in the project.
    private void LogModels()
    {
        // Clear previous list of models.
        models.Clear();
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
    }

    // Check if a GameObject is a 3D model.
    private bool IsModel(GameObject obj)
    {
        // If it has a SkinnedMeshRenderer component, it is a model.
        if (obj.GetComponent<SkinnedMeshRenderer>() != null)
        {
            return true;
        }
        // If it has a MeshRenderer component, it is a model (but not when this is commented out!).
        /*if (obj.GetComponent<MeshRenderer>() != null)
        {
            return true;
        }*/

        // Check children for above componenets.
        bool isModel = false;
        // Loop through children until there are no more children, or a child is determined to
        // be a model.
        for (int i = 0; i < obj.transform.childCount && !isModel; i++)
        {
            isModel |= IsModel(obj.transform.GetChild(i).gameObject);
        }
        return isModel;
    }
}
