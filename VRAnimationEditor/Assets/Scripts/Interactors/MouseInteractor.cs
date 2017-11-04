using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for mouse pointer interactions.
// Raycasting logic is done the same as it would be for a VR pointer.
public class MouseInteractor : MonoBehaviour {
	// Can only ever have one mouse, so make it's pointer ID always be 0.
	private const int InteractorID = 0;

    // Adjust the speed at which grabbed objects are moved.
    public float dragSpeed = 1f;

    // Object that the mouse is over (must have collider to be detected).
    private GameObject pointFocus, grabFocus;

    void Update(){
        PointUpdate();
        GrabUpdate();
        ButtonAxisUpdate();
    }

	void PointUpdate () {
        // Get ray from mouse position.
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Raycast to see if we are pointing at anything.
        RaycastHit hitInfo;
        if (Physics.Raycast(mouseRay, out hitInfo))
        {
            // If we are pointing at something different than last frame's focus...
            if (hitInfo.collider.gameObject != pointFocus)
            {
                // Change our focus to the new object.
                ChangePointFocus(hitInfo.collider.gameObject);
            }
            // If we are not grabbing something, move the mouse interactor the pointed position.
            if(grabFocus == null){
                transform.position = hitInfo.point;
            }
        }
        // If we are not pointing at anything...
        else
        {
            // Make sure the focus is null.
            if (pointFocus != null)
            {
                ChangePointFocus(null);
            }
        }
	}

    private void ButtonAxisUpdate(){
        // Script that will recieve input.
        IButtonAxisReciever reciever;
        // Since grabbed objects don't stay under the mouse, prioritize grab focus for input first.
        if (grabFocus != null && grabFocus.GetComponent<IButtonAxisReciever>() != null)
        {
            reciever = grabFocus.GetComponent<IButtonAxisReciever>();
        }
        // If there are no button / axis recievers on the grab focus, try the point focus.
        else if(pointFocus != null && pointFocus.GetComponent<IButtonAxisReciever>() != null)
        {
            reciever = pointFocus.GetComponent<IButtonAxisReciever>();
        }
        // If there is nothing there either, we have no script to update so just return.
        else{
            return;
        }
        // Notify script of the following events.
        if (Input.GetMouseButtonDown(0))
        {
            reciever.OnRecieveButton(InteractorID, 0, true);
        }
        if (Input.GetMouseButtonDown(0))
        {
            reciever.OnRecieveButton(InteractorID, 0, false);
        }

        if (Input.GetMouseButtonDown(1))
        {
            reciever.OnRecieveButton(InteractorID, 1, true);
        }
        if (Input.GetMouseButtonDown(1))
        {
            reciever.OnRecieveButton(InteractorID, 1, false);
        }
    }

    private void GrabUpdate(){
        // Check grab / release.
        if (Input.GetMouseButtonDown(0))
        {
            Grab();
        }
        if (Input.GetMouseButtonUp(0))
        {
            Release();
        }
        // While an object is grabbed, use mouse movement to move the interactor position (which the grabbed object will have parented itself to
        // if it wants to be moved).
        if (grabFocus != null)
        {
            float grabFocusDistance = (Camera.main.transform.position - transform.position).magnitude;
            Vector3 hrzMove = Camera.main.transform.right * Input.GetAxisRaw("Mouse X") * dragSpeed * Time.deltaTime * grabFocusDistance;
            Vector3 vrtMove = Camera.main.transform.up * Input.GetAxisRaw("Mouse Y") * dragSpeed * Time.deltaTime * grabFocusDistance;
            transform.position += hrzMove + vrtMove;
        }
    }

    private void ChangePointFocus(GameObject newFocus){
        // Make sure the focus is actually changing.
        if (pointFocus == newFocus)
        {
            return;
        }
        // Tell old focus the pointer is exiting (if applicable).
        if (pointFocus != null && pointFocus.GetComponent<IPointerReciever>() != null)
        {
            pointFocus.GetComponent<IPointerReciever>().OnPointerExit(InteractorID);
        }
        // Tell new focus the pointer is entering (if applicable).
        if (newFocus != null && newFocus.GetComponent<IPointerReciever>() != null)
        {
            newFocus.GetComponent<IPointerReciever>().OnPointerEnter(InteractorID);
        }
        // Set the new focus.
        pointFocus = newFocus;
    }


    private void Grab(){
        if (pointFocus != null && pointFocus.GetComponent<IGrabReciever>() != null)
        {
            grabFocus = pointFocus;
            grabFocus.GetComponent<IGrabReciever>().OnGrab(gameObject);
        }       
    }

    private void Release(){
        if (grabFocus != null)
        {
            grabFocus.GetComponent<IGrabReciever>().OnRelease(gameObject);
            grabFocus = null;
        }      
    }
}
