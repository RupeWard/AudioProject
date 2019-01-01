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

		private float _zeroThreshold = Mathf.Epsilon;

		private float _attenuation;
		public void SetAttenuation(float f)
		{
			if (f <= AudioConsts.MIN_GUITAR_ATTENUATION|| f >= 1f)
			{
				throw new System.Exception( string.Format("Attenuation out of range (" + AudioConsts.MIN_GUITAR_ATTENUATION + ", 1): {0}", f ));
			}
			_attenuation = f;
		}

		public struct CtorParams
		{
			public int capacity;
			public float attenuation;
			public float zeroThreshold;
		}

		public KSRingBufferF(CtorParams cparams )
			: base( cparams.capacity )
		{
			_zeroThreshold = Mathf.Epsilon;
			if (cparams.zeroThreshold > 0f)
			{
				if (cparams.zeroThreshold > 0.1f)
				{
					Debug.LogWarningFormat( "Invalid ZT: {0}", cparams.zeroThreshold );
				}
				else
				{
					_zeroThreshold = cparams.zeroThreshold;
				}
			}
			_sum = 0f;
			if (cparams.attenuation <= 0f || cparams.attenuation >= 1f)
			{
				throw new System.Exception( "Attenuation range error: " + cparams.attenuation);
			}
			_attenuation = cparams.attenuation;

			Debug.LogWarningFormat( "Created buffer with params: C={0}, A={1}, ZT={2}", cparams.capacity, cparams.attenuation, cparams.zeroThreshold );
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
			if (Mathf.Abs(newVal) <= _zeroThreshold)
			{
				newVal = 0f;
			}
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
