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
    private NodeData tempEditData;
    private List<NodeEdit> editHistory;

    void Start(){
        editHistory = new List<NodeEdit>();
    }

    void Update(){
        if (rootTransform == null)
        {
            return;
        }
        if (initialNodeData == null)
        {
            initialNodeData = new NodeData(rootTransform, true);
        }

        if (OVRInput.GetDown(OVRInput.RawButton.A))
        {
            Debug.Log("Reseting pose");
            RestoreInitialPose();
        }
        if (OVRInput.GetDown(OVRInput.RawButton.X))
        {
            Undo();
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

    public void OnPoseEditStart(Transform t){
        tempEditData = new NodeData(t);
    }

    public void OnPoseEditFinish(Transform t){
        editHistory.Add(new NodeEdit(tempEditData, t));
    }

    private void Undo(){
        if (editHistory.Count > 0)
        {
            NodeEdit undoEdit = editHistory[editHistory.Count - 1];
            undoEdit.editTransform.localPosition -= undoEdit.deltaPos;
            //undoEdit.editTransform.localRotation *= undoEdit.deltaRot;//Quaternion.Inverse(undoEdit.deltaRot);
            editHistory.RemoveAt(editHistory.Count - 1);
        }
    }


    public class NodeData{
        public Vector3 position;
        public Quaternion rotation;
        public List<NodeData> children;

        public NodeData(Transform t, bool loadChildren = false){
            position = t.localPosition;
            rotation = t.localRotation;
            if(loadChildren){
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

    public class NodeEdit{
        public Transform editTransform;
        public Vector3 deltaPos;
        public Quaternion deltaRot;

        public NodeEdit(NodeData initialNodeData, Transform editedTransform){
            editTransform = editedTransform;
			if(editedTransform == null || initialNodeData == null)
				return;
            deltaPos = editTransform.localPosition - initialNodeData.position;
            deltaRot = Quaternion.Inverse(initialNodeData.rotation) * editTransform.localRotation;
        }
    }
}
