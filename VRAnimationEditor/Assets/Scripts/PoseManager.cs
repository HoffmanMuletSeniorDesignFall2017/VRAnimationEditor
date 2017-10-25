using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseManager : MonoBehaviour {
    public AnimationClip animClip;
    public Transform rootTransform;
    public bool useRotation = true;
    public bool usePosition = false;

    private float time = 1;
    private List<NodeData> initialNodeData;

    void Start(){
        initialNodeData = GetHierarchyNodeDatas(rootTransform);
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

    private static void ApplyPose(List<NodeData> nodeDatas, Transform root){
        root.position = nodeDatas[0].position;
        root.rotation = nodeDatas[0].rotation;
        nodeDatas.RemoveAt(0);
        for (int i = 0; i < root.childCount; i++)
        {
            ApplyPose(nodeDatas, root.GetChild(i));
        }
    }

    private static List<NodeData> GetHierarchyNodeDatas(Transform root){
        List<NodeData> data = new List<NodeData>();
        data.Add(new NodeData(root));
        for (int i = 0; i < root.childCount; i++)
        {
            data.AddRange(GetHierarchyNodeDatas(root.GetChild(i)));
        }
        return data;
    }

    public class NodeData{
        public Vector3 position;
        public Quaternion rotation;

        public NodeData(Transform t){
            position = t.position;
            rotation = t.rotation;
        }
    }

    void OnTriggerEnter(){
        RestoreInitialPose();
    }

    public void SetModel(GameObject model){
        rootTransform = model.transform;
    }

    public void SetAnimationClip(AnimationClip animClip){
        this.animClip = animClip;
    }
}
