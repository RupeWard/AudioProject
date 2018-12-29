using UnityEngine;
using System;
using RJWS.Core.TransformExtensions;

//http://www.develop-online.net/tools-and-tech/procedural-audio-with-unity/0117433

namespace RJWS.Audio
{
	public class RingBufferGeneratorFilter : MonoBehaviour
	{
		public bool debugMe = true;

		private Core.Audio.RingBuffer<float> _ringBuffer = null;
//		private double _frequency;

//		private double _increment;
//		private double _phase;

//		private static double s_sampling_frequency = -1;

		public void Awake( )
		{
			/*
			if (s_sampling_frequency == -1)
			{
				s_sampling_frequency = AudioSettings.outputSampleRate;
				Debug.Log( "Audio output sampling rate is " + s_sampling_frequency.ToString( ) );
			}
			*/
		}

		public void Init( Core.Audio.RingBuffer<float> rb /*, float f */)
		{
			if (debugMe)
			{
				Debug.LogFormat( this, "Init: {0}", transform.GetPathInHierarchy( ) );
			}
			_ringBuffer = rb;
//			_frequency = (double)f;
		}

		/*
		public void SetFrequency( float f )
		{
			_frequency = (double)f;
		}
		*/

		void OnAudioFilterRead( float[] data, int channels )
		{
			//		Debug.Log (data.Length + " samples in " + channels + " channels");

			if (_ringBuffer != null)
			{

//				_increment = _frequency / s_sampling_frequency;
				for (int i = 0; i < data.Length; i = i + channels)
				{
//					_phase = _phase + _increment;

					data[i] = _ringBuffer.Dequeue( );

					// if we have stereo, we copy the mono data to each channel
					if (channels == 2)
					{
						data[i + 1] = data[i];
					}
//					if (_phase > 1f) _phase = 0;
				}
			}
		}

	}
	
}


