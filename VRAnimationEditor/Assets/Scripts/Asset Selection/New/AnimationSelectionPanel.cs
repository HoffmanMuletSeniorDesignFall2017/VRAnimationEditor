using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class AnimationSelectionPanel : MonoBehaviour
{
    public GameObject animTilePrefab;
    public GameObject contentObj;
    public ScrollRect scrollRect;
    public float scrollSpeed = 0.1f;
    public AnimationTile selectedTile;
    public CustomContentSizeFitter customFitter;
    public int maxAnimationCount = 100;

    private int animCount = 0;

    void Start()
    {
        StartCoroutine(LoadAnimsRoutine());
    }

    void Update()
    {
        scrollRect.verticalNormalizedPosition += (OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).y +
            OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y) * scrollSpeed * Time.deltaTime;
    }

    private void ClearContent()
    {
        for (int i = 0; i < contentObj.transform.childCount; i++)
        {
            Destroy(contentObj.transform.GetChild(i));
        }
    }

    private void AddAnimation(AnimationClip anim)
    {

        GameObject tile = Instantiate(animTilePrefab, contentObj.transform);
        tile.GetComponent<AnimationTile>().Init(anim, this);
    }

    public IEnumerator LoadAnimsRoutine()
    {
        string[] animGuids = AssetDatabase.FindAssets("t:AnimationClip");
        yield return null;
        for (int i = 0; i < animGuids.Length && animCount < maxAnimationCount; i++)
        {
            // Get animation path from GUID.
            string animPath = AssetDatabase.GUIDToAssetPath(animGuids[i]);
            yield return null;
            AnimationClip tempAnim = AssetDatabase.LoadAssetAtPath<AnimationClip>(animPath);
            yield return null;
            AddAnimation(tempAnim);
            animCount++;
            customFitter.enabled = animCount % 8 == 3;
        }
        customFitter.enabled = true;
        yield return null;
        customFitter.enabled = false;
    }

    public void SetSelectedTile(AnimationTile tile)
    {
        if (selectedTile != null)
        {
            selectedTile.SetSelect(false);
        }
        selectedTile = tile;
        tile.SetSelect(true);
    }
}