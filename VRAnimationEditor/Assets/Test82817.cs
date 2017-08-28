using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test82817 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.KeypadEnter)) {
			AnimationEditorFunctions.CreateNewAnimation (string.Concat("Test", System.DateTime.Now.Minute.ToString()));
		}
	}
}
