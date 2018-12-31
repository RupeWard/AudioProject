using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Core.Audio
{
	// Karplus-String
	// https://introcs.cs.princeton.edu/java/assignments/guitar.html

	public class KSRingBufferF : RingBuffer<float>
	{
		public float Amplitude
		{
			get
			{
				if (NumQueued > 0)
				{
					return _sum / (float)NumQueued;
				}
				return 0;
			}
		}

		private float _sum;

		private float _attenuation;
		public void SetAttenuation(float f)
		{
			if (f <= AudioConsts.MIN_GUITAR_ATTENUATION|| f >= 1f)
			{
				throw new System.Exception( string.Format("Attenuation out of range (" + AudioConsts.MIN_GUITAR_ATTENUATION + ", 1): {0}", f ));
			}
			_attenuation = f;
		}

		public KSRingBufferF(int c, float att )
			: base( c )
		{
			_sum = 0f;
			if (att <= 0f || att >= 1f)
			{
				throw new System.Exception( "Attenuation range error: " + att );
			}
			_attenuation = att;
		}

		/*
		public KSRingBufferF(float[] b, float att)
			:base( b )
		{
			if (att <= 0f || att >= 1f)
			{
				throw new System.Exception( "Attenuation range error: " + att );
			}
			_attenuation = att;
		}
		*/

		override public float Dequeue()
		{
			float result = base.Dequeue( );
			_sum -= Mathf.Abs( result);

			float next = Peek( );
			float newVal = _attenuation * 0.5f * (result + next);
            Enqueue( newVal );
			return result;
		}

		public override void Enqueue( float d )
		{
			base.Enqueue( d );
			_sum += Mathf.Abs( d );
		}

		public override void Clear( )
		{
			base.Clear( );
			_sum = 0f;
		}
	}

}
