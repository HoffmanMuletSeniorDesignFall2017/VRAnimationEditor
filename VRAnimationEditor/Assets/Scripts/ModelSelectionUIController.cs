using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelSelectionUIController : MonoBehaviour {
    public GameObject modelTilePrefab;
    public AssetLogger assetLogger;

	// Use this for initialization
	void Start () {
        AddModelTiles();
	}

    private void AddModelTiles(){
        for(int i = 0; i < assetLogger.models.Count; i++){
            GameObject modelTile = Instantiate<GameObject>(modelTilePrefab, transform);
            modelTile.GetComponent<ModelTileController>().modelUICtrl = this;
            modelTile.GetComponent<ModelTileController>().SetModel(assetLogger.models[i]);
            modelTile.name = "model tile " + i;
        }
    }

    public void SetSelectedModel(GameObject selection){
        Debug.Log("Model " + selection.name + " selected");
    }
}
