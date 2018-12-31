using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Audio
{
	[CreateAssetMenu( menuName = "RJWS/Guitar/Settings", order = 1000 )]
	public class GuitarSettings : ScriptableObject
	{
		public EPluckerType pluckerType = EPluckerType.BasicDrag;
		public bool useReverb = false;
		public float attenuation = Core.Audio.AudioConsts.DEFAULT_GUITAR_ATTENUATION;

		private const string DEFSETTINGSPATH = "DefaultGuitarSettings";

		static public GuitarSettings LoadDefaultsIfNUll(ref GuitarSettings gs)
		{
			if (gs == null)
			{
				gs = Resources.Load( DEFSETTINGSPATH ) as GuitarSettings;
				Debug.Log( "Loaded default guitar settings" );
			}
			return gs;
		}
	}
}
