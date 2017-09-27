using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScrollController : MonoBehaviour {
    public RectTransform scrollBar;
    public RectTransform scrollBarBar;
    public LayoutGroup contentLayout;
    public bool checkPanelSizes;
    public bool updateScrollBarSize = true;
    public bool updateScrollBarPos = true;
    public float scrollSpeed = 1f;

    private float barScrollRange;
    private float contentScrollRange;
    private RectTransform contentRectTrsfm;
    private RectTransform view;
    private float scrollPosition;

	// Use this for initialization
	void Start () {
        view = GetComponent<RectTransform>();
        contentRectTrsfm = contentLayout.GetComponent<RectTransform>();
        StartCoroutine(UpdateScrollBarSize());
	}
	
	// Update is called once per frame
	void Update () {
		Scroll(Input.GetAxisRaw("Scroll"));
            
	}

    private IEnumerator UpdateScrollBarSize(){
        while(contentLayout.preferredHeight == 0){
            yield return null;
        }

        float contentVisiblility = view.rect.height / contentLayout.preferredHeight;
        Rect newScrollBarBarRect = scrollBarBar.rect;
        newScrollBarBarRect.height = scrollBar.rect.height * contentVisiblility;
        scrollBarBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newScrollBarBarRect.height);
        barScrollRange = scrollBar.rect.height - scrollBarBar.rect.height;
        contentScrollRange = contentLayout.preferredHeight - view.rect.height;
        SetScrollPosition(0);
    }
     
    private void SetScrollPosition(float scrollPos)
    {
        scrollPosition = Mathf.Clamp01(scrollPos);
        float barY = scrollBar.rect.height / 2 - scrollBarBar.rect.height / 2;
        barY -= barScrollRange * scrollPosition;
		if (float.IsNaN (barY)) {
			barY = 0;
		}
        scrollBarBar.localPosition = new Vector3(0, barY, 0);
        float contentY = 0;
        //contentY += contentLayout.preferredHeight / 2 - view.rect.height / 2;
        contentY += contentScrollRange * scrollPosition;
        if(float.IsNaN(contentY)){
            contentY = 0;
        }
        contentRectTrsfm.localPosition = new Vector3(0, contentY, 0);
    }

	public void ScrollUp(){
        SetScrollPosition(scrollPosition - scrollSpeed * Time.deltaTime / contentScrollRange);
    }

    public void ScrollDown(){
        SetScrollPosition(scrollPosition + scrollSpeed * Time.deltaTime / contentScrollRange);
    }

	public void Scroll(float delta){
		SetScrollPosition (scrollPosition - delta * scrollSpeed * Time.deltaTime / contentScrollRange);
	}
}
