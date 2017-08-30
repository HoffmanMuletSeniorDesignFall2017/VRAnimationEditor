using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePointerController : MonoBehaviour {
    private GameObject focus;
	
	// Update is called once per frame
	void Update () {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(mouseRay, out hitInfo))
        {
            if (hitInfo.collider.gameObject != focus)
            {
                ChangeFocus(hitInfo.collider.gameObject);
            }
        }
        else
        {
            if (focus != null)
            {
                ChangeFocus(null);
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (HasPointerReciever(focus))
            {
                focus.GetComponent<IPointerReciever>().OnClick();
            }
        }
	}

    private void ChangeFocus(GameObject newFocus){
        if (focus == newFocus)
        {
            return;
        }
        if (HasPointerReciever(focus))
        {
            focus.GetComponent<IPointerReciever>().OnPointerExit();
        }
        if (HasPointerReciever(newFocus))
        {
            newFocus.GetComponent<IPointerReciever>().OnPointerEnter();
        }
        focus = newFocus;
    }

    private bool HasPointerReciever(GameObject obj){
        if (obj == null)
        {
            return false;
        }
        return obj.GetComponent<IPointerReciever>() != null;
    }
}
