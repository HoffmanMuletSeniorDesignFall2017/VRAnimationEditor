using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseManager : MonoBehaviour {
    public AnimationClip animClip;
    public Transform rootTransform;
    public bool useRotation = true;
    public bool usePosition = false;

    private float time = 1;
    private NodeData initialNodeData;

    void Update(){
        if (rootTransform == null)
        {
            return;
        }
        if (initialNodeData == null)
        {
            initialNodeData = new NodeData(rootTransform);
        }

        if (OVRInput.GetDown(OVRInput.RawButton.A))
        {
            Debug.Log("Reseting pose");
            RestoreInitialPose();
        }
    }
	
    public void GenerateKeyframesForCurrentPose(){
        RecursiveGenerateKeyframes(rootTransform);
    }

    private void RecursiveGenerateKeyframes(Transform t){
        
    }

    private void GenerateRotationKeyframes(Transform t, AnimationCurve curveX){
        if (useRotation)
        {
            

        }
    }

    public void RestoreInitialPose(){
        ApplyPose(initialNodeData, rootTransform);
    }

    private static void ApplyPose(NodeData nodeData, Transform root){
        root.localPosition = nodeData.position;
        root.localRotation = nodeData.rotation;
        int nDIndex = 0;
        for (int i = 0; i < root.childCount; i++)
        {
            if (root.GetChild(i).gameObject.layer != LayerMask.NameToLayer("UI"))
            {
                ApplyPose(nodeData.children[nDIndex], root.GetChild(i));
                nDIndex++;
            }
        }

    }


    public class NodeData{
        public Vector3 position;
        public Quaternion rotation;
        public List<NodeData> children;
        public string debugName;

        public NodeData(Transform t){
            position = t.localPosition;
            rotation = t.localRotation;
            debugName = t.name;
            children = new List<NodeData>();
            for(int i = 0; i < t.childCount; i++){
                if(t.GetChild(i).gameObject.layer  != LayerMask.NameToLayer("UI"))
                {
                    children.Add(new NodeData(t.GetChild(i)));
                }
            }
        }
    }
}
