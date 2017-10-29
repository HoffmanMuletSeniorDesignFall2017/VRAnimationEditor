using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualizer : MonoBehaviour {

	public bool selected = false;

	public bool grabbing = false;

	public bool childNeedsDeletion = false;

	public Visualizer associatedVisualizer;

	protected bool pointerHoveringOverThis = false;

	// Use this for initialization
	void Start () {

		this.enabled = false;		//We don't need update so we disable this
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
