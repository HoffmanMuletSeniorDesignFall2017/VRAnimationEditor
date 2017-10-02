using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualizer : MonoBehaviour, IPointerReciever {

	public bool selected = false;

	public bool grabbing = false;

	public bool childNeedsDeletion = false;

	public Visualizer associatedVisualizer;

	protected bool pointerHoveringOverThis = false;
	private LinkedList<int> interactingPointers;
	private LinkedList<int> pressingPointers;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnPointerExit(int pointerID)
	{
		pointerHoveringOverThis = false;
	}

	public void OnPointerEnter(int pointerID){
		pointerHoveringOverThis = true;
	}

	public void OnButtonDown(int pointerID, int buttonID)
	{
		
	}

	public void OnButtonUp(int pointerID, int buttonID)
	{
		

	}

}
