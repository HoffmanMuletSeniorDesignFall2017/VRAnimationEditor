using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Script for controlling tiles in the model selection UI.
public class ModelTileController : MonoBehaviour, IPointerReciever, IButtonAxisReciever
{
	// Tile UI text component.
	public Text modelNameText;
	// Tile highlight image component.
	public Image highlightImage;
	public Color highlightColor, selectColor;
	// Reference to parent ModelSelectionUIController.
	// This is set by that script when it instantiates the tile prefab.
	// The reference is needed here for setting the selected model, which is
	// done through this parent script.
	public ModelSelectionUIController modelUICtrl;

	// Model tied to this tile.
	private GameObject model;
	// List of pointers currently pointing at this tile. Needed when using multiple pointers,
	// so that e.g. when you take one pointer off while another pointer is on the tile, it will
	// stay highlighted.
	private LinkedList<int> interactingPointers;
	private LinkedList<int> pressingPointers;

	public enum HighlightState	{None, Highlighted, Selected};
	private HighlightState currentHighlightState = HighlightState.None;

	void Awake()
	{
		SetHighlight(HighlightState.None);
		interactingPointers = new LinkedList<int>();
		pressingPointers = new LinkedList<int>();
	}

	// Assigns a 3D model to this tile.
	// Called by ModelSelectionUIController when setting up tiles.
	public void SetModel(GameObject model)
	{
		this.model = model;
		modelNameText.text = model.name;
	}

	private void SetHighlight(HighlightState state)
	{
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
		currentHighlightState = state;
	}

	// Pointer interaction methods.

	// Called by a pointer controller when a pointer starts pointing at a tile.
	public void OnPointerEnter(int pointerID)
	{
		// Add new pointer ID to the list.
		interactingPointers.AddFirst(pointerID);
		// Highlight tile if not already highlighted.
		if (currentHighlightState == HighlightState.None)
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

    public void OnRecieveButton(int sourceID, int buttonID, bool state){
        if (state == true)
        {
            OnButtonDown(sourceID, buttonID);
        }
        else
        {
            OnButtonUp(sourceID, buttonID);
        }
    }

    public void OnRecieveAxis(int sourceID, int axisID, float  axisValue){
        
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
				modelUICtrl.SetSelectedModel(model);
				pressingPointers.Remove(pointerID);
				if (pressingPointers.Count == 0)
				{
					SetHighlight(HighlightState.Highlighted);
				}
			}
		}

	}
}
