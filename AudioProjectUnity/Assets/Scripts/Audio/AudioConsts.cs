using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Core.Audio
{
	static public class AudioConsts 
	{
		public const double CONCERT_A = 440.0;
		public static readonly double CONCERT_C = CONCERT_A * System.Math.Pow( 1.05956, 3.0 );
	}
}
