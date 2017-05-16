using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RJWS.Core.Settings;

namespace RJWS.UI
{
	[System.Serializable]
	public class ScrollBarSettings : RJWS.Core.Settings.Settings__Base
	{
		public ELowHigh position = ELowHigh.High;
		public float width = 40f;
		public bool allowPositionChangeOnExternalZoom = false;
		public bool allowPositionChangeOnInternalZoom = false;

		public ScrollBarSettings(string n): base( n )
		{
		}

		protected override Settings__Base CloneBase()
		{
			ScrollBarSettings clone = new ScrollBarSettings( name );
			clone.position = position;
			clone.width = width;
			return clone;
		}
	}
}
