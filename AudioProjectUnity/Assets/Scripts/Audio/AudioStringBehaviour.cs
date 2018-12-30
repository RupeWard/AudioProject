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
		
		private void Awake()
		{
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

		public void UserReverb(bool b)
		{
			_reverbFilter.enabled = b;
		}

		public void Pluck(float f, float atten = -1)
		{
			if (debugMe)
			{
				Debug.LogFormat( this, "Pluck( {0} ) ", f );
			}
			Kill( );
			audioSource.Play( );

			_string = new AudioString( f, atten );
			
			_string.Pluck( );
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
