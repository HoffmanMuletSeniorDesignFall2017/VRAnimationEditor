using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.EditorVR;

public class WorkspaceUiScaleMaintainer : MonoBehaviour  {
    private Vector3 initScale;
    private Rect initRect;
    private RectTransform rectTrsfm;
    private bool isInitialized = false;

    public void Init () {
        initScale = transform.lossyScale;
        if(initScale == Vector3.zero){
            return;
        }
        rectTrsfm = GetComponent<RectTransform>();
        initRect = rectTrsfm.rect;
        isInitialized = true;
	}
	
    public void OnBoundsChange () {
        if(!isInitialized){
            Init();
            return;
        }
        Vector3 currentScale = transform.lossyScale;

        if(currentScale != initScale){
            float horizontalChange = currentScale.x / initScale.x;
            float verticalChange = currentScale.y / initScale.y;

            Vector3 newLocalScale = transform.localScale;
            newLocalScale.x /= horizontalChange;
            newLocalScale.y /= verticalChange;
            //transform.localScale = newLocalScale;

            rectTrsfm.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, initRect.size.x / horizontalChange);
            rectTrsfm.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, initRect.size.y / verticalChange);
        }
	}
}
