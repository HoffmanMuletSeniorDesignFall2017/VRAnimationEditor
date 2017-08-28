using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class Test : MonoBehaviour {

	Animator animator;
	AnimationClip animClip;
	AnimationCurve animCurve;
	Keyframe keyframe;
	AnimationState animState;

	Animation anim;

	// Use this for initialization
	void Start () {
		//animator = GetComponent<Animator> ();
		//animClip = animator.GetCurrentAnimatorClipInfo(1)[0].clip;
		//animCurve =
		//keyframe

		anim = GetComponent<Animation> ();
		animClip = anim.clip;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			//animClip = anim.clip;

			animClip.ClearCurves ();
			//animClip.ClearCurves ();
			//animator.GetCurrentAnimatorStateInfo(1).normalizedTime = 0.0f;

		}
		if (Input.GetKeyDown (KeyCode.RightArrow)) {

			AnimationCurve testCurve = new AnimationCurve();
			Keyframe key1 = new Keyframe();
			Keyframe key2 = new Keyframe();

			key1.time = 0f;
			key1.value = 2;

			key2.time = 2f;
			key2.value = 1;

			testCurve.AddKey (key1);
			testCurve.AddKey (key2);

			animClip.SetCurve("", typeof(Transform), "localPosition.y", testCurve);

			//anim.

			//animClip.
		}
	}
}
