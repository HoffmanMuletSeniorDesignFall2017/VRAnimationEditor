using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class ModelSelectionPanel : MonoBehaviour, IModelSelector {
    private const int MinModelLevels = 4;

    public GameObject modelTilePrefab;
    public GameObject contentObj;
    public ScrollRect scrollRect;

    private IModelRequester modelRequester;

    void Start(){
        StartCoroutine("LoadModelsRoutine");
    }

    void Update(){
        scrollRect.
    }

    public void RequestModel(IModelRequester requester){
        modelRequester = requester;
    }

    private void ClearContent(){
        for (int i = 0; i < contentObj.transform.childCount; i++)
        {
            Destroy(contentObj.transform.GetChild(i));
        }
    }

    private void AddModel(GameObject model){
        
        GameObject tile = Instantiate(modelTilePrefab, contentObj.transform);
        tile.GetComponent<ModelTile>().Init(model);
    }

    public IEnumerator LoadModelsRoutine(){
        string[] gameObjGuids = AssetDatabase.FindAssets("t:GameObject");
        for (int i = 0; i < gameObjGuids.Length; i++)
        {
            // Get GameObject path from GUID.
            string gameObjPath = AssetDatabase.GUIDToAssetPath(gameObjGuids[i]);
            // Check extension
            if (IsValidModelExtension(System.IO.Path.GetExtension(gameObjPath)))
            {
                // Load GameObject.
                GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(gameObjPath);
                // If the GameObject is a model, add it to the list.
                if (IsAnimationModel(obj))
                {
                    AddModel(obj);
                }
            }
            yield return null; 
        }
    }

    private static bool IsValidModelExtension(string extension){
        if (extension == ".prefab")
        {
            return false;
        }
        return true;
    }

    // Check if a GameObject is a 3D model.
    private static bool IsAnimationModel(GameObject obj)
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