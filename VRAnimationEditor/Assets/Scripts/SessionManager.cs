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
		//sessionAnim = AnimationEditorFunctions.CopyToNewAnimation (sessionAnim, "new");
		AnimationClip newAnimation = AnimationEditorFunctions.CreateNewAnimation ("Test");
		/*//===============================================================================================================================
		AnimationCurve testCurve = new AnimationCurve();
		Keyframe key1 = new Keyframe();
		Keyframe key2 = new Keyframe();

		key1.time = 0f;
		key1.value = 2;

		key2.time = 2f;
		key2.value = 1;

		testCurve.AddKey (key1);
		testCurve.AddKey (key2);

		//animClip.SetCurve("", typeof(Transform), "localPosition.y", testCurve);
		sessionAnim.SetCurve("", typeof(Transform), "localPosition.y", testCurve);
		sessionAnim.SetCurve("", typeof(Transform), "localScale.y", testCurve);

		testCurve = new AnimationCurve();
		key1 = new Keyframe();
		key2 = new Keyframe();

		key1.time = 0f;
		key1.value = 3f;

		key2.time = 4f;
		key2.value = -5f;

		testCurve.AddKey (key1);
		testCurve.AddKey (key2);

		sessionAnim.SetCurve("", typeof(Transform), "localPosition.x", testCurve);
		sessionAnim.SetCurve("", typeof(Transform), "localScale.x", testCurve);

		//===============================================================================================================================*/
		newAnimation = sessionAnim;

        GameObject objInstance =  AnimationEditorFunctions.InstantiateWithAnimation(sessionModel, newAnimation);

		//TODO: May have to change this, since we might not want the actual model a child of the animation visualizer.
		/* GameObject newThing = new GameObject();
		newThing.transform.localPosition.Set (objInstance.transform.localPosition.x, objInstance.transform.localPosition.y, objInstance.transform.localPosition.z);

		objInstance.transform.parent = newThing.transform;

		objInstance.transform.localPosition.Set (0f, 0f, 0f);

		newThing.transform.parent = animVis.transform;
		newThing.transform.localPosition = new Vector3 (0f, 0f, 0f);
*/
		//-------End TODO

		/*
		NodeVisualizer nodeVis = objInstance.AddComponent<NodeVisualizer> ();
		nodeVis.nodeMarkerPrefab = templateNodeVisualizer.nodeMarkerPrefab;
		nodeVis.makeTransparent = templateNodeVisualizer.makeTransparent;
		nodeVis.transparentTemplate = templateNodeVisualizer.transparentTemplate;
		*/

		//StartCoroutine(WaitAndDoTheThing (objInstance));
		animVis.SetCurrentClipAndGameObject(sessionAnim, objInstance);
    }

	IEnumerator WaitAndDoTheThing(GameObject objInstance){
		yield return new WaitForFixedUpdate ();
		yield return null;

		animVis.SetCurrentClipAndGameObject (sessionAnim, objInstance);
	}
}
