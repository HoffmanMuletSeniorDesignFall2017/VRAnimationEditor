using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyframeWorkArea : MonoBehaviour {

	//This is a window which will hold all of the keyframes for an animation. Needs to have an attached GameObject

	public float timeScale = 1;	 //The timeScale, i.e. if the user is zoomed in really far and is dealing with frames or zoomed out and dealing with seconds-minutes

	public float verticalZoom = 1;	//The vertical zoom, i.e., how much vertical space there is between curves

	public float bounds = 0;		//The x-axis bounds - i.e., there will be no more keyframes past this number
	public TimelineVisualizer timelineVisualizer;	//The visualizer for the timeline

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RefreshBounds(float newX){
		bounds = newX;
		timelineVisualizer.bound = bounds;
	}
}
