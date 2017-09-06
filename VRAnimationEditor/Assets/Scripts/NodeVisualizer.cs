using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeVisualizer : MonoBehaviour {
	public Transform root;
	public GameObject nodeMarkerPrefab;

	public Material transparentTemplate;

	private List<List<Material>> initialMaterials;
	private List<Renderer> meshRends;
	// Use this for initialization
	void Start () {
		meshRends = GetMeshRenderers (root.gameObject);

		initialMaterials = new List<List<Material>> ();
		for (int i = 0; i < meshRends.Count; i++) {
			initialMaterials.Add (new List<Material> ());
			Material[] newMats = new Material[meshRends [i].materials.Length];
			for (int j = 0; j < meshRends [i].materials.Length; j++) {
				initialMaterials [i].Add (meshRends [i].materials [j]);
				newMats [j] = transparentTemplate;
				newMats [j].mainTexture = initialMaterials [i] [j].mainTexture;
			}
			meshRends [i].materials = newMats;
			//meshRends [i].material.mainTexture = initialMaterials [i] [0].mainTexture;
		}

		SpawnNodeMarkers (root);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void SpawnNodeMarkers(Transform obj){
		for (int i = 0; i < obj.childCount; i++) {
			SpawnNodeMarkers (obj.GetChild (i));
		}
		Instantiate (nodeMarkerPrefab, obj);
	}

	private List<Renderer> GetMeshRenderers(GameObject obj){
		List<Renderer> renderers = new List<Renderer> ();
		for(int i = 0; i < obj.transform.childCount; i++){
			renderers.AddRange(GetMeshRenderers(obj.transform.GetChild(i).gameObject));
		}
		if (GetComponent<Renderer> () != null) {
			renderers.Add (GetComponent<Renderer> ());
		}
		return renderers;
	}

	private void CopyMaterials(){
		initialMaterials = new List<List<Material>> ();
	}
}
