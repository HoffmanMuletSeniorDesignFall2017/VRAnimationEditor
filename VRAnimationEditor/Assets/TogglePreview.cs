using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogglePreview : MonoBehaviour {

    private NodeVisualizationManager nvm;
    public GameObject modelSpawnAnchor;

    bool toggled = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick()
    {

        if (nvm == null)
            nvm = modelSpawnAnchor.GetComponentInChildren<NodeVisualizationManager>();

        if (!toggled)
        {
            nvm.RestoreMaterials();
            nvm.HideNodeMarkers();
            toggled = true;
        }
        else
        {
            nvm.ReplaceMaterialsExternal();
            nvm.ShowNodeMarkers();
            toggled = false;
        }
    }
}
