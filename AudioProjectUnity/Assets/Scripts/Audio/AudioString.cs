using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Audio
{
	public class AudioString 
	{
		private static int s_defaultSampleRate = -1;
		public static int DEFAULT_SAMPLE_RATE
		{
			get
			{
				if (s_defaultSampleRate < 0)
				{
					s_defaultSampleRate = AudioSettings.outputSampleRate;
				}
				return s_defaultSampleRate;
			}
		}

		private double _frequency;
		private int _sampleRate;
		public Core.Audio.RingBuffer<float> ringbuffer
		{
			get;
			private set;
		}

		public int TickCount
		{
			get;
			private set;
		}

		public AudioString(double f, int sr = -1)
		{
			if (sr == -1)
			{
				sr = DEFAULT_SAMPLE_RATE;
			}
			_frequency = f;
			_sampleRate = sr;
			int c = Mathf.CeilToInt( (float)_sampleRate / (float)f );
			ringbuffer = new Core.Audio.KSRingBufferF( c, Core.Audio.KSRingBufferF.DEFAULT_GUITAR_ATTENUATION );
			ringbuffer.Fill( 0f );
			TickCount = 0;
		}

		public AudioString( float[] b, int sr = -1)
		{
			if (sr == -1)
			{
				sr = DEFAULT_SAMPLE_RATE;
			}
			_sampleRate = sr;
			ringbuffer = new Core.Audio.KSRingBufferF( b, Core.Audio.KSRingBufferF.DEFAULT_GUITAR_ATTENUATION );
			_frequency = (float)(_sampleRate) / ringbuffer.Capacity;
			TickCount = 0;
		}

		public void Pluck(float amplitude = 1f)
		{
			if (amplitude <= 0)
			{
				Debug.LogErrorFormat( "amplitude = {0}", amplitude );
				amplitude = 1f;
			}
			ringbuffer.Clear( );

			float min = -0.5f * amplitude;
			float max = -1f * min;

			while (!ringbuffer.IsFull)
			{
				ringbuffer.Enqueue( Random.Range( min, max ) );
			}

			TickCount = 0;
		}

		/*
		public void Tick()
		{
			float v0 = _ringbuffer.Dequeue( );
			float v1 = _ringbuffer.Peek( );
			_ringbuffer.Enqueue( 0.5f * (v0 + v1) );
			TickCount++;
		}
		*/

	}
}
