using System.Collections.Generic;
using UnityEditor.Experimental.EditorVR.Extensions;
using UnityEditor.Experimental.EditorVR.Handles;
using UnityEditor.Experimental.EditorVR.Utilities;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.Experimental.EditorVR.Workspaces;

class AssetSelectionWorkspace : Workspace {
	[SerializeField]
	GameObject contentPrefab;

	public override void Setup ()
	{
		base.Setup ();

		GameObject contentObj = ObjectUtils.Instantiate (contentPrefab, m_WorkspaceUI.sceneContainer, false);
		contentObj.GetComponent<AssetGridUI> ().Setup ();
	}
}
