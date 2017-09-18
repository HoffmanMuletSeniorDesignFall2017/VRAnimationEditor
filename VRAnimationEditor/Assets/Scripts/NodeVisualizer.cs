﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeVisualizer : MonoBehaviour {
	public bool isTemplate = false;
	public Transform root;
	public GameObject nodeMarkerPrefab;
	public bool makeTransparent = true;
	public Material transparentTemplate;

	private List<Material> initialMaterials;
	private List<Renderer> meshRends;

	// Use this for initialization
	void Start () {
		if (isTemplate) {
			return;
		}
		meshRends = GetMeshRenderers (gameObject);
		Debug.Log ("Found " + meshRends.Count + " renderers!");
		initialMaterials = GetMaterials (meshRends);
		if (makeTransparent) {
			ReplaceMaterials (meshRends, transparentTemplate);
		}
		if (root == null) {
			root = GuessRoot ();
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
		GameObject marker = Instantiate (nodeMarkerPrefab, obj);
		marker.transform.localPosition = Vector3.zero;
		marker.transform.localRotation = Quaternion.identity;
	}

	private List<Renderer> GetMeshRenderers(GameObject obj){
		List<Renderer> renderers = new List<Renderer> ();
		for(int i = 0; i < obj.transform.childCount; i++){
			renderers.AddRange(GetMeshRenderers(obj.transform.GetChild(i).gameObject));
		}
		if (obj.GetComponent<Renderer> () != null) {
			renderers.Add (obj.GetComponent<Renderer> ());
		}
		return renderers;
	}
		
	private List<Material> GetMaterials(List<Renderer> renderers){
		List<Material> materials = new List<Material> ();
		for (int i = 0; i < renderers.Count; i++) {
			materials.AddRange (renderers [i].materials);
		}
		return materials;
	}

	private void ReplaceMaterials(List<Renderer> renderers, Material replaceMat){
		for(int i = 0; i < renderers.Count; i++){
			Material[] newMatSet = renderers[i].materials;
			for(int j = 0; j < newMatSet.Length; j++){
				newMatSet[j] = replaceMat;
			}
			renderers[i].materials = newMatSet;
		}
	}

	private Transform GuessRoot(){
		Transform deepest = transform;
		int maxLevelCount = 0;
		for (int i = 0; i < transform.childCount; i++) {
			int levelCount = GetLevelCount (transform.GetChild (i));
			if (levelCount >= maxLevelCount) {
				deepest = transform.GetChild (i);
				maxLevelCount = levelCount;
			}
		}
		return deepest;
	}

	private int GetLevelCount(Transform obj){
		int levels = 0;
		for (int i = 0; i < obj.childCount; i++) {
			levels = Mathf.Max (levels, GetLevelCount (obj.GetChild (i)));
		}
		return levels + 1;
	}
}
