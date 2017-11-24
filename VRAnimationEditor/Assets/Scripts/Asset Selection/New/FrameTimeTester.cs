using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameTimeTester : MonoBehaviour {

    private int frameCount = 0;
    public float targetFrameTime = 1 / 90f;
    public float maxDeviation;
    public float maxDeviationFract;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        frameCount++;
        if(frameCount > 30)
        {
            float deviation = Mathf.Abs(targetFrameTime - Time.deltaTime);
            if (maxDeviation < deviation)
            {
                maxDeviation = deviation;
                maxDeviationFract = deviation / targetFrameTime;
            }
        }       
    }
}
