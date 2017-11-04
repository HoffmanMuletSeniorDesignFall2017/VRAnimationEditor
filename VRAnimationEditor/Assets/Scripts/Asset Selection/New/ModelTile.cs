using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class ModelTile : MonoBehaviour, IPointerReciever {
    public Text nameTextBox;
    public RawImage icon;
    public Image highlight;

    private GameObject model;
    private List<int> focusingInteractorIds;

    void Awake(){
        focusingInteractorIds = new List<int>();
    }

    public void Init(GameObject model){
        this.model = model;
        nameTextBox.text = model.name;
        icon.texture = AssetPreview.GetAssetPreview(model);
        highlight.enabled = false;
    }

    private void UpdateHighlight(){
        highlight.enabled = focusingInteractorIds.Count > 0;
    }

    public void OnPointerEnter(int interactorId){
        focusingInteractorIds.Add(interactorId);
        UpdateHighlight();
    }

    public void OnPointerExit(int interactorId){
        focusingInteractorIds.Remove(interactorId);
        UpdateHighlight();
    }
}
