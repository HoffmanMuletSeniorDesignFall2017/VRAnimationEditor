using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRControllerInteractor : MonoBehaviour {
    private static int vrControllerCount = 0;

    public bool isLeftHand = true;
    public GameObject lazerLine;
    public float pointerMaxDistance = 1000f;
    public float axisDeadzone = 0.1f;

    private int interactorID = 0;
    private GameObject pointFocus;
    private GameObject grabFocus;
    private GameObject grabCandidate;
    private List<GameObject> buttonAxisFocuses;

	void Start () {
        interactorID = vrControllerCount;
        vrControllerCount++;

        buttonAxisFocuses = new List<GameObject>();
	}
	
    void Update () {
        PointerUpdate();
        ButtonAxisUpdate();
    }

    private void PointerUpdate()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        // Raycast to see if we are pointing at anything.
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, pointerMaxDistance))
        {
            // Set lazer length to stop at hit.
            float hitDistance = (hitInfo.point - transform.position).magnitude;
            lazerLine.transform.localScale = new Vector3 (1, 1, hitDistance);

            // If we are pointing at something different than last frame's focus...
            if (hitInfo.collider.gameObject != pointFocus)
            {
                // Change our focus to the new object.
                ChangePointFocus(hitInfo.collider.gameObject);
            }
        }
        // If we are not pointing at anything...
        else
        {
            // Set lazer length go to max distance.
            lazerLine.transform.localScale = new Vector3(1, 1, pointerMaxDistance);

            // Make sure the focus is null.
            if (pointFocus != null)
            {
                ChangePointFocus(null);
            }
        }
    }

    private void GrabUpdate(){
        if (isLeftHand)
        {
            if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
            {
                if (grabCandidate != null)
                {
                    grabFocus = grabCandidate;
                    grabFocus.GetComponent<IGrabReciever>().OnGrab(gameObject);
                    if (!buttonAxisFocuses.Contains(grabFocus))
                    {
                        buttonAxisFocuses.Add(grabFocus);
                    }
                }
            }
        }
    }




    private void ButtonAxisUpdate(){
        if (isLeftHand)
        {
            // Buttons.
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
                SendButtonToRecievers(0, true);
            if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
                SendButtonToRecievers(0, false);
            if (OVRInput.GetDown(OVRInput.Button.Three))
                SendButtonToRecievers(1, true);
            if (OVRInput.GetUp(OVRInput.Button.Three))
                SendButtonToRecievers(1, false);
            if (OVRInput.GetDown(OVRInput.Button.Four))
                SendButtonToRecievers(2, true);
            if (OVRInput.GetUp(OVRInput.Button.Four))
                SendButtonToRecievers(2, false);

            // Axes.
            if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).magnitude > axisDeadzone)
            {
                SendAxisToRecievers(0, OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x);
                SendAxisToRecievers(1, OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y);
            }
        }
        // Right hand.
        else
        {
            // Buttons.
            if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
                SendButtonToRecievers(0, true);
            if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
                SendButtonToRecievers(0, false);
            if (OVRInput.GetDown(OVRInput.Button.One))
                SendButtonToRecievers(1, true);
            if (OVRInput.GetUp(OVRInput.Button.One))
                SendButtonToRecievers(1, false);
            if (OVRInput.GetDown(OVRInput.Button.Two))
                SendButtonToRecievers(2, true);
            if (OVRInput.GetUp(OVRInput.Button.Two))
                SendButtonToRecievers(2, false);

            // Axes.
            if (OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).magnitude > axisDeadzone)
            {
                SendAxisToRecievers(0, OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).x);
                SendAxisToRecievers(1, OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y);
            }
        }
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
                buttonAxisFocuses[i].GetComponent<IButtonAxisReciever>().OnRecieveButton(interactorID, buttonID, buttonState);
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
                buttonAxisFocuses[i].GetComponent<IButtonAxisReciever>().OnRecieveAxis(interactorID, axisID, axisValue);
            }
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
            pointFocus.GetComponent<IPointerReciever>().OnPointerExit(interactorID);
            // Remove buttonAxis reciever if old pointer focus has one and it is not shared with the grab focus.
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
            newFocus.GetComponent<IPointerReciever>().OnPointerEnter(interactorID);
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

    void OnTriggerEnter(Collider collider){
        if (collider.GetComponent<IGrabReciever>() != null)
        {
            grabCandidate = collider.gameObject;
        }
    }

    void OnTriggerExit(Collider collider){
        if (grabCandidate == collider.gameObject)
        {
            grabCandidate = null;
        }
    }
}
