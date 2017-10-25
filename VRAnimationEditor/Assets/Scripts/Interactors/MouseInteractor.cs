using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for mouse pointer interactions.
// Raycasting logic is done the same as it would be for a VR pointer.
public class MouseInteractor : MonoBehaviour {
	// Can only ever have one mouse, so make it's pointer ID always be 0.
	private const int InteractorID = 0;

    // Object that the mouse is over (must have collider to be detected).
    private GameObject pointFocus;
    private GameObject grabFocus;
    private List<GameObject> grabCandidates, buttonAxisFocuses;
    private GameObject grabAnchor;
	
    void Start(){
        buttonAxisFocuses = new List<GameObject>();
        grabCandidates = new List<GameObject>();
        grabAnchor = new GameObject("Mouse Grab Anchor");
    }

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
            if (grabFocus == null)
            {
                grabAnchor.transform.position = hitInfo.point;
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
        if (Input.GetMouseButtonDown(0))
        {
            SendButtonToRecievers(0, true);
        }
        if (Input.GetMouseButtonDown(1))
        {
            SendButtonToRecievers(0, false);
        }

        if (Input.GetMouseButtonDown(1))
        {
            SendButtonToRecievers(1, true);
        }
        if (Input.GetMouseButtonDown(1))
        {
            SendButtonToRecievers(1, false);
        }
    }

    private void GrabUpdate(){
        if (Input.GetMouseButtonDown(0))
        {
            Grab();
        }
        if (Input.GetMouseButtonUp(0))
        {
            Release();
        }
    }

    // Change the current pointer focus.
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

            if (pointFocus.GetComponent<IButtonAxisReciever>() != null)
            {
                if (pointFocus != grabFocus)
                {
                    buttonAxisFocuses.Remove(pointFocus);
                }
            }
        }
        // Tell new focus the pointer is entering (if applicable).
        if (newFocus != null && newFocus.GetComponent<IPointerReciever>() != null)
        {
            newFocus.GetComponent<IPointerReciever>().OnPointerEnter(InteractorID);
            // Add to buttonAxis focus list if it has reciever and is not already in list.
            if (newFocus.GetComponent<IButtonAxisReciever>() != null)
            {
                if(!buttonAxisFocuses.Contains(newFocus))
                {
                    buttonAxisFocuses.Add(newFocus);
                }
            }
        }
        // Set the new focus.
        pointFocus = newFocus;
    }

    private void SendButtonToRecievers(int buttonID, bool buttonState){
        for (int i = 0; i < buttonAxisFocuses.Count; i++)
        {
            if (buttonAxisFocuses[i] == null)
            {
                buttonAxisFocuses.RemoveAt(i);
            }
            else
            {
                buttonAxisFocuses[i].GetComponent<IButtonAxisReciever>().OnRecieveButton(InteractorID, buttonID, buttonState);
            }
        }
    }

    private void SendAxisToRecievers(int axisID, float axisValue){
        for (int i = 0; i < buttonAxisFocuses.Count; i++)
        {
            if (buttonAxisFocuses[i] == null)
            {
                buttonAxisFocuses.RemoveAt(i);
            }
            else
            {
                buttonAxisFocuses[i].GetComponent<IButtonAxisReciever>().OnRecieveAxis(InteractorID, axisID, axisValue);
            }
        }
    }

    void OnTriggerEnter(Collider collider){
        if (collider.GetComponent<IGrabReciever>() != null)
        {
            grabCandidates.Add(collider.gameObject);
        }
    }

    void OnTriggerExit(Collider collider){
        grabCandidates.Remove(collider.gameObject);
    }

    private void Grab(){
        if (grabCandidates.Count > 0)
        {
            grabFocus = grabCandidates[grabCandidates.Count - 1];
            grabFocus.GetComponent<IGrabReciever>().OnGrab(gameObject);
            if (grabFocus.GetComponent<IButtonAxisReciever>() != null && !buttonAxisFocuses.Contains(grabFocus))
            {
                buttonAxisFocuses.Add(grabFocus);
            }
        }       
    }

    private void Release(){
        if (grabFocus != null)
        {
            grabFocus.GetComponent<IGrabReciever>().OnRelease(gameObject);
            if (grabFocus.GetComponent<IButtonAxisReciever>() != null && grabFocus != pointFocus)
            {
                buttonAxisFocuses.Remove(grabFocus);
            }
            grabFocus = null;
        }
    }
}
