using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class JumpToNextKeyframe : MonoBehaviour {

    public AnimationVisualizer animVis;

    public static float FUDGE_FACTOR = 0.001f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick()
    {
        bool foundNextKeyframe = false;

        float currentTime = animVis.keyframeWorkArea.GetComponent<KeyframeWorkArea>().timelineVisualizer.GetAnimatorTime();
        float closestTime = 2f;

        List<AnimationCurve> animCurves = new List<AnimationCurve>();

        for(int i = 0; i < animVis.GetNumAnimCurveVisualizers(); i++)
        {
            animCurves.Add(animVis.GetAnimCurveVisualizer(i).animCurve);
        }

        for(int i = 0; i < animCurves.Count; i++)
        {
            for(int j = 0; j < animCurves[i].keys.Length; j++)
            {
                if(animCurves[i].keys[j].time / animVis.keyframeWorkArea.GetComponent<KeyframeWorkArea>().bounds * AnimationCurveVisualizer.X_OFFSET_CONSTANT > currentTime + FUDGE_FACTOR)
                {
                    if(animCurves[i].keys[j].time / animVis.keyframeWorkArea.GetComponent<KeyframeWorkArea>().bounds * AnimationCurveVisualizer.X_OFFSET_CONSTANT < closestTime)
                    {
                        foundNextKeyframe = true;
                        closestTime = animCurves[i].keys[j].time / animVis.keyframeWorkArea.GetComponent<KeyframeWorkArea>().bounds * AnimationCurveVisualizer.X_OFFSET_CONSTANT;
                    }
                }
            }
        }

        if (foundNextKeyframe)
        {
            if (closestTime >= .99f)
            {
                animVis.keyframeWorkArea.GetComponent<KeyframeWorkArea>().timelineVisualizer.ChangeTime(.99f);
            }
            else
            {
                animVis.keyframeWorkArea.GetComponent<KeyframeWorkArea>().timelineVisualizer.ChangeTime(closestTime);
            }
        }
        else
        {
            animVis.keyframeWorkArea.GetComponent<KeyframeWorkArea>().timelineVisualizer.ChangeTime(.99f);
        }
    }
}
