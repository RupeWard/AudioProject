using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RJWS.Enums;

[System.Serializable]
public class XX_AxisDefn 
{
	public enum EAxisType
	{
		FixedValue,
		ScreenFractionValue
	}
	public EAxisType axisType = EAxisType.FixedValue;
	
	public float snap = 0.1f;

	public string axisName = "Axis";
	public RJWS.EOrthoDirection eDirection = RJWS.EOrthoDirection.Horizontal;
	public float value = 0f;
	public float tickLabelSizeScaling = 1f; // default from settings
	public float tickLineWidthScaling = 1f; // default from settings
	public float tickLineLengthScaling = 1f; // default from settings
	public float tickBase = 0f;
	public float tickSpacing = 0.2f;
	public float axisLineWidthScaling = 1f; // default from settings
	public float axisLabelSizeScaling = 1f; // default from settings

	public static XX_AxisDefn CreateFixed(RJWS.EOrthoDirection dirn, float v)
	{
		XX_AxisDefn defn = new XX_AxisDefn( );
		defn.axisName = dirn.ToChar( ) + "_F_" + v.ToString( );
		defn.axisType = EAxisType.FixedValue;
		defn.eDirection = dirn;
		defn.value = v;

		return defn;
	}

	public static XX_AxisDefn CreateFractional( RJWS.EOrthoDirection dirn, float v )
	{
		XX_AxisDefn defn = new XX_AxisDefn( );
		defn.axisName = dirn.ToChar( ) + "_S_" + v.ToString( );
		defn.axisType = EAxisType.ScreenFractionValue;
		defn.eDirection = dirn;
		defn.value = v;

		if (v > 1f | v < 0f)
		{
			Debug.LogWarning( "Fractional axis value out of range at " + v );
		}
		return defn;
	}

}
