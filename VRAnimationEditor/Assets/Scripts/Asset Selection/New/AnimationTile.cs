using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;

public class AnimationTile : MonoBehaviour, IPointerReciever, IButtonAxisReciever
{
    public Text nameTextBox;
    public Image highlight;
    public Color highlightColor, selectColor;
    private bool isSelected;
    public AnimationClip anim;
    public AnimationSelectionPanel panel;
    public string animPath;

    private List<int> focusingInteractorIds;

    void Awake()
    {
        focusingInteractorIds = new List<int>();
    }

    public void Init(AnimationClip anim, AnimationSelectionPanel panel)
    {
        this.anim = anim;
        nameTextBox.text = anim.name;
        highlight.color = highlightColor;
        highlight.enabled = false;
        this.panel = panel;
    }

    private void UpdateHighlight()
    {
        highlight.enabled = (focusingInteractorIds.Count > 0 || isSelected);
    }

    public void OnPointerEnter(int interactorId)
    {
        ButtonAxisEmitterLookup.RegisterReciever(this, interactorId);
        focusingInteractorIds.Add(interactorId);
        UpdateHighlight();
    }

    public void OnPointerExit(int interactorId)
    {
        focusingInteractorIds.Remove(interactorId);
        UpdateHighlight();
        ButtonAxisEmitterLookup.UnregisterReciever(this, interactorId);
    }

    public void OnRecieveButton(int sourceID, int buttonID, bool buttonState)
    {
        if (focusingInteractorIds.Contains(sourceID) && (buttonID == 0) && (buttonState == true))
        {
            if(anim == null)
            {
                anim = AssetDatabase.LoadAssetAtPath<AnimationClip>(animPath);
            }
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
        if (state == true)
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
