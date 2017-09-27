using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTMouseControl : MonoBehaviour {

	private bool grabbing = false;
	private float distance = 0f;
	Transform thingThatWeClicked;

	public AnimationVisualizer animVis;

	bool haveSelection = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			//Debug.Log ("Clicked!");
			RaycastHit hitInfo;

			Debug.DrawRay (Camera.main.ScreenPointToRay(Input.mousePosition).GetPoint(0f), Camera.main.ScreenPointToRay (Input.mousePosition).direction*1500f);


			if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hitInfo, 1500f)) {

				//Debug.Log ("Hit something!");

				if (hitInfo.transform.GetComponent<MovableVisualizer> () != null) {
					
					//Debug.Log ("About to select!");
					if (thingThatWeClicked != null) {
						if (thingThatWeClicked != hitInfo.transform) {
							thingThatWeClicked.GetComponent<MovableVisualizer> ().DeGrab ();
							thingThatWeClicked.GetComponent<MovableVisualizer> ().Deselect ();
							haveSelection = false;
						}
					}

					hitInfo.transform.GetComponent<MovableVisualizer> ().Select ();

					thingThatWeClicked = hitInfo.transform;
					distance = hitInfo.distance;
					grabbing = false;

					if (haveSelection == true) {
						hitInfo.transform.GetComponent<MovableVisualizer> ().Grab ();
						grabbing = true;	//We are grabbing
					}

					haveSelection = true;
				} else if (hitInfo.transform.GetComponent<ValueVisualizer> () != null) {
					hitInfo.transform.GetComponent<ValueVisualizer> ().IncrementValue ();
				}
				else {
					haveSelection = false;
					if (thingThatWeClicked.gameObject.GetComponent<MovableVisualizer> () != null) {
						thingThatWeClicked.gameObject.GetComponent<MovableVisualizer> ().DeGrab ();
						thingThatWeClicked.gameObject.GetComponent<MovableVisualizer> ().Deselect ();
					}
				}
			} else {
				haveSelection = false;
				if (thingThatWeClicked != null) {
					
					if (thingThatWeClicked.gameObject.GetComponent<MovableVisualizer> () != null) {
						thingThatWeClicked.gameObject.GetComponent<MovableVisualizer> ().DeGrab ();
						thingThatWeClicked.gameObject.GetComponent<MovableVisualizer> ().Deselect ();
					}
				}
			}
		}

		if (grabbing) {
			//Dragging
			if (thingThatWeClicked != null) {
				thingThatWeClicked.GetComponent<MovableVisualizer> ().Move (Camera.main.ScreenPointToRay (Input.mousePosition).GetPoint (distance));
			}
		}

		if (Input.GetMouseButtonUp (0) && grabbing) {
			grabbing = false;
			if(thingThatWeClicked != null)
				thingThatWeClicked.GetComponent<MovableVisualizer> ().DeGrab ();
		}

		if (Input.GetKeyDown (KeyCode.Keypad7) && haveSelection) {
			if (thingThatWeClicked != null) {
				thingThatWeClicked.GetComponent<MovableVisualizer> ().Delete ();
			}
		}

		if (Input.GetKeyDown (KeyCode.Keypad8)) {
			animVis.GetLastAnimCurveVisualizer ().AddKeyframe ();
		}

		if (Input.GetMouseButtonDown (1)) {
			RaycastHit hitInfo;
		
			if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hitInfo, 1500f)) {

				if (hitInfo.transform.GetComponent<ValueVisualizer> () != null) {
					hitInfo.transform.GetComponent<ValueVisualizer> ().DecrementValue ();
				}
				
			} 
		}

	}
}
