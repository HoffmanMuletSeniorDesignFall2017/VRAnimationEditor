using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interface for scripts that respond to pointer input.
// This would include mouse, gaze, hand, etc. pointers.
public interface IPointerReciever {
    void OnPointerEnter();
    void OnPointerExit();
    void OnClick();
}
