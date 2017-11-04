using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
	public Transform lookTowards;

	void Update() 
	{
		//transform.LookAt (lookTowards, Vector3.up);
		Vector3 oldAngles = transform.localEulerAngles;

		transform.LookAt (lookTowards);
		transform.localEulerAngles = new Vector3(oldAngles.x, transform.localEulerAngles.y + 180, oldAngles.z);
	}
}