using UnityEngine;
using RJWS.Core.DebugDescribable;
using System;
using System.Text;

namespace RJWS.Audio
{
	[CreateAssetMenu( menuName = "RJWS/Guitar/PluckSettings", order = 1000 )]
	public class PluckSettings : ScriptableObject, IDebugDescribable
	{
		private const string DEFSETTINGSPATH = "DefaultPluckSettings";

		public Vector2 speedRange = new Vector2( 0f, 1000f );
		public Vector2 volumeRange = new Vector2( 0.1f, 1f );

		public float gamma = 0f;
		public bool useEnter = true;
		public bool useExit = true;

		private const string PPKEY_SpeedMin = "SpeedMin";
		private const string PPKEY_SpeedMax = "SpeedMax";
		private const string PPKEY_VolMin = "VolMin";
		private const string PPKEY_VolMax = "VolMax";
		private const string PPKEY_Gamma = "Gamma";
		private const string PPKEY_UseEnter = "UseEnter";
		private const string PPKEY_UseExit = "UseExit";

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
			if (PlayerPrefs.HasKey( PPKEY_VolMin))
			{
				volumeRange.x = PlayerPrefs.GetFloat( PPKEY_VolMin);
			}
			if (PlayerPrefs.HasKey( PPKEY_VolMax ))
			{
				volumeRange.y = PlayerPrefs.GetFloat( PPKEY_VolMax );
			}
			if (PlayerPrefs.HasKey( PPKEY_Gamma))
			{
				gamma = PlayerPrefs.GetFloat( PPKEY_Gamma);
			}

			if (PlayerPrefs.HasKey( PPKEY_UseEnter))
			{
				useEnter = (PlayerPrefs.GetInt( PPKEY_UseEnter) == 1);
			}
			if (PlayerPrefs.HasKey( PPKEY_UseExit))
			{
				useExit= (PlayerPrefs.GetInt( PPKEY_UseExit) == 1);
			}
		}

		public void SaveToPlayerPrefs( )
		{
			PlayerPrefs.SetFloat( PPKEY_SpeedMin, speedRange.x );
			PlayerPrefs.SetFloat( PPKEY_SpeedMax, speedRange.y );
			PlayerPrefs.SetFloat( PPKEY_VolMin, volumeRange.x );
			PlayerPrefs.SetFloat( PPKEY_VolMax, volumeRange.y );
			PlayerPrefs.SetFloat( PPKEY_Gamma, gamma );
		    PlayerPrefs.SetInt( PPKEY_UseEnter, (useEnter)?(1):(0));
			PlayerPrefs.SetInt( PPKEY_UseExit, (useExit) ? (1) : (0) );
		}

		static public PluckSettings LoadDefaultsIfNUll(ref PluckSettings gs)
		{
			if (gs == null)
			{
				gs = Resources.Load( DEFSETTINGSPATH ) as PluckSettings;
				Debug.Log( "Loaded default pluck settings" );

#if !UNITY_EDITOR
				gs.LoadFromPlayerPrefs( );
#endif
			}
			return gs;
		}

		public float GetVolumeForSpeed(float s)
		{
			float speedFactor = (s - speedRange.x) / (speedRange.y - speedRange.x);
			speedFactor = Mathf.Clamp01(speedFactor );
			if (gamma <= 0f)
			{
				return Mathf.Lerp( volumeRange.x, volumeRange.y, speedFactor );
			}
			else
			{
				return Mathf.Lerp( volumeRange.x, volumeRange.y, (Mathf.Exp( gamma * speedFactor ) - 1f) / (Mathf.Exp( gamma ) - 1) );
			}
		}

		public void DebugDescribe( StringBuilder sb )
		{
			sb.Append( "PluckSettings..." );
			sb.Append( "\n volRange = " ).Append( volumeRange.ToString( ) );
			sb.Append( "\n speedRange = " ).Append( speedRange.ToString( ) );
			sb.Append( "\n gamma = " ).Append( gamma);
			sb.Append( "\n useEnter = " ).Append( useEnter );
			sb.Append( "\n useExit = " ).Append( useExit );
			sb.Append( "\n---" );
		}
	}
}
