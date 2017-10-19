using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RJWS.Core.DebugDescribable;
using System;

namespace RJWS.Audio
{
	abstract public class AbstractWaveFormGenerator: Grph.AbstractGraphGenerator
	{
		protected AbstractWaveFormGenerator(string n)
			:base( n )
		{
		}

		abstract public float GetValueForTimeSecs( float seconds);

		public override float GetYForX( float x )
		{
			return GetValueForTimeSecs( x );
		}

		abstract protected void DebugDescribeDetails( System.Text.StringBuilder sb );

		public override void DebugDescribe( System.Text.StringBuilder sb )
		{
			sb.Append( "[WF " );
			DebugDescribeDetails( sb );
			sb.Append( "]" );
		}
	}
}
