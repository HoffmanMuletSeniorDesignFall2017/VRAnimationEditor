using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineVisualizer : Visualizer {
	//Used to control the visualization of animation time

	public GameObject timeLine;	//The visual representation of the current time; i.e. a line from the top that intersects with keyframes when they are "hit"

	public Animator animator;		//The animator that is holding the animation that we are currently editing.

	public float bound = 0f;		//The value for the local position x bound of the timeline cursor (i.e., once it reaches the end, the animation is finished)

	private bool reset = false;

	private float animatorTime = 0f;

	private string currentClipName = "Test";

	private bool doneInitialSwitch = false;

	//private AnimatorOverrideController overrideController;
	private RuntimeAnimatorController originalController;

	// Use this for initialization
	void Start () {
		//TODO: Add a component that allows the visualizer to be "selected" (when it is selected, it will play the animation at this point)
		if (timeLine == null) {
			//FREAK OUT!
			Debug.LogError("You never set the timeLine gameObject in the TimelineVisualizer!");
		}

		timeLine.AddComponent<MovableVisualizer> ();
		timeLine.GetComponent<MovableVisualizer> ().constrainedToLocalX = true;
		timeLine.GetComponent<MovableVisualizer> ().associatedVisualizer = this;

		//overrideController = new AnimatorOverrideController ();
	}
	
	// Update is called once per frame
	void Update () {
		if (animator != null) {

			//Debug.Log (animator.GetCurrentAnimatorStateInfo (0).normalizedTime);
			if(animator.GetCurrentAnimatorStateInfo (0).normalizedTime != 0)
				animatorTime = animator.GetCurrentAnimatorStateInfo (0).normalizedTime;
			//Debug.Log (animatorTime);



			if (!selected) {	//If the timeline visualizer is not selected, then it should simply read the info from the animator.
				
				timeLine.transform.localPosition = new Vector3 (((animator.GetCurrentAnimatorStateInfo (0).normalizedTime) - Mathf.Floor (animator.GetCurrentAnimatorStateInfo (0).normalizedTime)) * bound, 
					timeLine.transform.localPosition.y, timeLine.transform.localPosition.z);


				if (animator.GetCurrentAnimatorStateInfo (0).normalizedTime > 1f) {
					animator.Play (animator.GetCurrentAnimatorStateInfo (0).shortNameHash, 0, 0f);
				}



			} 

			else {	//If the timeline visualizer IS selected, then that means the user is scrubbing! So, we have to set the animator time to the appropriate position of the visualizer.

				float adjustedPosition = timeLine.transform.localPosition.x / bound;
				if (adjustedPosition > 1f)
					adjustedPosition = 1f;
				else if (adjustedPosition < 0)
					adjustedPosition = 0;

				animator.Play (animator.GetCurrentAnimatorStateInfo (0).shortNameHash, 0, adjustedPosition);

				timeLine.transform.localPosition = new Vector3 (adjustedPosition * bound, timeLine.transform.localPosition.y, timeLine.transform.localPosition.z);
			}
		}
	}

	public float GetAnimatorTime(){
		return animatorTime;
	}

	public void ChangeClip(AnimationClip clip ){
		if (!doneInitialSwitch) {
			//Animator anim = GetComponent<Animator>(); 
			Animator anim = animator;
			//AnimatorOverrideController overrideController = 
				//new AnimatorOverrideController ();
			originalController = anim.runtimeAnimatorController;

			AnimatorStateInfo[] layerInfo = new AnimatorStateInfo[anim.layerCount];
			for (int i = 0; i < anim.layerCount; i++) {
				layerInfo [i] = anim.GetCurrentAnimatorStateInfo (i);
			}

			AnimatorOverrideController overrideController = new AnimatorOverrideController (anim.runtimeAnimatorController);

			//overrideController.runtimeAnimatorController = anim.runtimeAnimatorController;
			overrideController [currentClipName] = clip;
			anim.runtimeAnimatorController = overrideController;

			// Force an update
			anim.Update (0.0f);

			// Push back state
			for (int i = 0; i < anim.layerCount; i++) {
				anim.Play (layerInfo [i].nameHash, i, layerInfo [i].normalizedTime);
			}

			currentClipName = clip.name;
			//doneInitialSwitch = true;
		}/* else {
			Animator anim = animator;

			AnimatorStateInfo[] layerInfo = new AnimatorStateInfo[anim.layerCount];
			for (int i = 0; i < anim.layerCount; i++) {
				layerInfo [i] = anim.GetCurrentAnimatorStateInfo (i);
			}



			//overrideController.runtimeAnimatorController = originalController;
			//((AnimatorOverrideController) anim.runtimeAnimatorController) [currentClipName] = clip;
			overrideController[currentClipName] = clip;
			//anim.runtimeAnimatorController = overrideController;

			// Force an update
			anim.Update (0.0f);

			// Push back state
			for (int i = 0; i < anim.layerCount; i++) {
				anim.Play (layerInfo [i].nameHash, i, layerInfo [i].normalizedTime);
			}

			currentClipName = clip.name;
		}*/
	}

	public void ChangeTime(float newTime){
		if (animator != null) 
			animator.Play (animator.GetCurrentAnimatorStateInfo (0).shortNameHash, 0, newTime);
	}
}
