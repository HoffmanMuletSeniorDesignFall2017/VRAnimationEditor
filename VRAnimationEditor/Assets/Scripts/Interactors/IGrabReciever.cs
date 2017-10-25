using System;
using UnityEngine;

public interface IGrabReciever
{
    void OnGrab (GameObject grabber);
    void OnRelease(GameObject grabber);
}