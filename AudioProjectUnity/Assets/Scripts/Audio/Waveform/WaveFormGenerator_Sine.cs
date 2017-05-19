using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Audio
{
	public class WaveFormGenerator_Sine : PeriodicWaveFormGenerator
	{
		const float TwoPi = 2f * Mathf.PI;

		private float _amplitude;
		
		public WaveFormGenerator_Sine( string n, float w, float a) : base( n, w )
		{
			_amplitude = a;
		}

		override public float GetValueForPhase( float phase )
		{
			return _amplitude * Mathf.Sin( phase * 2f * TwoPi );
		}

		override protected void DebugDescribeDetails( System.Text.StringBuilder sb )
		{
			sb.Append( "Sine, f=" ).Append( 1f / _wavelengthSecs ).Append( " a=" ).Append( _amplitude );
		}

	}

}
