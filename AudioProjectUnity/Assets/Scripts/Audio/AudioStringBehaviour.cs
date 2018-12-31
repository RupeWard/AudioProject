using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RJWS.Core.TransformExtensions;

namespace RJWS.Audio
{
	public class AudioStringBehaviour : MonoBehaviour
	{
		public bool debugMe = true;

		public const float DEFAULT_LOWPASSFREQ = 5000f;

		private AudioString _string = null;

		public AudioSource audioSource;

		private RingBufferGeneratorFilter _generator;
		private AudioLowPassFilter _lowPassFilter;
		private AudioReverbFilter _reverbFilter;

		public System.Action<AudioStringBehaviour> onChangedAction;

		public float openFrequency = Core.Audio.AudioConsts.CONCERT_A;

		public float Frequency
		{
			get;
			private set;
		}

		public float Attenuation
		{
			get;
			private set;
		}

		private void Awake()
		{
	        Frequency = openFrequency;
			Attenuation = Core.Audio.AudioConsts.DEFAULT_GUITAR_ATTENUATION;

			if (audioSource == null)
			{
				audioSource = GetComponent<AudioSource>( );
			}
			if (audioSource == null)
			{
				Debug.LogErrorFormat( this, "No AudioSource on {0}", transform.GetPathInHierarchy( ) );
			}
			_generator = audioSource.GetComponent<RingBufferGeneratorFilter>( );
			_lowPassFilter = audioSource.GetComponent<AudioLowPassFilter>( );
			_reverbFilter = audioSource.GetComponent<AudioReverbFilter>( );
		}

		public void SetLowPassFrequency(float f)
		{
			_lowPassFilter.cutoffFrequency = f;
		}

		public float LowPassCutOffFrequency
		{
			get { return _lowPassFilter.cutoffFrequency; }
		}

		public void UseReverb(bool b)
		{
			_reverbFilter.enabled = b;
		}

		public bool UsingReverb
		{
			get
			{
				return _reverbFilter.enabled;
			}
		}

		public void SetFrequency( float f)
		{
			if (f < AudioString.MIN_FREQ)
			{
				Debug.LogErrorFormat("Frequency OOR: {0}", f);
			}
			else
			{
				Frequency = f;
				if (onChangedAction != null)
				{
					onChangedAction( this );
				}
			}
		}

		public void SetAttenuation(float f)
		{
			if (f < Core.Audio.AudioConsts.MIN_GUITAR_ATTENUATION || f >= 1f)
			{
				Debug.LogErrorFormat( this, "Atten OOR at {0}", f );
			}
			else
			{
				Attenuation = f;
			}
		}

		public void Pluck( float volume, int fret = 0, float f = -1, float atten = -1)
		{
			if (debugMe)
			{
				Debug.LogFormat( this, "{0}: Pluck( {1} ) ", gameObject.name, fret );
			}
			if (f != -1)
			{
				Frequency = f;
			}
			if (atten != -1)
			{
				Attenuation = atten;
			}
			Kill( );
			audioSource.volume = volume;
			audioSource.Play( );

			_string = new AudioString( Frequency, Attenuation );
			
			_string.Pluck(fret);
			_generator.Init( _string.ringbuffer );
		}

		private System.Text.StringBuilder _debugSB = new System.Text.StringBuilder( );

		public void Kill()
		{
			if (debugMe)
			{
				_debugSB.Length = 0;
				_debugSB.Append( "Kill:" );
			}

			if (audioSource.isPlaying)
			{
				if (debugMe)
				{
					_debugSB.Append( " Stop" );
				}
				audioSource.Stop( );
			}
			if (debugMe)
			{
				_debugSB.Append( " DONE - " ).Append(transform.GetPathInHierarchy());
				Debug.LogFormat( this, _debugSB.ToString( ) );
			}
		}
	}

}
