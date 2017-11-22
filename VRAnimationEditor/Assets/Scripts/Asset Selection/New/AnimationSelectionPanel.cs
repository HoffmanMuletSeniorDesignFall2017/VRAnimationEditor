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
    public float loadFrametimeFraction = 0.25f;
    public AnimationTile selectedTile;

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
        System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
        float frameTime = 1 / 90f;
        Debug.Log("Frametime: " + frameTime + " s");
        float loadFrameTimeLimit = frameTime * loadFrametimeFraction * 1000;
        Debug.Log("Limiting load time per frame to " + loadFrameTimeLimit + " ms");
        timer.Start();
        string[] animGuids = AssetDatabase.FindAssets("t:AnimationClip");
        if (timer.ElapsedMilliseconds > loadFrameTimeLimit)
        {
            timer.Stop();
            yield return null;
            timer.Reset();
            timer.Start();
        }
        for (int i = 0; i < animGuids.Length; i++)
        {
            // Get animation path from GUID.
            string animPath = AssetDatabase.GUIDToAssetPath(animGuids[i]);
            AnimationClip tempAnim = AssetDatabase.LoadAssetAtPath<AnimationClip>(animPath);
            AddAnimation(tempAnim);
            if (timer.ElapsedMilliseconds > loadFrameTimeLimit)
            {
                timer.Stop();
                yield return null;
                timer.Reset();
                timer.Start();
            }
        }
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