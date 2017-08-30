using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Script for controlling tiles in the model selection UI.
public class ModelTileController : MonoBehaviour, IPointerReciever {
    // Tile UI text component.
    public Text modelNameText;
    // Tile highlight image component.
    public Image highlight;
    // Reference to parent ModelSelectionUIController.
    // This is set by that script when it instantiates the tile prefab.
    // The reference is needed here for setting the selected model, which is
    // done through this parent script.
    public ModelSelectionUIController modelUICtrl;

    private GameObject model;

    void Start(){
        highlight.enabled = false;
    }

    public void SetModel(GameObject model){
        this.model = model;
        modelNameText.text = model.name;
    }

    public void OnPointerEnter(){
        highlight.enabled = true;
    }

    public void OnPointerExit(){
        highlight.enabled = false;
    }

    public void OnClick(){
        modelUICtrl.SetSelectedModel(model);
    }
}
