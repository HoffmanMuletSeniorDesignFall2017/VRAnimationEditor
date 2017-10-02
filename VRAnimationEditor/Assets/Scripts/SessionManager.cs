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

		//TODO: May have to change this, since we might not want the actual model a child of the animation visualizer.
		GameObject newThing = new GameObject();
		newThing.transform.localPosition.Set (objInstance.transform.localPosition.x, objInstance.transform.localPosition.y, objInstance.transform.localPosition.z);

		objInstance.transform.parent = newThing.transform;

		objInstance.transform.localPosition.Set (0f, 0f, 0f);

		newThing.transform.parent = animVis.transform;
		newThing.transform.localPosition = new Vector3 (0f, 0f, 0f);

		//-------End TODO

        animVis.SetCurrentClipAndGameObject(sessionAnim, objInstance);

		NodeVisualizer nodeVis = objInstance.AddComponent<NodeVisualizer> ();
		nodeVis.nodeMarkerPrefab = templateNodeVisualizer.nodeMarkerPrefab;
		nodeVis.makeTransparent = templateNodeVisualizer.makeTransparent;
		nodeVis.transparentTemplate = templateNodeVisualizer.transparentTemplate;
    }
}
