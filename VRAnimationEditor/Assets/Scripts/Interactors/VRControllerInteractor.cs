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
    private List<GameObject> grabCandidates;
    private List<GameObject> buttonAxisFocuses;

	void Start () {
        interactorID = vrControllerCount;
        vrControllerCount++;

        buttonAxisFocuses = new List<GameObject>();
		grabCandidates = new List<GameObject> ();
	}
	
    void Update () {
        PointerUpdate();
        ButtonAxisUpdate();
        GrabUpdate();
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
            if (OVRInput.GetDown(OVRInput.RawButton.LHandTrigger))
            {
                Grab();
            }
            if (OVRInput.GetUp(OVRInput.RawButton.LHandTrigger))
            {
                Release();
            }
        }
        else
        {
            if (OVRInput.GetDown(OVRInput.RawButton.RHandTrigger))
            {
                Grab();
            }
            if (OVRInput.GetUp(OVRInput.RawButton.RHandTrigger))
            {
                Release();
            }        
        }
    }

	private GameObject GetClosestGrabCandidate(){
		if (grabCandidates.Count == 0)
			return null;
		int minIndex = -1;
		float minDistance = float.MaxValue;
		for (int i = 0; i < grabCandidates.Count; i++)
		{
			float distance = (grabCandidates[i].transform.position - transform.position).magnitude;
			if (distance < minDistance)
			{
				minIndex = i;
				minDistance = distance;
			}
		}
		return grabCandidates[minIndex];
	}

    private void Grab(){
		if (grabCandidates.Count > 0)
        {
			grabFocus = GetClosestGrabCandidate();
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

    private void ButtonAxisUpdate(){
        if (isLeftHand)
        {
            // Buttons.
            if (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger))
                SendButtonToRecievers(0, true);
            if (OVRInput.GetUp(OVRInput.RawButton.LIndexTrigger))
                SendButtonToRecievers(0, false);
            if (OVRInput.GetDown(OVRInput.RawButton.A))
                SendButtonToRecievers(1, true);
            if (OVRInput.GetUp(OVRInput.RawButton.A))
                SendButtonToRecievers(1, false);
            if (OVRInput.GetDown(OVRInput.RawButton.B))
                SendButtonToRecievers(2, true);
            if (OVRInput.GetUp(OVRInput.RawButton.B))
                SendButtonToRecievers(2, false);

            // Axes.
            if (OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).magnitude > axisDeadzone)
            {
                SendAxisToRecievers(0, OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).x);
                SendAxisToRecievers(1, OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).y);
            }
        }
        // Right hand.
        else
        {
            // Buttons.
            if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
                SendButtonToRecievers(0, true);
            if (OVRInput.GetUp(OVRInput.RawButton.RIndexTrigger))
                SendButtonToRecievers(0, false);
            if (OVRInput.GetDown(OVRInput.RawButton.X))
                SendButtonToRecievers(1, true);
            if (OVRInput.GetUp(OVRInput.RawButton.X))
                SendButtonToRecievers(1, false);
            if (OVRInput.GetDown(OVRInput.RawButton.Y))
                SendButtonToRecievers(2, true);
            if (OVRInput.GetUp(OVRInput.RawButton.Y))
                SendButtonToRecievers(2, false);

            // Axes.
            if (OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).magnitude > axisDeadzone)
            {
                SendAxisToRecievers(0, OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x);
                SendAxisToRecievers(1, OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y);
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
			grabCandidates.Add(collider.gameObject);
        }
        if(collider.GetComponent<ITouchReciever>() != null){
            collider.GetComponent<ITouchReciever>().OnTouchEnter(interactorID, 0);
        }
    }

    void OnTriggerExit(Collider collider){
		grabCandidates.Remove(collider.gameObject);
        if(collider.GetComponent<ITouchReciever>() != null){
            collider.GetComponent<ITouchReciever>().OnTouchExit(interactorID, 0);
        }
    }
}
