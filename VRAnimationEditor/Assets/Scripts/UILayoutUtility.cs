using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class UILayoutUtility
{

    public static float GetLayoutGroupHeight(LayoutGroup lGroup)
	{
        return 0;
	}

	private static void MergeRects(ref Rect r1, ref Rect r2)
	{
		r1.xMax = Mathf.Max(r1.xMax, r2.xMax);
		r1.yMax = Mathf.Max(r1.yMax, r2.yMax);
		r1.xMin = Mathf.Min(r1.xMin, r2.xMin);
		r1.yMin = Mathf.Min(r1.yMin, r2.yMin);
	}
}
