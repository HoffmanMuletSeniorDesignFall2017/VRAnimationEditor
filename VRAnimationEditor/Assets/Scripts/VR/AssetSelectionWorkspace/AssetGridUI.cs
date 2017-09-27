using System.Collections.Generic;
using UnityEditor.Experimental.EditorVR.Extensions;
using UnityEditor.Experimental.EditorVR.Handles;
using UnityEditor.Experimental.EditorVR.Utilities;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.Experimental.EditorVR.Workspaces;

public class AssetGridUI : MonoBehaviour {
	public GameObject itemPrefab;
	public void Setup(){
		for (int i = 0; i < 5; i++) {
			ObjectUtils.Instantiate (itemPrefab, transform);
		}
	}
}
