using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Core.Settings
{
	[System.Serializable]
	public class Setting_ELowHigh : Setting__Base
	{
		public ELowHigh value;

		public Setting_ELowHigh( string n ) : base( n )
		{
		}

		public Setting_ELowHigh( string n, ELowHigh e ) : base( n )
		{
			value = e;
		}

		override protected Setting__Base CloneBase( )
		{
			return new Setting_ELowHigh( name, value );
		}
	}

	[System.Serializable]
	public class Setting_Enum < TEnumType > : Setting__Base
	{
		public TEnumType value = default(TEnumType);
		
		public Setting_Enum( string n ) : base(n)
		{
		}

		public Setting_Enum( string n, TEnumType e ) : base( n )
		{
			value = e;
		}

		override protected Setting__Base CloneBase( )
		{
			return new Setting_Enum< TEnumType >( name, value );
		}
	}

}
