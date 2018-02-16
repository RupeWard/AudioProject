using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Graph.Display
{
	[System.Serializable]
	public class GraphDisplaySettings
	{
		public Color fixedHorizontalAxisColour;
		public Color fixedVerticalAxisColour;
		public Color screenHorizontalAxisColour;
		public Color screenVerticalAxisColour;

		public float defaultAxisWidth = 6f;

		public Color fractionalPointColour = Color.green;
		public Color samplePointColour = Color.red;

		public Color pureConnectorColour = Color.green;
		public Color sampleHidingConnectorColor = Color.red;

		public float lineWidth = 2f;
		public float pointSizeSampled = 5f;
		public float pointSizeFractional = 3.5f;

		/*
		public int numFractionalPoints = 32;
		public int numSampledPoints = 16;

		public int totalNumPoints
		{
			get { return numFractionalPoints + numSampledPoints;  }
		}
		*/

		public Color GetColor( AxisDefn axisDefn )
		{
			return GetColor( axisDefn.eDirection, axisDefn.axisType );
		}

		public Color GetColor( RJWS.EOrthoDirection dirn, AxisDefn.EAxisType axisType )
		{
			switch (dirn)
			{
				case RJWS.EOrthoDirection.Horizontal:
					{
						if (axisType == AxisDefn.EAxisType.FixedValue)
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
						if (axisType == AxisDefn.EAxisType.FixedValue)
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
}

