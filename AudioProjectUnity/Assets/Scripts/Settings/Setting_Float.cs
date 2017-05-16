using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Core.Settings
{
	[System.Serializable]
	public class Setting_Float : Setting__Base
	{
		public float value = 0f;

		public Setting_Float( string n ) : base(n)
		{
		}

		public Setting_Float( string n, float f ) : base( n )
		{
			value = f;
		}

		override protected Setting__Base CloneBase( )
		{
			return new Setting_Float( name, value );
		}
	}

}
