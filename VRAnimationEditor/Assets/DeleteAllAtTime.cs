using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DeleteAllAtTime : MonoBehaviour
{

    float THRESHOLD = .05f;

    public AnimationVisualizer animVis;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClick()
    {
        float currentTime = animVis.keyframeWorkArea.GetComponent<KeyframeWorkArea>().timelineVisualizer.GetAnimatorTime();

        List<AnimationCurve> animCurves = new List<AnimationCurve>();

        for (int i = 0; i < animVis.GetNumAnimCurveVisualizers(); i++)
        {
            animCurves.Add(animVis.GetAnimCurveVisualizer(i).animCurve);
        }

        for(int i = 0; i < animCurves.Count; i++)
        {
            for (int j = 0; j < animCurves[i].keys.Length; j++) {
                Debug.Log(animCurves[i].keys[j].time * AnimationCurveVisualizer.X_OFFSET_CONSTANT);
                Debug.Log(currentTime * animVis.keyframeWorkArea.GetComponent<KeyframeWorkArea>().bounds);
                if(Mathf.Abs(animCurves[i].keys[j].time * AnimationCurveVisualizer.X_OFFSET_CONSTANT - currentTime * animVis.keyframeWorkArea.GetComponent<KeyframeWorkArea>().bounds) < THRESHOLD)
                {
                    animCurves[i].RemoveKey(j);
                    animVis.RefreshAnimationCurve(i);
                    j--;
                }
            }
        }

        animVis.RefreshCurves();
    }
}
