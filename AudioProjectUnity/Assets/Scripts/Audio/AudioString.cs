﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Audio
{
	public class AudioString 
	{
		public const float MIN_FREQ = 1f;
		public const float MIN_LOWPASSFREQ = 100f;

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

		private float _openFrequency;
		private int _sampleRate;
		public Core.Audio.KSRingBufferF ringbuffer
		{
			get;
			private set;
		}

		private int _maxFret = 12;
	
		public struct CtorParams
		{
			public float openFrequency;
			public float attenuation;
			public int maxFret;
			public int sampleRate;
			public float zeroThreshold;
		}


		public AudioString(CtorParams cparams)
		{
			if (cparams.sampleRate <= 0)
			{
				cparams.sampleRate = DEFAULT_SAMPLE_RATE;
			}
			if (cparams.maxFret > 0)
			{
				_maxFret = cparams.maxFret;
			}
			if (cparams.attenuation< 0f)
			{
				cparams.attenuation = Core.Audio.AudioConsts.DEFAULT_GUITAR_ATTENUATION;
			}
			_openFrequency = cparams.openFrequency;
			_sampleRate = cparams.sampleRate;
			
			int c = Mathf.CeilToInt( (float)_sampleRate / _openFrequency );
			ringbuffer = new Core.Audio.KSRingBufferF(
				new Core.Audio.KSRingBufferF.CtorParams( )
				{
					capacity = c,
					attenuation = cparams.attenuation,
					zeroThreshold = cparams.zeroThreshold
				} 
			);
		}

		/*
		public AudioString( float[] b, float atten, int sr = -1)
		{
			if (sr < 0)
			{
				sr = DEFAULT_SAMPLE_RATE;
			}
			if (atten < 0f)
			{
				atten = Core.Audio.KSRingBufferF.DEFAULT_GUITAR_ATTENUATION;
            }
			
			_sampleRate = sr;
			ringbuffer = new Core.Audio.KSRingBufferF( b, atten);
			_frequency = (float)(_sampleRate) / ringbuffer.Capacity;
			TickCount = 0;
		}
		*/

		public void Pluck(int fret, float amplitude = 1f)
		{
			if (fret < 0)
			{
				Debug.LogErrorFormat( "fret OOR = {0}", fret);
				fret = 0;
			}
			else if (fret > _maxFret)
			{
				Debug.LogErrorFormat( "fret OOR = {0}", fret );
				fret = _maxFret;
			}
			if (amplitude <= 0)
			{
				Debug.LogErrorFormat( "amplitude = {0}", amplitude );
				amplitude = 1f;
			}
			ringbuffer.Clear( );

			float freq = _openFrequency;
			for (int i = 0; i < fret; i++)
			{
				freq *= Core.Audio.AudioConsts.FRET_FACTOR;
			}
			int c = Mathf.CeilToInt( (float)_sampleRate / freq );
			ringbuffer.Capacity = c;

			float min = -0.5f * amplitude;
			float max = -1f * min;

			while (!ringbuffer.IsFull)
			{
				ringbuffer.Enqueue( Random.Range( min, max ) );
			}
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
