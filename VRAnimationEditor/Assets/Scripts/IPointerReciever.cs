﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interface for scripts that respond to pointer input.
// This would include mouse, gaze, hand, etc. pointers.
public interface IPointerReciever {
	void OnPointerEnter(int pointerID);
	void OnPointerExit(int pointerID);
	void OnButtonDown(int pointerID, int buttonID);
	void OnButtonUp(int pointerID, int buttonID);
}
