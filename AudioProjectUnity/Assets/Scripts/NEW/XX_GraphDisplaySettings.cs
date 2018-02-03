using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class XX_GraphDisplaySettings 
{
	public Color fixedHorizontalAxisColour;
	public Color fixedVerticalAxisColour;
	public Color screenHorizontalAxisColour;
	public Color screenVerticalAxisColour;

	public float defaultAxisWidth = 6f;

	public Color GetColor( XX_AxisDefn axisDefn)
	{
		return GetColor( axisDefn.eDirection, axisDefn.axisType );
	}

public Color GetColor( RJWS.EOrthoDirection dirn, XX_AxisDefn.EAxisType axisType)
	{
		switch (dirn)
		{
			case RJWS.EOrthoDirection.Horizontal:
				{
					if (axisType == XX_AxisDefn.EAxisType.FixedValue)
					{
						return fixedHorizontalAxisColour;
					}
					else
					{
						return screenHorizontalAxisColour;
					}
				}
			case RJWS.EOrthoDirection.Vertical:
				{
					if (axisType == XX_AxisDefn.EAxisType.FixedValue)
					{
						return fixedVerticalAxisColour;
					}
					else
					{
						return screenVerticalAxisColour;
					}
				}
			default:
				{
					Debug.LogError( "Unhanlded dirn " + dirn );
					return Color.black;
				}
		}
	}
}
