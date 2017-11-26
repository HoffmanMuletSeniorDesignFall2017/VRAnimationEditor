using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class AssetSelectionButton_New : AssetSelectionButton
{
    void Update()
    {
        button.interactable = modelSelPanel.selectedTile != null;
    }

    new public void OnClick()
    {
        GameObject model = modelSelPanel.selectedTile.model;
        AnimationClip newAnimation = AnimationEditorFunctions.CreateNewAnimation("NewAnimation");

        GameObject animModel = AnimationEditorFunctions.InstantiateWithAnimation(model, newAnimation, modelSpawnAnchor);
        SetupNodeVisualization(animModel);

        StartCoroutine(WaitAndDoTheThing(animModel, newAnimation));
        //rigRoot.SetActive(false);
    }

}
