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
        GameObject ui = this.InstantiateUI(uiPrefab, m_WorkspaceUI.topFaceContainer, false);;

        preventResize = true;


        /*if(firstSetup){
            Debug.Log("Spawning workspace copy");
            Instantiate(gameObject);
        }
        else{
            Debug.Log("Workspace already copied");
        }*/
        //Transform root = GetRoot();
        //LongDebugLog(GetHierarchyString(root));

    }

    private Transform GetRoot(){
        Transform root = transform;
        while(root.parent != null){
            root = root.parent;
        }
        return root;
    }

    private string GetHierarchyString(Transform trsfm, int level = 0){
        string str = "";
        for(int i = 0; i < level; i++){
            str += "  ";
        }
        str += trsfm.name;
        if(trsfm == transform){
            str += " *";
        }
        str += "\n";
        for(int i = 0; i < trsfm.childCount; i++){
            str += GetHierarchyString(trsfm.GetChild(i), level + 1);
        }
        return str;
    }

    private void LongDebugLog(string message){
        string[] lines = message.Split('\n');
        int chunkLineCount = 0;
        string chunk = "";
        for(int i = 0; i < lines.Length; i++){
            chunk += lines[i] + '\n';
            chunkLineCount++;
            if(chunkLineCount > 450){
                Debug.Log(chunk);
                chunkLineCount = 0;
                chunk = "";
            }
        }
        if(chunkLineCount > 0){
            Debug.Log(chunk);
        }
    }
        
}