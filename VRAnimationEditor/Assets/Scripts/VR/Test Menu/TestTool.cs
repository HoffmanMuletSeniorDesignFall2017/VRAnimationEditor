using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.EditorVR;
using UnityEditor.Experimental.EditorVR.Utilities;
using UnityEngine.InputNew;

public class TestTool : MonoBehaviour, ITool, IInstantiateMenuUI, IUsesRayOrigin, IConnectInterfaces, ISelectTool, IStandardActionMap {
	[SerializeField]
	TestToolMenu menuPrefab;

	GameObject toolMenuObj;

	GameObject currentGameObject;

	public Transform rayOrigin { get; set; }

	void Start(){
		toolMenuObj = this.InstantiateMenuUI (rayOrigin, menuPrefab);
		TestToolMenu toolMenu = toolMenuObj.GetComponent<TestToolMenu> ();
		this.ConnectInterfaces (toolMenu, rayOrigin);
		toolMenu.close = Close;
	}

	public void ProcessInput(ActionMapInput input, ConsumeControlDelegate consumeControl){
		
	}

	void Close()
	{
		this.SelectTool(rayOrigin, GetType());
	}

	void OnDestroy()
	{
		ObjectUtils.Destroy(toolMenuObj);
	}
}
