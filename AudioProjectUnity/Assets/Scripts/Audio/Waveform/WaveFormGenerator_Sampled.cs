using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RJWS.Core.DebugDescribable;

namespace RJWS.Audio
{
	public class WaveFormGenerator_Sampled: AbstractWaveFormGenerator
	{
		static readonly private bool DEBUG_LOCAL = false;

		private float[] _samples;
		private double _sampleRate;

		private Vector2 _valueRange;

		override public Vector2 GetValueRange( Vector2? xRange = null )
		{
			if (xRange == null)
			{
				return _valueRange;
			}

			// TODO implement
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

		public override bool IsTimeValid( double seconds )
		{
			double index = (seconds / _sampleRate);
			return (index >= 0 && index <= (double)(_samples.Length-1));
        }

		System.Text.StringBuilder _debugSB = new System.Text.StringBuilder( );

		public int GetSampleXsInInterval( double secondsStart, double length, List< double > sample, int max = int.MaxValue)
		{
			sample.Clear( );

			double firstIndex = (secondsStart / _sampleRate);
			int firstIndexI = (int)System.Math.Ceiling( firstIndex );

			if (DEBUG_LOCAL)
			{
				_debugSB.Length = 0;
				_debugSB.Append( "Getsamples in interval: S=" + secondsStart + ", L=" + length ).Append("\n-- ");
				DebugDescribe( _debugSB );
				_debugSB.Append( "\n-- first=" ).Append( firstIndex ).Append(" (").Append(firstIndexI).Append(")");
			}

			if (firstIndexI < 0 )
			{
				Debug.LogWarning( "Start Time " + secondsStart + " out of range: index=" + firstIndex+ ": " + this.DebugDescribe( ) );
				firstIndex = 0;
			}
			else if (firstIndexI > (_samples.Length - 1))
			{
				Debug.LogWarning( "Start Time " + secondsStart + " out of range: index=" + firstIndex + ": " + this.DebugDescribe( ) );
				return 0;
			}

			double lastIndex = ( (secondsStart + length) / _sampleRate);
			int lastIndexI = (int)System.Math.Floor( lastIndex );

			if (DEBUG_LOCAL)
			{
				_debugSB.Append( ", Last=" ).Append( lastIndex ).Append( " (" ).Append( lastIndexI ).Append( ")" );
			}
			if (lastIndexI > (_samples.Length - 1))
			{
				Debug.LogWarning( "EndTime " + secondsStart + " out of range: index=" + lastIndex+ ": " + this.DebugDescribe( ) );
				lastIndexI = (_samples.Length - 1);
			}
			if (lastIndex < 0 )
			{
				Debug.LogWarning( "EndTime " + secondsStart + " out of range: index=" + lastIndex + ": " + this.DebugDescribe( ) );
				return 0;
			}

			if (max != int.MaxValue)
			{
				if (lastIndexI - firstIndexI > max)
				{
					return 0;
				}
			}

			int numInInterval = 0;
			for (int index = firstIndexI; index <= lastIndexI; index++)
			{
				sample.Add( firstIndexI + index * _sampleRate );
				numInInterval++;
			}
			if (DEBUG_LOCAL)
			{
				_debugSB.Append( " N = " ).Append( numInInterval );
				Debug.Log( _debugSB.ToString( ) );
			}

			return numInInterval;
		}

		override public float GetValueForTimeSecs( double seconds )
		{
			float value = 0f;

			double index = (seconds / _sampleRate);

			if (index < 0 || index > (_samples.Length-1))
			{
				Debug.LogWarning( "Time " + seconds + " out of range: index="+index+": "+this.DebugDescribe() );
				if (index < 0)
				{
					index = 0;
				}
				else if (index > (double)(_samples.Length - 1))
				{
					index = (double)(_samples.Length - 1);
				}
			}

			int baseIndex = (int)System.Math.Floor( index );
			if (baseIndex >= _samples.Length - 1)
			{
				return _samples[_samples.Length - 1];
			}

			double fractionalPart = index - baseIndex;
			try
			{
				value = RJWS.MathsHelpers.FLerpD( _samples[baseIndex], _samples[baseIndex + 1], fractionalPart );
			}
			catch (System.Exception ex)
			{
				Debug.LogError( "Index out of range: index=" + index + ", baseIndex=" + baseIndex + ", frac=" + fractionalPart + ": " + this.DebugDescribe( )+"\n"+ex.ToString() );
			}

			return value;
		}

		override protected void DebugDescribeDetails( System.Text.StringBuilder sb )
		{
			sb.Append( "Sampled, rate=" ).Append( _sampleRate ).Append( " n=" ).Append( _samples.Length).Append(", L=").Append(LengthSecs);
		}

	}

}
