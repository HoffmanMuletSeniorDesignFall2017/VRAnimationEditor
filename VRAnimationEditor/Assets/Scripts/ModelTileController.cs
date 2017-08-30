using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModelTileController : MonoBehaviour, IPointerReciever {
    public Text modelNameText;
    public Image highlight;
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
