using UnityEngine;
using RJWS.Core.DebugDescribable;
using System;
using System.Text;

namespace RJWS.Audio
{
	[CreateAssetMenu( menuName = "RJWS/Guitar/PluckSettings", order = 1000 )]
	public class PluckSettings : ScriptableObject, IDebugDescribable
	{
		private const string SETTINGSPATH = "PluckSettings";

		public Vector2 durationRange = new Vector2( 0f, 0.3f );
		public Vector2 speedRange = new Vector2( 0f, 1000f );
		public Vector2 strumVolRange = new Vector2( 0.1f, 1f );
		public Vector2 pluckVolRange = new Vector2( 0.1f, 1f );

		public float strumGamma = 0f;
		public float pluckGamma = 0f;
		public bool useEnter = true;
		public bool useExit = true;

		public float slideVolumeIncreaseFraction = 0.25f;
		public float slideVolumeIncreasedMin = 0.2f;

		private const string PPKEY_DurationMin = "DurationMin";
		private const string PPKEY_DurationMax = "DurationMax";
		private const string PPKEY_SpeedMin = "SpeedMin";
		private const string PPKEY_SpeedMax = "SpeedMax";
		private const string PPKEY_StrumVolMin = "StrumVolMin";
		private const string PPKEY_StrumVolMax = "StrumVolMax";
		private const string PPKEY_PluckVolMin = "PluckVolMin";
		private const string PPKEY_PluckVolMax = "PluckVolMax";
		private const string PPKEY_StrumGamma = "StrumGamma";
		private const string PPKEY_PluckGamma = "PluckGamma";
		private const string PPKEY_UseEnter = "UseEnter";
		private const string PPKEY_UseExit = "UseExit";
		private const string PPKEY_SlideFraction = "SlideFraction";
		private const string PPKEY_SlideMin = "SlideMin";

		public void LoadFromPlayerPrefs( )
		{
			if (PlayerPrefs.HasKey( PPKEY_SpeedMin ))
			{
				speedRange.x = PlayerPrefs.GetFloat( PPKEY_SpeedMin );
			}
			else
			{
				Debug.LogWarning( "No PP saved" );
				return;
			}
			if (PlayerPrefs.HasKey( PPKEY_SpeedMax ))
			{
				speedRange.y = PlayerPrefs.GetFloat( PPKEY_SpeedMax );
			}
			if (PlayerPrefs.HasKey( PPKEY_DurationMin))
			{
				durationRange.x = PlayerPrefs.GetFloat( PPKEY_DurationMin);
			}
			if (PlayerPrefs.HasKey( PPKEY_DurationMax))
			{
				durationRange.y = PlayerPrefs.GetFloat( PPKEY_DurationMax);
			}
			if (PlayerPrefs.HasKey( PPKEY_StrumVolMin))
			{
				strumVolRange.x = PlayerPrefs.GetFloat( PPKEY_StrumVolMin);
			}
			if (PlayerPrefs.HasKey( PPKEY_StrumVolMax ))
			{
				strumVolRange.y = PlayerPrefs.GetFloat( PPKEY_StrumVolMax );
			}
			if (PlayerPrefs.HasKey( PPKEY_StrumGamma))
			{
				strumGamma = PlayerPrefs.GetFloat( PPKEY_StrumGamma);
			}

			if (PlayerPrefs.HasKey( PPKEY_PluckVolMin))
			{
				pluckVolRange.x = PlayerPrefs.GetFloat( PPKEY_PluckVolMin);
			}
			if (PlayerPrefs.HasKey( PPKEY_PluckVolMax))
			{
				strumVolRange.y = PlayerPrefs.GetFloat( PPKEY_PluckVolMax);
			}
			if (PlayerPrefs.HasKey( PPKEY_PluckGamma))
			{
				pluckGamma= PlayerPrefs.GetFloat( PPKEY_PluckGamma);
			}

			if (PlayerPrefs.HasKey( PPKEY_UseEnter))
			{
				useEnter = (PlayerPrefs.GetInt( PPKEY_UseEnter) == 1);
			}
			if (PlayerPrefs.HasKey( PPKEY_UseExit))
			{
				useExit= (PlayerPrefs.GetInt( PPKEY_UseExit) == 1);
			}
			if (PlayerPrefs.HasKey( PPKEY_SlideFraction))
			{
				slideVolumeIncreaseFraction = PlayerPrefs.GetFloat( PPKEY_SlideFraction);
			}
			if (PlayerPrefs.HasKey( PPKEY_SlideMin))
			{
				slideVolumeIncreasedMin= PlayerPrefs.GetFloat( PPKEY_SlideMin);
			}
		}

