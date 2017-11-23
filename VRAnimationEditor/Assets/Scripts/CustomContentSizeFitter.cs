using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomContentSizeFitter : ContentSizeFitter {

	protected override void OnDisable()
    {
        // Do nothing instead of reseting layout.
    }
}
