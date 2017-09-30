using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetSelectionController : MonoBehaviour {
    public GameObject animTilePrefab, modelTilePrefab;

    public void SetupModelSelection(){
        List<GameObject> models = VRAssetManager.GetSkinnedModels();
        for (int i = 0; i < 10; i++)
        {
            GameObject.Instantiate(modelTilePrefab, transform);
        }
    }
}
