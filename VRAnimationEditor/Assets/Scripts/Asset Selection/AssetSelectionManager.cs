using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AssetSelectionManager : MonoBehaviour, IAssetRequester {
    public IAssetSelector assetSelector;
    public Transform modelSpawnAnchor;
	public AnimationVisualizer animVis;
	public NodeVisualizationManager templateNodeVisualizationManager;
    public bool selectModelFirst = true;
    public bool poseDebug = false;

    private GameObject model;
    private AnimationClip animClip;

    void Start(){
        StartNewSelection();
    }

	// Select a new animation - model pair.
    public void StartNewSelection(){
        model = null;
        animClip = null;
        if (selectModelFirst)
        {
            assetSelector.RequestSelectedModel(this);
        }
        else
        {
            assetSelector.RequestSelectedAnimationClip(this);
        }
    }

	// Called by assetSelector when a model has been selected.
    public void SetModel(GameObject model){
        this.model = model;
		Debug.Log ("Model set to " + model.name);
		// If we still need to get the animation, request it.
        if (selectModelFirst)
        {
            assetSelector.RequestSelectedAnimationClip(this);
        }
		// If should have already gotten the animation...
        else
        {
			// Verify we have an animation then setup the editing UI.
            if (animClip == null)
            {
                throw new UnityException("Animation clip not set.");
            }
            else
            {
                SetupAnimation();
            }
        }
    }


    public void SetAnimationClip(AnimationClip animClip){
        this.animClip = animClip;
		Debug.Log ("Animation clip set to " + animClip.name);
        if (selectModelFirst)
        {
            if (model == null)
            {
                throw new UnityException("Model not set.");
            }
            else
            {
                SetupAnimation();
            }
        }
        else
        {
            assetSelector.RequestSelectedModel(this);
        }
    }

    private void PoseDebugSetup(){
        GameObject obj = Instantiate(model, modelSpawnAnchor.position, modelSpawnAnchor.rotation);
        SetupNodeVisualization(obj);
    }


    private void SetupAnimation(){
        if (poseDebug)
        {
            PoseDebugSetup();
            return;
        }

		AnimationClip newAnimation = AnimationEditorFunctions.CreateNewAnimation ("Test");
        if (animClip != null)
        {
            for (int i = 0; i < AnimationUtility.GetCurveBindings (animClip).Length; i++) {
                newAnimation.SetCurve (AnimationUtility.GetCurveBindings (animClip) [i].path, AnimationUtility.GetCurveBindings (animClip) [i].type, AnimationUtility.GetCurveBindings (animClip) [i].propertyName, AnimationUtility.GetEditorCurve (animClip, AnimationUtility.GetCurveBindings (animClip) [i]));
            }
        }
		
        GameObject animModel = AnimationEditorFunctions.InstantiateWithAnimation(model, newAnimation, modelSpawnAnchor);
        SetupNodeVisualization(animModel);

		StartCoroutine(WaitAndDoTheThing (animModel, newAnimation));

    }

    private void SetupNodeVisualization(GameObject modelObj){
        NodeVisualizationManager nodeVis = modelObj.AddComponent<NodeVisualizationManager> ();
        nodeVis.nodeMarkerPrefab = templateNodeVisualizationManager.nodeMarkerPrefab;
        nodeVis.makeTransparent = templateNodeVisualizationManager.makeTransparent;
        nodeVis.transparentTemplate = templateNodeVisualizationManager.transparentTemplate;
    }

	IEnumerator WaitAndDoTheThing(GameObject objInstance, AnimationClip sessionAnim){
		yield return new WaitForFixedUpdate ();
		yield return null;

		animVis.SetCurrentClipAndGameObject (sessionAnim, objInstance);
	}

    private void PrintDebugAnimationInfo(){
        EditorCurveBinding[] curveBindings = AnimationUtility.GetObjectReferenceCurveBindings(animClip);
        string debug = "";
        for (int i = 0; i < curveBindings.Length; i++)
        {
            debug += curveBindings[i].propertyName + "\r\n";
        }
        Debug.Log("Curve bindings:\r\n" + debug);
    }
}
