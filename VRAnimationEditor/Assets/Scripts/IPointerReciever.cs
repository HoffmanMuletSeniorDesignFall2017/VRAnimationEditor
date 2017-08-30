using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPointerReciever {
    void OnPointerEnter();
    void OnPointerExit();
    void OnClick();
}
