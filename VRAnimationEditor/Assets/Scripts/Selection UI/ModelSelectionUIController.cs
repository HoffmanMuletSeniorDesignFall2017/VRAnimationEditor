using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelSelectionUIController : MonoBehaviour {
    public GameObject modelTilePrefab;
    public Transform contentPanel;
    // Set by session manager when instantiating the UI.
    public SessionManager sessionManager;

	// Use this for initialization
	void Start () {
        InitModelTiles();
	}

    private void InitModelTiles(){
		List<GameObject> models = AssetLogger.GetModels ();
		for(int i = 0; i < models.Count; i++){
            GameObject modelTile = Instantiate<GameObject>(modelTilePrefab, contentPanel);
            modelTile.GetComponent<ModelTileController>().modelUICtrl = this;
            modelTile.GetComponent<ModelTileController>().SetModel(models[i]);
            modelTile.name = "model tile " + i;
        }
    }

    public void SetSelectedModel(GameObject selection){
        Debug.Log("Model " + selection.name + " selected");
        sessionManager.sessionModel = selection;
        sessionManager.OnModelSelected();
        Destroy(gameObject);
    }
}
