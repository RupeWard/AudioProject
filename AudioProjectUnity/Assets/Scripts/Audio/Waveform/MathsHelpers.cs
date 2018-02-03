using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS
{
	static public class MathsHelpers
	{
		static public float FLerpD( float f0, float f1, double fraction )
		{
			return (f0 + (float)(fraction * (f1 - f0)));
		}
	}

}
