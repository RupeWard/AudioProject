using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Audio
{
	public interface IWaveFormProvider 
	{
		float GetValueForTime( float time);
		string WaveFormName( );
	}
}
