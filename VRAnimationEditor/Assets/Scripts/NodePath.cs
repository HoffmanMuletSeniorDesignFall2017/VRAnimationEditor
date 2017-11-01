using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NodePath : MonoBehaviour {
    public Transform node;
    private MeshFilter meshFilter;
    private Mesh mesh;
    private Vector3 lastPosition;
	void Start () {
        meshFilter = GetComponent<MeshFilter>();
        mesh = new Mesh();
        meshFilter.mesh = mesh;
        PlotPosition();
        lastPosition = node.position;
	}

    void Update(){
        if (node.position != lastPosition)
        {
            PlotPosition();
        }
    }

    private void PlotPosition(){
        Mesh newMesh = new Mesh();
        List<Vector3> vertPoss = new List<Vector3>();
        vertPoss.AddRange(mesh.vertices);
        vertPoss.Add(node.position);
        List<int> indices = new List<int>();
        indices.AddRange(mesh.GetIndices(0));
        indices.Add(indices.Count);
        newMesh.SetVertices(vertPoss);
        newMesh.SetIndices(indices.ToArray(), MeshTopology.LineStrip, 0);
        meshFilter.mesh = newMesh;
        mesh = newMesh;
    }
}
