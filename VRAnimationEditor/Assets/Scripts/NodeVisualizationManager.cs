using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is for visualizing the transform nodes of a model.
public class NodeVisualizationManager : MonoBehaviour {
	public bool isTemplate = false;
	public Transform root;
	public GameObject nodeMarkerPrefab;
	public bool makeTransparent = true;
	public Material transparentTemplate;

	private List<Material> initialMaterials;
	private List<Renderer> meshRends;
    private List<GameObject> nodeMarkers;

	// Use this for initialization
	void Start () {
		if (isTemplate) {
			return;
		}
		meshRends = GetMeshRenderers (gameObject);
		initialMaterials = GetMaterials (meshRends);
		if (makeTransparent) {
			ReplaceMaterials (meshRends, transparentTemplate);
		}
		if (root == null) {
			//root = GuessRoot ();
			root = transform;
		}

        nodeMarkers = new List<GameObject>();

		SpawnNodeMarkers (root);

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// Spawn markers for each transform in an object hierarchy.
	private void SpawnNodeMarkers(Transform obj){
		for (int i = 0; i < obj.childCount; i++) {
			SpawnNodeMarkers (obj.GetChild (i));
		}
		GameObject marker = Instantiate (nodeMarkerPrefab, obj);
		marker.transform.localPosition = Vector3.zero;
		marker.transform.localRotation = Quaternion.identity;
		marker.GetComponent<ModelNodeController> ().masterObject = this.gameObject;

        nodeMarkers.Add(marker);
	}

    public void HideNodeMarkers()
    {
        for(int i = 0; i < nodeMarkers.Count; i++)
        {
            nodeMarkers[i].SetActive(false);
        }
    }

    public void ShowNodeMarkers()
    {
        for (int i = 0; i < nodeMarkers.Count; i++)
        {
            nodeMarkers[i].SetActive(true);
        }
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
		int matNum = 0;
		for(int i = 0; i < renderers.Count; i++){
			Material[] newMatSet = renderers[i].materials;
			for(int j = 0; j < newMatSet.Length; j++){
				//newMatSet[j] = replaceMat;
				newMatSet[j] = new Material(replaceMat);
				newMatSet [j].mainTexture = initialMaterials [matNum].mainTexture;
				matNum++;
			}
			renderers[i].materials = newMatSet;
		}
	}

    public void ReplaceMaterialsExternal()
    {
        ReplaceMaterials(meshRends, transparentTemplate);
    }

    public void RestoreMaterials()
    {
        int matNum = 0;
        for (int i = 0; i < meshRends.Count; i++)
        {
            Material[] newMatSet = meshRends[i].materials;
            for (int j = 0; j < newMatSet.Length; j++)
            {
                //newMatSet[j] = replaceMat;
                newMatSet[j] = new Material(initialMaterials[matNum]);
                //newMatSet[j].mainTexture = initialMaterials[matNum].mainTexture;
                matNum++;
            }
            meshRends[i].materials = newMatSet;
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
