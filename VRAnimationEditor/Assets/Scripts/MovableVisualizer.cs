using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableVisualizer : Visualizer {

	//If this component is added to a GameObject then that GameObject can be selected and moved by the user
	//NOTE: This script must be on an object with another actual visualizer already on it

	public bool constrainedToLocalX = false;

	bool shouldDelete = false;

	public Visualizer associatedVisualizer;

	// Use this for initialization
	void Start () {
		if (gameObject.GetComponent<Collider> () == null) {
			gameObject.AddComponent<BoxCollider> ();
			GetComponent<BoxCollider> ().isTrigger = true;
		} else if (gameObject.GetComponent<Collider> ().enabled == false) {
			GetComponent<Collider> ().isTrigger = true;
			GetComponent<Collider> ().enabled = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Select(){
		selected = true;
		if(associatedVisualizer != null)
			associatedVisualizer.selected = true;
	}

	public void Deselect(){
		selected = false;
		if(associatedVisualizer != null)
			associatedVisualizer.selected = false;
	}

	public void Grab(){
		grabbing = true;
		if(associatedVisualizer != null)
			associatedVisualizer.grabbing = true;
	}

	public void DeGrab(){
		grabbing = false;
		if(associatedVisualizer != null)
			associatedVisualizer.grabbing = false;
	}

	public void Delete(){
		shouldDelete = true;
		if(associatedVisualizer != null)
			associatedVisualizer.childNeedsDeletion = true;
	}

	public void Move(Vector3 newPosition){
		if (constrainedToLocalX) {
			transform.position = new Vector3 (newPosition.x, transform.position.y, transform.position.z);
		}
	}
}
