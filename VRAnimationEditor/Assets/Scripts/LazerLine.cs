using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerLine : MonoBehaviour {
	public Material lineMat;

	// Use this for initialization
	void Start () {
		MeshRenderer meshRend = gameObject.AddComponent<MeshRenderer>();
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		Mesh mesh = new Mesh ();
		mesh.vertices = new Vector3[]{ new Vector3 (0, 0, 0), new Vector3 (0, 0, 1) };
		mesh.SetIndices (new int[]{ 0, 1 }, MeshTopology.Lines, 0);
		meshFilter.mesh = mesh;
		meshRend.material = lineMat;
	}
}
