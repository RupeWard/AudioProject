using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RJWS.Core.DebugDescribable;

namespace RJWS.Audio
{
	abstract public class AbstractWaveFormGenerator: Grph.AbstractGraphGenerator
	{
		public string Name
		{
			get;
			private set;
		}

		protected AbstractWaveFormGenerator(string n)
			: base( n )
		{
			Name = n;
		}

		abstract public bool IsTimeValid( double seconds );

		abstract public float GetValueForTimeSecs( double seconds);

		public override float GetYForX( double x )
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
