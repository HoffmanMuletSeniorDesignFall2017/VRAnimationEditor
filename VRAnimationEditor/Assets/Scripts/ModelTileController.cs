using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModelTileController : MonoBehaviour {
    public Text modelNameText;

    private GameObject model;

    public void SetModel(GameObject model){
        this.model = model;
        modelNameText.text = model.name;
    }
}
