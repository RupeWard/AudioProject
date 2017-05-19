using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AxisDefn 
{
	public string axisName = "Axis";
	public RJWS.EOrthoDirection eDirection = RJWS.EOrthoDirection.Horizontal;
	public float value = 0f;
	public float tickLabelSize = 12f;
	public float tickBase = 0f;
	public float tickSpacing = 0.2f;
	public float axisLineWidth = 10f;
}
