using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class AssetSelectionButtonNew : AssetSelectionButton
{
    void Update()
    {
        button.interactable = modelSelPanel.selectedTile != null;
    }

    new public void OnClick()
    {
        GameObject model = modelSelPanel.selectedTile.model;
        AnimationClip newAnimation = AnimationEditorFunctions.CreateNewAnimation("Test");

        AnimationCurve newAnimationCurve = new AnimationCurve();

        newAnimationCurve.AddKey(0f, 0f);

        newAnimationCurve.AddKey(1f, 0f);

        newAnimation.SetCurve("", typeof(Transform), "m_LocalPosition.x", newAnimationCurve);

        GameObject animModel = AnimationEditorFunctions.InstantiateWithAnimation(model, newAnimation, modelSpawnAnchor);
        SetupNodeVisualization(animModel);

        StartCoroutine(WaitAndDoTheThing(animModel, newAnimation));
        //rigRoot.SetActive(false);
    }

}
