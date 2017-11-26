using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;

public class ModelTile : MonoBehaviour, IPointerReciever, IButtonAxisReciever {
    public Text nameTextBox;
    public RawImage icon;
    public Image highlight;
    public Color highlightColor, selectColor;
    private bool isSelected;
    public GameObject model;
    public ModelSelectionPanel panel;

    private List<int> focusingInteractorIds;

    void Awake(){
        focusingInteractorIds = new List<int>();
    }

    public void Init(GameObject model, ModelSelectionPanel panel){
        this.model = model;
        nameTextBox.text = model.name;
        icon.texture = AssetPreview.GetAssetPreview(model);
        highlight.color = highlightColor;
        highlight.enabled = false;
        this.panel = panel;
    }

    private void UpdateHighlight(){
        highlight.enabled = (focusingInteractorIds.Count > 0 || isSelected);
    }

    public void OnPointerEnter(int interactorId){
        ButtonAxisEmitterLookup.RegisterReciever(this, interactorId);
        focusingInteractorIds.Add(interactorId);
        UpdateHighlight();
    }

    public void OnPointerExit(int interactorId){
        focusingInteractorIds.Remove(interactorId);
        UpdateHighlight();
        ButtonAxisEmitterLookup.UnregisterReciever(this, interactorId);
    }

    public void OnRecieveButton(int sourceID, int buttonID, bool buttonState)
    {
        if(focusingInteractorIds.Contains(sourceID) && (buttonID == 0) && (buttonState == true))
        {
            panel.SetSelectedTile(this);
        }
    }

    public void OnRecieveAxis(int sourceID, int axisID, float axisValue)
    {
        
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void SetSelect(bool state)
    {
        if(state == true)
        {
            isSelected = true;
            highlight.color = selectColor;
        }
        else
        {
            isSelected = false;
            highlight.color = highlightColor;
        }
        UpdateHighlight();
    }
}
