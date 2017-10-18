using UnityEngine;

// Simple interface for anything that will send requests for selected assets.
// Done as an interface because I wasn't sure what should be responsible for this.
public interface IAssetRequester {
    void SetModel(GameObject obj);
    void SetAnimationClip(AnimationClip animClip);
}
