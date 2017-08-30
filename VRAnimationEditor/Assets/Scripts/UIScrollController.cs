using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScrollController : MonoBehaviour {
    public RectTransform scrollBarBar;
    public RectTransform contentPanel;
    public bool checkPanelSizes;

    private RectTransform view;

	// Use this for initialization
	void Start () {
        view = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
        if(checkPanelSizes){
            Debug.Log("View height: " + view.rect.height);
            Debug.Log("Content height: " + contentPanel.rect.height);
            checkPanelSizes = false;
        }
	}
}
