using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.EditorVR;
using System;


public class TestToolMenu : MonoBehaviour, IMenu {

	public Action close;

	public bool visible
	{
		get { return gameObject.activeSelf; }
		set { gameObject.SetActive(value); }
	}

	public GameObject menuContent
	{
		get { return gameObject; }
	}

	public void Close(){
		close ();
	}
}
