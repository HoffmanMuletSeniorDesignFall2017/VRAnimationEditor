using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRButton : MonoBehaviour, IPointerReciever, IButtonAxisReciever {
	public Button button;
	public Image image;

	private List<int> focusingPointerIds;
    private List<int> pressingPointerIds;

	void Start(){
		focusingPointerIds = new List<int>();
        pressingPointerIds = new List<int>();
	}

	public void OnPointerEnter(int id){        
        if (pressingPointerIds.Count == 0)
        {
            image.color = button.colors.highlightedColor;
        }
		if (!focusingPointerIds.Contains(id))
		{
			focusingPointerIds.Add(id);
		}
        ButtonAxisEmitterLookup.RegisterReciever(this, id);
	}

	public void OnPointerExit(int id){
		focusingPointerIds.Remove(id);
        if (focusingPointerIds.Count == 0)
		{
            image.color = button.colors.normalColor;
            pressingPointerIds.Clear();
		}
        ButtonAxisEmitterLookup.UnregisterReciever(this, id);
	}
        
	public void OnRecieveButton (int sourceID, int buttonID, bool buttonState){
        if (buttonState == true)
        {
            if(!pressingPointerIds.Contains(sourceID)){
                pressingPointerIds.Add(sourceID);
            }
            image.color = button.colors.pressedColor;
        }
        else
        {
            if (pressingPointerIds.Contains(sourceID))
            {
                button.onClick.Invoke();
                pressingPointerIds.Remove(sourceID);
            }
            if (pressingPointerIds.Count == 0)
            {
                if (focusingPointerIds.Count == 0)
                {
                    image.color = button.colors.normalColor;
                }
                else
                {
                    image.color = button.colors.highlightedColor;
                }
            }
        }
	}

	public void OnRecieveAxis(int sourceID, int axisID, float axisValue){
		
	}

	public GameObject GetGameObject(){
		return gameObject;
	}
}
