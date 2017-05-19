using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Audio
{
	abstract public class PeriodicWaveFormGenerator : AbstractWaveFormGenerator
	{
		protected float _wavelengthSecs;

		protected PeriodicWaveFormGenerator(string n, float w) : base( n )
		{
			_wavelengthSecs = w;
		}

		abstract public float GetValueForPhase( float phase );

		override public float GetValueForTimeSecs( float seconds )
		{
			float phase = (seconds % _wavelengthSecs) / _wavelengthSecs;
			return GetValueForPhase( phase );
		}

	}

}
