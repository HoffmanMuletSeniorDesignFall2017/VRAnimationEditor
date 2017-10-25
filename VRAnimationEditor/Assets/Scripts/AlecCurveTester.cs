using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AlecCurveTester : MonoBehaviour {
    public AnimationClip clip;

	// Use this for initialization
	void Start () {
        PrintBindingInfo();
	}
	
    private void PrintBindingInfo(){
        string debug = "";
        EditorCurveBinding[] bindings = AnimationUtility.GetCurveBindings(clip);
        for (int i = 0; i < bindings.Length; i++)
        {
            debug += "binding " + i + ": path = " + bindings[i].path + ", property name = " + bindings[i].propertyName + "\r\n";
        }
        Debug.Log(debug);
    }
}
