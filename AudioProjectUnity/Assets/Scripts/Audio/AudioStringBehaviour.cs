using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RJWS.Core.TransformExtensions;

namespace RJWS.Audio
{
	public class AudioStringBehaviour : MonoBehaviour
	{
		public bool debugMe = true;

		private AudioString _string = null;

		public AudioSource audioSource;

		private RingBufferGeneratorFilter _generator;
		
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
			_generator = audioSource.gameObject.AddComponent<RingBufferGeneratorFilter>( );
		}

		public void Pluck(float f)
		{
			if (debugMe)
			{
				Debug.LogFormat( this, "Pluck( {0} ) ", f );
			}
			Kill( );
			audioSource.Play( );

			_string = new AudioString( f );
			
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
			/*
			if (_generator != null)
			{
				if (debugMe)
				{
					_debugSB.Append( " Destroy" );
				}
				Component.Destroy( _generator );
				_generator = null;
			}
			*/
			if (debugMe)
			{
				_debugSB.Append( " DONE - " ).Append(transform.GetPathInHierarchy());
				Debug.LogFormat( this, _debugSB.ToString( ) );
			}
		}
	}

}
