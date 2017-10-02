using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueVisualizer : Visualizer {

	private float currentValue = 0f;

	// Use this for initialization
	void Start () {
		gameObject.AddComponent<TextMesh> ();
		gameObject.GetComponent<TextMesh> ().characterSize = .18f;
		gameObject.GetComponent<TextMesh> ().fontSize = 77;

		gameObject.AddComponent<BoxCollider> ();
		gameObject.GetComponent<BoxCollider> ().size = new Vector3 (2.7f, 1.3f, 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateText(string newText){
		if(gameObject.GetComponent<TextMesh>() != null)
			gameObject.GetComponent<TextMesh> ().text = newText;
	}

	public void UpdateText(float newFloat){
		if(gameObject.GetComponent<TextMesh>() != null)
			gameObject.GetComponent<TextMesh> ().text = newFloat.ToString();

		currentValue = newFloat;
	}

	public void IncrementValue(){
		((AnimationCurveVisualizer)associatedVisualizer).EditSelectedKeyframeValue (currentValue + 1);
	}

	public void DecrementValue(){
		((AnimationCurveVisualizer)associatedVisualizer).EditSelectedKeyframeValue (currentValue - 1);
	}
}
