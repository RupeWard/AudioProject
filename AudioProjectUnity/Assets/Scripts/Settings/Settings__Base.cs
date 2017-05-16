using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Core.Settings
{
	[System.Serializable]
	abstract public class Settings__Base
	{
		public string name
		{
			get;
			protected set;
		}

		public Settings__Base(string n)
		{
			name = n;
		}

		abstract protected Settings__Base CloneBase( );
		virtual public T Clone<T>( ) where T : Settings__Base
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
