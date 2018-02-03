using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Audio
{
	abstract public class PeriodicWaveFormGenerator : AbstractWaveFormGenerator
	{
		protected double _wavelengthSecs;
		public double waveLengthSecs
		{
			get { return _wavelengthSecs;  }
		}

		protected PeriodicWaveFormGenerator(string n, double w) : base( n )
		{
			_wavelengthSecs = w;
		}

		abstract public float GetValueForPhase( double phase );

		override public float GetValueForTimeSecs( double seconds )
		{
			double phase = (seconds % _wavelengthSecs) / _wavelengthSecs;
			return GetValueForPhase( phase );
		}

		public override bool IsTimeValid( double seconds )
		{
			return true;
		}
	}

}
