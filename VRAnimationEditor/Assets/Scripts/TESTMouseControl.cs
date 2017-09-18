using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTMouseControl : MonoBehaviour {

	private bool inTheThing = false;
	private float distance = 0f;
	Transform thingThatWeClicked;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			//Debug.Log ("Clicked!");
			RaycastHit hitInfo;

			Debug.DrawRay (Camera.main.ScreenPointToRay(Input.mousePosition).GetPoint(0f), Camera.main.ScreenPointToRay (Input.mousePosition).direction*1500f);


			if (Physics.Raycast (Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, 1500f)) {

				//Debug.Log ("Hit something!");

				if (hitInfo.transform.GetComponent<MovableVisualizer> () != null) {
					//Debug.Log ("About to select!");
					hitInfo.transform.GetComponent<MovableVisualizer> ().Select ();
					inTheThing = true;
					thingThatWeClicked = hitInfo.transform;
					distance = hitInfo.distance;
				}
			}
		}

		if (inTheThing) {
			//Dragging
			thingThatWeClicked.GetComponent<MovableVisualizer>().Move(Camera.main.ScreenPointToRay(Input.mousePosition).GetPoint(distance));
		}

		if (Input.GetMouseButtonUp (0) && inTheThing) {
			inTheThing = false;
			thingThatWeClicked.GetComponent<MovableVisualizer> ().Deselect ();
		}
	}
}
