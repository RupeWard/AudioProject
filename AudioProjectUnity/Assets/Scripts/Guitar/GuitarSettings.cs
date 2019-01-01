using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RJWS.Core.DebugDescribable;
using System;
using System.Text;

namespace RJWS.Audio
{
	[CreateAssetMenu( menuName = "RJWS/Guitar/Settings", order = 1000 )]
	public class GuitarSettings : ScriptableObject, IDebugDescribable
	{
		public EPluckerType pluckerType = EPluckerType.BasicDrag;
		public bool useReverb = false;
		public float attenuation = Core.Audio.AudioConsts.DEFAULT_GUITAR_ATTENUATION;
		public float stringColliderSize = 1f; // 0 - 1
		public float zeroThreshold = 0.01f;
		public float minToColourString = 0.0001f;

		public Color idleColour = Color.white;
		public Color minVolColour = Color.yellow;
		public Color maxVolColour = Color.red;

		public Material bridgeMaterial;
		public Material fretMaterial;
		public Material stringMaterial;

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

		public void DebugDescribe( StringBuilder sb )
		{
			sb.Append( "GuitarSettings:" );
			sb.Append( "\n- PluckerType = " ).Append( pluckerType );
			sb.Append( "\n- UseReverb = " ).Append( useReverb);
			sb.Append( "\n- Attenuation = " ).Append( attenuation );
			sb.Append( "\n---" );
		}
	}
}
