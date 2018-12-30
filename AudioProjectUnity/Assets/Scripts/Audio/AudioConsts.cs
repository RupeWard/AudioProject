using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Core.Audio
{
	static public class AudioConsts 
	{
		public const double CONCERT_A = 440.0;
		public static readonly double CONCERT_C = CONCERT_A * System.Math.Pow( 1.05956, 3.0 );

		static public readonly List<float> s_standardGuitarTuning = new List<float>( )
		{
			329.63f,
			246.94f,
			196.00f,
			146.83f,
			110.00f,
			82.41f
		};


	}
}
