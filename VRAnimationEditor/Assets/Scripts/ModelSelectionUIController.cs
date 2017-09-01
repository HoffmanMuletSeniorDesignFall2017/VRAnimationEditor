using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelSelectionUIController : MonoBehaviour {
    public GameObject modelTilePrefab;
    public AssetLogger assetLogger;
    public Transform contentPanel;

	// Use this for initialization
	void Start () {
        InitModelTiles();
	}

    private void InitModelTiles(){
        for(int i = 0; i < assetLogger.models.Count; i++){
            GameObject modelTile = Instantiate<GameObject>(modelTilePrefab, contentPanel);
            modelTile.GetComponent<ModelTileController>().modelUICtrl = this;
            modelTile.GetComponent<ModelTileController>().SetModel(assetLogger.models[i]);
            modelTile.name = "model tile " + i;
        }
    }

    public void SetSelectedModel(GameObject selection){
        Debug.Log("Model " + selection.name + " selected");
    }
}
