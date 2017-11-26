using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class RecenterControls : MonoBehaviour {
	void Update () {
		if((OVRInput.GetDown(OVRInput.RawButton.LThumbstick) && OVRInput.Get(OVRInput.RawButton.RThumbstick)) ||
           (OVRInput.GetDown(OVRInput.RawButton.RThumbstick) && OVRInput.Get(OVRInput.RawButton.LThumbstick)))
        {
            InputTracking.Recenter();
        }
	}
}
