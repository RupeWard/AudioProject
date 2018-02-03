using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Audio
{
	public class WaveFormGenerator_Sine : PeriodicWaveFormGenerator
	{
		const float TwoPi = 2f * Mathf.PI;

		private float _amplitude;

		private Vector2 _valueRange;

		override public Vector2 GetValueRange( Vector2? xRange)
        {
			if (xRange == null)
			{
				return _valueRange;
			}

			// TODO implement
			return _valueRange;
		}

		public WaveFormGenerator_Sine( string n, double w, float a) : base( n, w )
		{
			_amplitude = a;
			_valueRange = new Vector2( -1f * _amplitude, _amplitude );
		}

		override public float GetValueForPhase( double phase )
		{
			return (float)(System.Math.Sin( phase * TwoPi ) * _amplitude);
		}

		override protected void DebugDescribeDetails( System.Text.StringBuilder sb )
		{
			sb.Append( "Sine, w=").Append(_wavelengthSecs).Append("s, f=" ).Append( 1f / _wavelengthSecs ).Append( " a=" ).Append( _amplitude );
		}

	}

}
