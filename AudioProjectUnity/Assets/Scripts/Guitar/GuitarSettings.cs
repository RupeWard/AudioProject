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
		public Color dampedColour = Color.red;

		public Color fretMarkerColour = Color.blue;

		public Material bridgeMaterial;
		public Material fretMaterial;
		public Material stringMaterial;

		public Texture2D[] fretMarkerSprites;
		public Texture2D emptyMarkerSprite;
		public Texture2D dampedMarkerSprite;

		private const string PPKEY_PluckerType = "PluckerType";
		private const string PPKEY_UseReverb = "UseReverb";
		private const string PPKEY_Attenuation = "Attenuation";
		private const string PPKEY_StringColliderSize = "StringColliderSize";
		private const string PPKEY_ZeroThreshold = "ZeroThreshold";

		public void LoadFromPlayerPrefs()
		{
			if (PlayerPrefs.HasKey(PPKEY_PluckerType))
			{
				string s = PlayerPrefs.GetString( PPKEY_PluckerType );
				EPluckerType pt = (EPluckerType) System.Enum.Parse( typeof( EPluckerType ), s );
				pluckerType = pt;
			}
			else
			{
				Debug.LogWarning( "No PP saved" );
				return;
			}
			if (PlayerPrefs.HasKey( PPKEY_UseReverb ))
			{
				useReverb = (PlayerPrefs.GetInt( PPKEY_UseReverb ) == 1);
			}
			if (PlayerPrefs.HasKey( PPKEY_Attenuation ))
			{
				attenuation = PlayerPrefs.GetFloat( PPKEY_Attenuation);
			}
			if (PlayerPrefs.HasKey( PPKEY_StringColliderSize))
			{
				stringColliderSize= PlayerPrefs.GetFloat( PPKEY_StringColliderSize);
			}
			if (PlayerPrefs.HasKey( PPKEY_ZeroThreshold))
			{
				zeroThreshold = PlayerPrefs.GetFloat( PPKEY_ZeroThreshold);
			}
		}

		public void SaveToPlayerPrefs( )
		{
			PlayerPrefs.SetString( PPKEY_PluckerType, pluckerType.ToString() );
			PlayerPrefs.SetInt( PPKEY_UseReverb, (useReverb)?1:0);
			PlayerPrefs.SetFloat( PPKEY_Attenuation, attenuation );
			PlayerPrefs.SetFloat( PPKEY_StringColliderSize, stringColliderSize );
			PlayerPrefs.SetFloat( PPKEY_ZeroThreshold, zeroThreshold );

			PlayerPrefs.Save( );
		}

		private const string DEFSETTINGSPATH = "DefaultGuitarSettings";

		static public GuitarSettings LoadDefaultsIfNUll(ref GuitarSettings gs)
		{
			if (gs == null)
			{
				gs = Resources.Load( DEFSETTINGSPATH ) as GuitarSettings;
				Debug.Log( "Loaded default guitar settings" );

#if !UNITY_EDITOR
				gs.LoadFromPlayerPrefs( );
#endif
			}
			return gs;
		}

		public Texture2D FretMarker(int fret)
		{			
			if (fret < 0 || fret >= fretMarkerSprites.Length)
			{
				return dampedMarkerSprite;
			}
			return fretMarkerSprites[fret];
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
