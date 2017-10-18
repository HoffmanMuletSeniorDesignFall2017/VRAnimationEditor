using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SessionManager : MonoBehaviour {
    public GameObject modelSelectionUIPrefab;
    public GameObject animClipSelectionUIPrefab;
    public GameObject sessionModel;
    public AnimationClip sessionAnim;
    public AnimationVisualizer animVis;
	public Transform animModelAnchor;

	public NodeVisualizationManager templateNodeVisualizer;

    void Start(){
        StartNewSession();
    }

    public void StartNewSession(){
        sessionModel = null;
        sessionAnim = null;

        GameObject modelSelUI = Instantiate<GameObject>(modelSelectionUIPrefab);
        modelSelUI.GetComponent<ModelSelectionUIController>().sessionManager = this;
    }

    public void OnModelSelected(){
        GameObject animSelUI = Instantiate<GameObject>(animClipSelectionUIPrefab);
        animSelUI.GetComponent<AnimationSelectionUIController>().sessionManager = this;
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

		newAnimation.SetCurve("", typeof(Transform), "localPosition.y", testCurve);
		//sessionAnim.SetCurve("", typeof(Transform), "localScale.y", testCurve);

		testCurve = new AnimationCurve();
		key1 = new Keyframe();
		key2 = new Keyframe();

		key1.time = 0f;
		key1.value = 3f;

		key2.time = 4f;
		key2.value = -5f;

		testCurve.AddKey (key1);
		testCurve.AddKey (key2);

		newAnimation.SetCurve("", typeof(Transform), "localPosition.x", testCurve);
		//sessionAnim.SetCurve("", typeof(Transform), "localScale.x", testCurve);

		//===============================================================================================================================*/
		//newAnimation = sessionAnim;
		//deep copy
		for (int i = 0; i < AnimationUtility.GetCurveBindings (sessionAnim).Length; i++) {
			newAnimation.SetCurve (AnimationUtility.GetCurveBindings (sessionAnim) [i].path, AnimationUtility.GetCurveBindings (sessionAnim) [i].type, AnimationUtility.GetCurveBindings (sessionAnim) [i].propertyName, AnimationUtility.GetEditorCurve (sessionAnim, AnimationUtility.GetCurveBindings (sessionAnim) [i]));
		}

        GameObject objInstance =  AnimationEditorFunctions.InstantiateWithAnimation(sessionModel, newAnimation);

		//TODO: May have to change this, since we might not want the actual model a child of the animation visualizer.
		/* GameObject newThing = new GameObject();
		 * 
		newThing.transform.localPosition.Set (objInstance.transform.localPosition.x, objInstance.transform.localPosition.y, objInstance.transform.localPosition.z);

		//objInstance.transform.parent = newThing.transform;
		objInstance.transform.parent = animModelAnchor;

		objInstance.transform.localPosition.Set (0f, 0f, 0f);

		newThing.transform.parent = animVis.transform;
		newThing.transform.localPosition = new Vector3 (0f, 0f, 0f);
*/
		//-------End TODO


		NodeVisualizationManager nodeVis = objInstance.AddComponent<NodeVisualizationManager> ();
		nodeVis.nodeMarkerPrefab = templateNodeVisualizer.nodeMarkerPrefab;
		nodeVis.makeTransparent = templateNodeVisualizer.makeTransparent;
		nodeVis.transparentTemplate = templateNodeVisualizer.transparentTemplate;


		StartCoroutine(WaitAndDoTheThing (objInstance, newAnimation));
		//animVis.SetCurrentClipAndGameObject(newAnimation, objInstance);
    }

	IEnumerator WaitAndDoTheThing(GameObject objInstance, AnimationClip sessionAnim){
		yield return new WaitForFixedUpdate ();
		yield return null;

		animVis.SetCurrentClipAndGameObject (sessionAnim, objInstance);
	}
}
