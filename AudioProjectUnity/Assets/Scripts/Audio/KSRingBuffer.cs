using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Core.Audio
{
	public class KSRingBufferF : RingBuffer<float>
	{
		public const float DEFAULT_GUITAR_ATTENUATION = 0.994f;

		private float _attenuation;

		public KSRingBufferF(int c, float att )
			: base( c )
		{
			if (att <= 0f || att >= 1f)
			{
				throw new System.Exception( "Attenuation range error: " + att );
			}
			_attenuation = att;
		}

		public KSRingBufferF(float[] b, float att)
			:base( b )
		{
			if (att <= 0f || att >= 1f)
			{
				throw new System.Exception( "Attenuation range error: " + att );
			}
			_attenuation = att;
		}

		override public float Dequeue()
		{
			float result = base.Dequeue( );
			float next = Peek( );
			Enqueue( _attenuation * 0.5f * (result + next) );
			return result;
		}
	}

}
