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

		static public PluckSettings LoadDefaultsIfNUll(ref PluckSettings gs)
		{
			if (gs == null)
			{
				gs = Resources.Load( DEFSETTINGSPATH ) as PluckSettings;
				Debug.Log( "Loaded default pluck settings" );
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
