using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RJWS.Enums;

[System.Serializable]
public class AxisDefn : RJWS.Core.DebugDescribable.IDebugDescribable
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

	public AxisDefn clone()
	{
		AxisDefn defn = new AxisDefn( );

		defn.axisType = this.axisType; ;

		defn.snap = this.snap;

		defn.axisName = this.axisName;
		defn.eDirection = this.eDirection;
		defn.value = this.value;
		defn.tickLabelSizeScaling = this.tickLabelSizeScaling; 
		defn.tickLineWidthScaling = this.tickLineWidthScaling;
		defn.tickLineLengthScaling = this.tickLineLengthScaling; 
		defn.tickBase = this.tickBase;
		defn.tickSpacing = this.tickSpacing;
		defn.axisLineWidthScaling = this.axisLineWidthScaling;
		defn.axisLabelSizeScaling = this.axisLabelSizeScaling;

		return defn;
}

	public void DebugDescribe( System.Text.StringBuilder sb)
	{
		sb.Append( "[ Axis: " ).Append( eDirection ).Append( " " );
		if (eDirection == RJWS.EOrthoDirection.Horizontal)
		{
			sb.Append( "H " );
		}
		else
		{
			sb.Append( "V " );
		}
		if (axisType == EAxisType.FixedValue)
		{
			sb.Append( "Fix@" );
		}
		else
		{
			sb.Append( "Fract" );
		}
		sb.Append( value.ToString( ) );
		sb.Append( "]" );

	}

public static AxisDefn CreateFixed(RJWS.EOrthoDirection dirn, float v)
	{
		AxisDefn defn = new AxisDefn( );
		defn.axisName = dirn.ToChar( ) + "_F_" + v.ToString( );
		defn.axisType = EAxisType.FixedValue;
		defn.eDirection = dirn;
		defn.value = v;

		return defn;
	}

	public static AxisDefn CreateFractional( RJWS.EOrthoDirection dirn, float v )
	{
		AxisDefn defn = new AxisDefn( );
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
