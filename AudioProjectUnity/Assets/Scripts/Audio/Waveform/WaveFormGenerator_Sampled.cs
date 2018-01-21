using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RJWS.Core.DebugDescribable;

namespace RJWS.Audio
{
	public class WaveFormGenerator_Sampled: AbstractWaveFormGenerator
	{
		private float[] _samples;
		private double _sampleRate;

		private Vector2 _valueRange;

		override public Vector2 GetValueRange()
		{
			return _valueRange;
		}

		public double LengthSecs
		{
			get { return _sampleRate * (_samples.Length - 1);  }
		}

		public WaveFormGenerator_Sampled( string n, float[] samples, double sampleRate): base( n )
		{
			_sampleRate = sampleRate;
			_samples = new float[samples.Length];
			_valueRange.x = float.MaxValue;
			_valueRange.y = float.MinValue;
			for (int i = 0; i < samples.Length; i++)
			{
				float sample = samples[i];
				if (sample < _valueRange.x)
				{
					_valueRange.x = sample;
				}
				if (sample > _valueRange.y)
				{
					_valueRange.y = sample;
				}
				_samples[i] = sample;
			}
		}

		public override bool IsTimeValid( float seconds )
		{
			float indexF = (float)(seconds / _sampleRate);
			return (indexF >= 0f && indexF <= (float)(_samples.Length-1));
        }

		override public float GetValueForTimeSecs( float seconds )
		{
			float value = 0f;

			float indexF = (float) (seconds / _sampleRate);

			if (indexF < 0f || indexF > (float)(_samples.Length-1))
			{
				Debug.LogWarning( "Time " + seconds + " out of range: index="+indexF+": "+this.DebugDescribe() );
				if (indexF < 0f)
				{
					indexF = 0f;
				}
				else if (indexF > (float)(_samples.Length - 1))
				{
					indexF = (float)(_samples.Length - 1);
				}
			}

			int baseIndex = Mathf.FloorToInt( indexF );
			if (baseIndex >= _samples.Length - 1)
			{
				return _samples[_samples.Length - 1];
			}

			float fractionalPart = indexF - baseIndex;
			try
			{
				value = Mathf.Lerp( _samples[baseIndex], _samples[baseIndex + 1], fractionalPart );
			}
			catch (System.Exception ex)
			{
				Debug.LogError( "Index out of range: indexF=" + indexF + ", baseIndex=" + baseIndex + ", frac=" + fractionalPart + ": " + this.DebugDescribe( )+"\n"+ex.ToString() );
			}

			return value;
		}

		override protected void DebugDescribeDetails( System.Text.StringBuilder sb )
		{
			sb.Append( "Sampled, rate=" ).Append( _sampleRate ).Append( " n=" ).Append( _samples.Length).Append(", L=").Append(LengthSecs);
		}

	}

}
