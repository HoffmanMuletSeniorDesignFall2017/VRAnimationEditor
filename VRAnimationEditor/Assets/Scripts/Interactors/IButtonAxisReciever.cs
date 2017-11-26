using System;
using UnityEngine;

public interface IButtonAxisReciever
{
	void OnRecieveButton (int sourceID, int buttonID, bool buttonState);
	void OnRecieveAxis(int sourceID, int axisID, float axisValue);
	GameObject GetGameObject();
}