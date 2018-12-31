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

		static public PluckSettings LoadDefaultsIfNUll(ref PluckSettings gs)
		{
			if (gs == null)
			{
				gs = Resources.Load( DEFSETTINGSPATH ) as PluckSettings;
				Debug.Log( "Loaded default pluck settings" );
			}
			return gs;
		}

		public void DebugDescribe( StringBuilder sb )
		{
			sb.Append( "PluckSettings..." );
			sb.Append( "\n---" );
		}
	}
}
