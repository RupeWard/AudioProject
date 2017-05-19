using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Audio
{
	public class WaveFormGenerator_Saw: PeriodicWaveFormGenerator
	{
		private float _amplitude;

		public WaveFormGenerator_Saw( string n, float w, float a): base( n,w )
		{
			_amplitude = a;
		}

		override public float GetValueForPhase( float phase )
		{
			phase = phase - Mathf.Floor( phase );
			if (phase <= 0.25f)
			{
				return Mathf.Lerp( 0f, _amplitude, phase / 0.25f );
			}
			if (phase <= 0.75f)
			{
				return Mathf.Lerp( _amplitude, -1f * _amplitude, (phase - 0.25f) / (0.5f) );
			}
			return Mathf.Lerp( -1f * _amplitude, 0f, (phase - 0.75f) / 0.25f );
		}

		override protected void DebugDescribeDetails( System.Text.StringBuilder sb )
		{
			sb.Append( "Saw, f=" ).Append( 1f / _wavelengthSecs ).Append( " a=" ).Append( _amplitude );
		}

	}

}
