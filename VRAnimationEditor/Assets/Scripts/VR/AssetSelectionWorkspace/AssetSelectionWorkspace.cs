using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.EditorVR;
using UnityEditor.Experimental.EditorVR.Workspaces;
using System;

class AssetSelectionWorkspace : Workspace {
    [SerializeField]
    public GameObject uiPrefab;

    public override void Setup()
    {
        // Set inital workspace bounds.
        m_CustomStartingBounds = new Vector3(0.6f, MinBounds.y, 0.4f);

        base.Setup();

        // Instantiate ui and set its transform.
        GameObject ui = this.InstantiateUI(uiPrefab);
        ui.transform.SetParent(m_WorkspaceUI.topFaceContainer, false);
        ui.transform.localPosition = new Vector3(0, 0, 0);
        ui.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);


    }
}