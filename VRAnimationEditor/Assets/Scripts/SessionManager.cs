using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionManager : MonoBehaviour {
    public GameObject modelSelectionUIPrefab;
    public GameObject animClipSelectionUIPrefab;
    public AssetLogger assetLogger;
    public GameObject sessionModel;
    public AnimationClip sessionAnim;
    public AnimationVisualizer animVis;

	public NodeVisualizer templateNodeVisualizer;

    void Start(){
        StartNewSession();
    }

    public void StartNewSession(){
        sessionModel = null;
        sessionAnim = null;

        GameObject modelSelUI = Instantiate<GameObject>(modelSelectionUIPrefab);
        modelSelUI.GetComponent<ModelSelectionUIController>().sessionManager = this;
        modelSelUI.GetComponent<ModelSelectionUIController>().assetLogger = assetLogger;
    }

    public void OnModelSelected(){
        GameObject animSelUI = Instantiate<GameObject>(animClipSelectionUIPrefab);
        animSelUI.GetComponent<AnimationSelectionUIController>().sessionManager = this;
        animSelUI.GetComponent<AnimationSelectionUIController>().assetLogger = assetLogger;
    }

    public void OnAnimationSelected(){
        GameObject objInstance =  AnimationEditorFunctions.InstantiateWithAnimation(sessionModel, sessionAnim);
        animVis.SetCurrentClipAndGameObject(sessionAnim, objInstance);

		NodeVisualizer nodeVis = objInstance.AddComponent<NodeVisualizer> ();
		nodeVis.nodeMarkerPrefab = templateNodeVisualizer.nodeMarkerPrefab;
		nodeVis.makeTransparent = templateNodeVisualizer.makeTransparent;
		nodeVis.transparentTemplate = templateNodeVisualizer.transparentTemplate;
    }
}
