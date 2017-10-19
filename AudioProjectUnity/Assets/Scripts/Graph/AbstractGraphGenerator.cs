using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RJWS.Core.DebugDescribable;

namespace RJWS.Grph
{
	abstract public class AbstractGraphGenerator: IDebugDescribable 
	{
		public string Name
		{
			get;
			private set;
		}

		protected AbstractGraphGenerator( string n)
		{
			Name = n;
		}

		abstract public Vector2 GetValueRange( );

		abstract public float GetYForX( float x);

		abstract public void DebugDescribe( System.Text.StringBuilder sb );
	}
}
