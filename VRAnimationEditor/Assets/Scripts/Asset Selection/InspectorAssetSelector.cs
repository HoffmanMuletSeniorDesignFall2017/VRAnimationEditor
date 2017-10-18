using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectorAssetSelector : IAssetSelector {
    public GameObject model;
    public AnimationClip animClip;

    public override void RequestSelectedModel(IAssetRequester requester){
        requester.SetModel(model);
    }

    public override void RequestSelectedModel(IAssetRequester requester, AnimationClip animClipFilter){
        RequestSelectedModel(requester);
		Debug.Log ("Filtered request not supported.");
    }

    public override void RequestSelectedAnimationClip(IAssetRequester requester){
        requester.SetAnimationClip(animClip);
    }

    public override void RequestSelectedAnimationClip(IAssetRequester requester, GameObject modelFilter){
        RequestSelectedAnimationClip(requester);
		Debug.Log ("Filtered request not supported.");
    }
	
}
