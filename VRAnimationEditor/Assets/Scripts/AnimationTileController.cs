using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationTileController : MonoBehaviour, IPointerReciever {
    public Text animNameText;
    public Image highlightImage;
    public Color highlightColor, selectColor;
    public AnimationSelectionUIController animSelUICtrl;

    private AnimationClip animClip;
    private LinkedList<int> interactingPointers;
    private LinkedList<int> pressingPointers;

    public enum HighlightState{None, Highlighted, Selected};
    private HighlightState currentHState;

    void Start(){
        SetHighlight(HighlightState.None);
        interactingPointers = new LinkedList<int>();
        pressingPointers = new LinkedList<int>();
    }

    private void SetHighlight(HighlightState state){
        switch (state)
        {
            case HighlightState.Highlighted:
                highlightImage.color = highlightColor;
                highlightImage.enabled = true;
                break;
            case HighlightState.Selected:
                highlightImage.color = selectColor;
                highlightImage.enabled = true;
                break;
            default:
                highlightImage.enabled = false;
                break;
        }
        currentHState = state;
    }

    public void SetAnimation(AnimationClip animClip){
        this.animClip = animClip;
        animNameText.text = animClip.name;
    }

    // Pointer interaction methods.

    // Called by a pointer controller when a pointer starts pointing at a tile.
    public void OnPointerEnter(int pointerID)
    {
        // Add new pointer ID to the list.
        interactingPointers.AddFirst(pointerID);
        // Highlight tile if not already highlighted.
        if (currentHState == HighlightState.None)
        {
            SetHighlight(HighlightState.Highlighted);
        }
    }

    // Called by a pointer controller when a pointer stops pointing at a tile.
    public void OnPointerExit(int pointerID)
    {
        // Remove the pointer ID from the list.
        interactingPointers.Remove(pointerID);
        pressingPointers.Remove(pointerID);
        // If there are no more interacting pointers, de-highlight the tile.
        if (interactingPointers.Count == 0)
        {
            SetHighlight(HighlightState.None);
        }
        else if (pressingPointers.Count == 0)
        {
            SetHighlight(HighlightState.Highlighted);
        }
    }

    public void OnButtonDown(int pointerID, int buttonID)
    {
        if (buttonID == 0)
        {
            pressingPointers.AddFirst(pointerID);
            SetHighlight(HighlightState.Selected);
        }
    }

    public void OnButtonUp(int pointerID, int buttonID)
    {
        if (buttonID == 0)
        {
            if (pressingPointers.Contains(pointerID))
            {
                animSelUICtrl.SetSelectedAnimation(animClip);
                pressingPointers.Remove(pointerID);
                if (pressingPointers.Count == 0)
                {
                    SetHighlight(HighlightState.Highlighted);
                }
            }
        }

    }
}
