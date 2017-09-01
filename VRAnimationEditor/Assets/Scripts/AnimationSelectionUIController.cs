using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSelectionUIController : MonoBehaviour {
    public GameObject animationTilePrefab;
    public AssetLogger assetLogger;
    public Transform contentPanel;

	// Use this for initialization
	void Start () {
        InitAnimationTiles();
	}
	
    private void InitAnimationTiles(){
        for (int i = 0; i < assetLogger.animationClips.Count; i++)
        {
            GameObject animTile = Instantiate<GameObject>(animationTilePrefab, contentPanel);
            animTile.GetComponent<AnimationTileController>().animSelUICtrl = this;
            animTile.GetComponent<AnimationTileController>().SetAnimation(assetLogger.animationClips[i]);
            animTile.name = "animation tile " + i;
        }
    }

    public void SetSelectedAnimation(AnimationClip animClip){
        Debug.Log("Animation " + animClip.name + " selected.");
    }
}
