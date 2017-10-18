using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableVisualizer : Visualizer, IPointerReciever {

	//If this component is added to a GameObject then that GameObject can be selected and moved by the user
	//NOTE: This script must be on an object with another actual visualizer already on it

	public bool constrainedToLocalX = false;

	bool shouldDelete = false;

	//public Visualizer associatedVisualizer;

	// Use this for initialization
	void Start () {
		if (gameObject.GetComponent<Collider> () == null) {
			gameObject.AddComponent<BoxCollider> ();
			GetComponent<BoxCollider> ().isTrigger = true;
		} else if (gameObject.GetComponent<Collider> ().enabled == false) {
			//GetComponent<Collider> ().isTrigger = true;
			GetComponent<Collider> ().enabled = true;
		}

		interactingPointers = new LinkedList<int>();
		pressingPointers = new LinkedList<int>();

		this.enabled = false;		//We don't need update so we disable this
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Select(){
		if(GetComponent<cakeslice.Outline>() != null)
			GetComponent<cakeslice.Outline>().enabled = true;
		selected = true;
		if(associatedVisualizer != null)
			associatedVisualizer.selected = true;
	}

	public void Deselect(){
		if(GetComponent<cakeslice.Outline>() != null)
			GetComponent<cakeslice.Outline>().enabled = false;
		selected = false;
		if(associatedVisualizer != null)
			associatedVisualizer.selected = false;
	}

	public void Grab(){
		grabbing = true;
		GetComponent<Collider> ().enabled = false;
		if(associatedVisualizer != null)
			associatedVisualizer.grabbing = true;
	}

	public void DeGrab(){
		grabbing = false;
		GetComponent<Collider> ().enabled = true;
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

	/*
	public void OnPointerExit(int pointerID)
	{
		interactingPointers.Remove(pointerID);
		pressingPointers.Remove(pointerID);
		pointerHoveringOverThis = false;
	}

	public void OnPointerEnter(int pointerID){
		pointerHoveringOverThis = true;
	}

	public void OnButtonDown(int pointerID, int buttonID)
	{
		if (buttonID == 0)
		{
			pressingPointers.AddFirst(pointerID);
			if (selected) {
				Grab ();
				return;
			}
			if (pointerHoveringOverThis) {
				Select ();
				return;
			}
			Deselect ();
			DeGrab ();
		}
	}

	public void OnButtonUp(int pointerID, int buttonID)
	{
		if (buttonID == 0)
		{
			pressingPointers.Remove(pointerID);

			if (grabbing) {
				DeGrab ();
			}
		}

	}
	*/


}
