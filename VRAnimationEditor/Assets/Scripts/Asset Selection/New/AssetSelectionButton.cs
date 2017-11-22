using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssetSelectionButton : IAssetSelector {

    public IAssetRequester requester;
    public AnimationSelectionPanel animSelPanel;
    public ModelSelectionPanel modelSelPanel;
    public Button button;

    public override void RequestSelectedAnimationClip(IAssetRequester requester)
    {
        this.requester = requester;
    }

    public override void RequestSelectedAnimationClip(IAssetRequester requester, GameObject modelFilter)
    {
        this.requester = requester;
    }

    public override void RequestSelectedModel(IAssetRequester requester)
    {
        this.requester = requester;
    }

    public override void RequestSelectedModel(IAssetRequester requester, AnimationClip animClipFilter)
    {
        this.requester = requester;
    }
	
	void Update () {
        button.interactable = animSelPanel.selectedTile != null && modelSelPanel.selectedTile != null;
	}

    public void OnClick()
    {
        requester.SetModel(modelSelPanel.selectedTile.model);
        requester.SetAnimationClip(animSelPanel.selectedTile.anim);

    }
}
