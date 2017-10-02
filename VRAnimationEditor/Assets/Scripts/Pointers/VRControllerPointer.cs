using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRControllerPointer : MonoBehaviour {
	private static int vrPointerCount = 0;

	public bool isLeft = true;

	private int pointerId = 0;
	private GameObject focus;

	void Start(){
		pointerId = vrPointerCount;
		vrPointerCount++;
	}

	void Update () {
		Ray ray = new Ray(transform.position, transform.forward);
		// Raycast to see if we are pointing at anything.
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo))
		{
			// If we are pointing at something different than last frame's focus...
			if (hitInfo.collider.gameObject != focus)
			{
				// Change our focus to the new object.
				ChangeFocus(hitInfo.collider.gameObject);
			}
		}
		// If we are not pointing at anything...
		else
		{
			// Make sure the focus is null.
			if (focus != null)
			{
				ChangeFocus(null);
			}
		}
		// If focus has a pointer receiver, check mouse buttons and send them to focus as needed.
		if (HasPointerReciever(focus))
		{
			IPointerReciever pointerReciever = focus.GetComponent<IPointerReciever> ();
			if (isLeft) {
				if (OVRInput.GetDown (OVRInput.Button.PrimaryIndexTrigger)) {
					pointerReciever.OnButtonDown (pointerId, 0);
				}

				if (OVRInput.GetUp (OVRInput.Button.PrimaryIndexTrigger)) {
					pointerReciever.OnButtonUp (pointerId, 0);
				}
			} else {
				if (OVRInput.GetDown (OVRInput.Button.SecondaryIndexTrigger)) {
					pointerReciever.OnButtonDown (pointerId, 0);
				}

				if (OVRInput.GetUp (OVRInput.Button.SecondaryIndexTrigger)) {
					pointerReciever.OnButtonUp (pointerId, 0);
				}
			}
		}
	}

	// Change the current pointer focus.
	private void ChangeFocus(GameObject newFocus){
		// Make sure the focus is actually changing.
		if (focus == newFocus)
		{
			return;
		}
		// Tell old focus the pointer is exiting (if applicable).
		if (HasPointerReciever(focus))
		{
			focus.GetComponent<IPointerReciever>().OnPointerExit(pointerId);
		}
		// Tell new focus the pointer is entering (if applicable).
		if (HasPointerReciever(newFocus))
		{
			newFocus.GetComponent<IPointerReciever>().OnPointerEnter(pointerId);
		}
		// Set the new focus.
		focus = newFocus;
	}

	// Check if the focus has a pointer reciever component.
	private bool HasPointerReciever(GameObject obj){
		if (obj == null)
		{
			return false;
		}
		return obj.GetComponent<IPointerReciever>() != null;
	}
}
