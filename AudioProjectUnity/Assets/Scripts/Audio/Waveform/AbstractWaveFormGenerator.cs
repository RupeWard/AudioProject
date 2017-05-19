using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RJWS.Core.DebugDescribable;

namespace RJWS.Audio
{
	abstract public class AbstractWaveFormGenerator: IDebugDescribable 
	{
		public string Name
		{
			get;
			private set;
		}

		protected AbstractWaveFormGenerator(string n)
		{
			Name = n;
		}

		abstract public float GetValueForTimeSecs( float seconds);

		abstract protected void DebugDescribeDetails( System.Text.StringBuilder sb );

		public void DebugDescribe(System.Text.StringBuilder sb)
		{
			sb.Append( "[WF " );
			DebugDescribeDetails( sb );
			sb.Append( "]" );
		}
	}
}
