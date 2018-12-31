using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Core.Audio
{
	static public class AudioConsts 
	{
		public const float CONCERT_A = 440.0f;
		public const float FRET_FACTOR = 1.05956f;
        public static readonly float CONCERT_C = CONCERT_A * Mathf.Pow( FRET_FACTOR, 3.0f );
		public const float DEFAULT_GUITAR_ATTENUATION = 0.994f;
		public const float MIN_GUITAR_ATTENUATION = 0.9f;
		
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
