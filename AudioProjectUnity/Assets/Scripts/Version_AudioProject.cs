using UnityEngine;
using System.Collections;
using RJWS.Core.DebugDescribable;

namespace RJWS.Core.Version
{
	public static partial class Version
	{
		// Change this to match whenever updating version number in build settings
		// Put a 'D' at the start to make a Dev version 
		public static VersionNumber versionNumber
		{
			get
			{
				if (_versionNumber == null)
				{
					Build.BuildConfig buildConfig = Build.BuildConfig.LoadConfigFromResources( );
					_versionNumber = new VersionNumber( buildConfig.versionString);
					_versionNumber.subNumbers_[3] = buildConfig.buildNumber;
					Debug.LogWarningFormat( "Loaded version from config: {0}\nConfig=...\n{1}", _versionNumber.ToString( ), buildConfig.DebugDescribe( ) );
				}
				return _versionNumber;
			}
		}
		public static VersionNumber _versionNumber = null; 

	}
}