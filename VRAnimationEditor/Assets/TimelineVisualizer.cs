using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineVisualizer : MonoBehaviour {
	//Used to control the visualization of animation time

	public GameObject timeLine;	//The visual representation of the current time; i.e. a line from the top that intersects with keyframes when they are "hit"

	public Animator animator;		//The animator that is holding the animation that we are currently editing.

	public float bound = 0f;		//The value for the local position x bound of the timeline cursor (i.e., once it reaches the end, the animation is finished)

	private bool reset = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (animator != null) {
			timeLine.transform.localPosition = new Vector3 (((animator.GetCurrentAnimatorStateInfo (0).normalizedTime) - Mathf.Floor (animator.GetCurrentAnimatorStateInfo (0).normalizedTime)) * bound, 
				timeLine.transform.localPosition.y, timeLine.transform.localPosition.z);
		}
	}
}