		public void SaveToPlayerPrefs( )
		{
			PlayerPrefs.SetFloat( PPKEY_SpeedMin, speedRange.x );
			PlayerPrefs.SetFloat( PPKEY_SpeedMax, speedRange.y );
			PlayerPrefs.SetFloat( PPKEY_DurationMin, durationRange.x );
			PlayerPrefs.SetFloat( PPKEY_DurationMax, durationRange.y );
			PlayerPrefs.SetFloat( PPKEY_StrumVolMin, strumVolRange.x );
			PlayerPrefs.SetFloat( PPKEY_StrumVolMax, strumVolRange.y );
			PlayerPrefs.SetFloat( PPKEY_StrumGamma, strumGamma );
		    PlayerPrefs.SetInt( PPKEY_UseEnter, (useEnter)?(1):(0));
			PlayerPrefs.SetInt( PPKEY_UseExit, (useExit) ? (1) : (0) );
			PlayerPrefs.SetFloat( PPKEY_SlideFraction, slideVolumeIncreaseFraction);
			PlayerPrefs.SetFloat( PPKEY_SlideMin, slideVolumeIncreasedMin);
		}

		static public PluckSettings LoadIfNUll(ref PluckSettings gs)
		{
			if (gs == null)
			{
				gs = Resources.Load( SETTINGSPATH ) as PluckSettings;
				Debug.Log( "Loaded pluck settings" );

#if !UNITY_EDITOR
				gs.LoadFromPlayerPrefs( );
#endif
			}
			return gs;
		}

		public float GetStrumVolumeForSpeed(float s)
		{
			float speedFactor = (s - speedRange.x) / (speedRange.y - speedRange.x);
			speedFactor = Mathf.Clamp01(speedFactor );
			if (Mathf.Approximately(strumGamma, 0f))
			{
				return Mathf.Lerp( strumVolRange.x, strumVolRange.y, speedFactor );
			}
			else
			{
				return Mathf.Lerp( strumVolRange.x, strumVolRange.y, (Mathf.Exp( strumGamma * speedFactor ) - 1f) / (Mathf.Exp( strumGamma ) - 1) );
			}
		}

		public float GetPluckVolumeForDuration( float s )
		{
			float durationFactor = (s - durationRange.x) / (durationRange.y - durationRange.x);
			durationFactor = Mathf.Clamp01( durationFactor );
			if (Mathf.Approximately( pluckGamma, 0f))
			{
				return Mathf.Lerp( pluckVolRange.y, pluckVolRange.x, durationFactor );
			}
			else
			{
				return Mathf.Lerp( pluckVolRange.y, pluckVolRange.x, (Mathf.Exp( pluckGamma* durationFactor ) - 1f) / (Mathf.Exp( strumGamma ) - 1) );
			}
		}

		public void DebugDescribe( StringBuilder sb )
		{
			sb.Append( "PluckSettings..." );
			sb.Append( "\n volRange = " ).Append( strumVolRange.ToString( ) );
			sb.Append( "\n speedRange = " ).Append( speedRange.ToString( ) );
			sb.Append( "\n volumeRange = " ).Append( durationRange.ToString( ) );
			sb.Append( "\n gamma = " ).Append( strumGamma);
			sb.Append( "\n useEnter = " ).Append( useEnter );
			sb.Append( "\n useExit = " ).Append( useExit );
			sb.Append( "\n---" );
		}
	}
}
