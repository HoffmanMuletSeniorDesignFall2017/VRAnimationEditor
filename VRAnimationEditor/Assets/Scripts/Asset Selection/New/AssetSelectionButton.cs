using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class AssetSelectionButton : MonoBehaviour {

    public AnimationSelectionPanel animSelPanel;
    public ModelSelectionPanel modelSelPanel;
    public Button button;
    public Transform modelSpawnAnchor;
    public AnimationVisualizer animVis;
    public NodeVisualizationManager templateNodeVisualizationManager;
    public GameObject rigRoot;

    void Update () {
        button.interactable = animSelPanel.selectedTile != null && modelSelPanel.selectedTile != null;
    }

    public void OnClick()
    {
        AnimationClip animClip = animSelPanel.selectedTile.anim;
        GameObject model = modelSelPanel.selectedTile.model;
        AnimationClip newAnimation = AnimationEditorFunctions.CreateNewAnimation(animClip.name);

        for (int i = 0; i < AnimationUtility.GetCurveBindings(animClip).Length; i++)
        {
            newAnimation.SetCurve(AnimationUtility.GetCurveBindings(animClip)[i].path, AnimationUtility.GetCurveBindings(animClip)[i].type, AnimationUtility.GetCurveBindings(animClip)[i].propertyName, AnimationUtility.GetEditorCurve(animClip, AnimationUtility.GetCurveBindings(animClip)[i]));
        }

        if (animClip != null)
        {
            for (int i = 0; i < AnimationUtility.GetCurveBindings(animClip).Length; i++)
            {
                newAnimation.SetCurve(AnimationUtility.GetCurveBindings(animClip)[i].path, AnimationUtility.GetCurveBindings(animClip)[i].type, AnimationUtility.GetCurveBindings(animClip)[i].propertyName, AnimationUtility.GetEditorCurve(animClip, AnimationUtility.GetCurveBindings(animClip)[i]));
            }
        }

        GameObject animModel = AnimationEditorFunctions.InstantiateWithAnimation(model, newAnimation, modelSpawnAnchor);
        SetupNodeVisualization(animModel);

        StartCoroutine(WaitAndDoTheThing(animModel, newAnimation));
        //rigRoot.SetActive(false);
    }

    protected void SetupNodeVisualization(GameObject modelObj)
    {
        NodeVisualizationManager nodeVis = modelObj.AddComponent<NodeVisualizationManager>();
        nodeVis.nodeMarkerPrefab = templateNodeVisualizationManager.nodeMarkerPrefab;
        nodeVis.makeTransparent = templateNodeVisualizationManager.makeTransparent;
        nodeVis.transparentTemplate = templateNodeVisualizationManager.transparentTemplate;
    }

    protected IEnumerator WaitAndDoTheThing(GameObject objInstance, AnimationClip sessionAnim)
    {
        yield return new WaitForFixedUpdate();
        yield return null;

        animVis.gameObject.SetActive(true);

        animVis.SetCurrentClipAndGameObject(sessionAnim, objInstance);

        yield return null;

        rigRoot.SetActive(false);
    }

}
