using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Core.Settings
{
	[System.Serializable]
	abstract public class Setting__Base 
	{
		public string name
		{
			get;
			protected set;
		}

		public Setting__Base( string n )
		{
			name = n;
		}

		abstract protected Setting__Base CloneBase( );
		virtual public T Clone< T >() where T : Setting__Base
		{
			T clone = CloneBase( ) as T;
			if (clone == null)
			{
				Debug.LogError( "Can't clone " + this.GetType( ) + " as " + typeof( T ) );
			}
			return clone;
		}
	}

}
