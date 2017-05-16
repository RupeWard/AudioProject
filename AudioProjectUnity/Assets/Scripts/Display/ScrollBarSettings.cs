using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RJWS.Core.Settings;

namespace RJWS.UI
{
	[System.Serializable]
	public class ScrollBarSettings : RJWS.Core.Settings.Settings__Base
	{
		public Setting_ELowHigh position = new Setting_ELowHigh( "position", ELowHigh.High);
		public Setting_Float width = new Setting_Float("width", 40f);

		public ScrollBarSettings(string n): base( n )
		{
		}

		protected override Settings__Base CloneBase()
		{
			ScrollBarSettings clone = new ScrollBarSettings( name );
			clone.position = position.Clone<Setting_ELowHigh>();
			clone.width = width.Clone<Setting_Float>();
			return clone;
		}
	}
}
