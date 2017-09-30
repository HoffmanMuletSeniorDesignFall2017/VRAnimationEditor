using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for mouse pointer interactions.
// Raycasting logic is done the same as it would be for a VR pointer.
public class MousePointerController : MonoBehaviour {
	// Can only ever have one mouse, so make it's pointer ID always be 0.
	private const int PointerID = 0;

    // Object that the mouse is over (must have collider to be detected).
    private GameObject focus;
	
	void Update () {
        // Get ray from mouse position.
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Raycast to see if we are pointing at anything.
        RaycastHit hitInfo;
        if (Physics.Raycast(mouseRay, out hitInfo))
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
	        if (Input.GetMouseButtonDown(0))
	        {          
				pointerReciever.OnButtonDown(PointerID, 0);
            }
			if (Input.GetMouseButtonUp (0)) {
				pointerReciever.OnButtonUp (PointerID, 0);
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
			focus.GetComponent<IPointerReciever>().OnPointerExit(PointerID);
        }
        // Tell new focus the pointer is entering (if applicable).
        if (HasPointerReciever(newFocus))
        {
            newFocus.GetComponent<IPointerReciever>().OnPointerEnter(PointerID);
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
