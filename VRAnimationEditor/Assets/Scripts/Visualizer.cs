using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualizer : MonoBehaviour, IPointerReciever {

	public bool selected = false;

	public bool grabbing = false;

	public bool childNeedsDeletion = false;

	public Visualizer associatedVisualizer;

	protected bool pointerHoveringOverThis = false;
	protected LinkedList<int> interactingPointers;
	protected LinkedList<int> pressingPointers;

	// Use this for initialization
	void Start () {
		interactingPointers = new LinkedList<int>();
		pressingPointers = new LinkedList<int>();

		this.enabled = false;		//We don't need update so we disable this
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnPointerExit(int pointerID)
	{
		
	}

	public void OnPointerEnter(int pointerID){

	}

	public void OnButtonDown(int pointerID, int buttonID)
	{
		
	}

	public void OnButtonUp(int pointerID, int buttonID)
	{
		

	}

}
