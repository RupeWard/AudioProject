using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RJWS.Core.Settings;

namespace RJWS.UI.Scrollable
{
	[System.Serializable]
	public class ScrollableScrollBarSettings : RJWS.Core.Settings.Settings__Base
	{
		public ELowHigh position = ELowHigh.High;
		public float width = 40f;
		public bool allowPositionChangeOnExternalZoom = false;
		public bool allowPositionChangeOnInternalZoom = false;

		public ScrollableScrollBarSettings(string n): base( n )
		{
		}

		protected override Settings__Base CloneBase()
		{
			ScrollableScrollBarSettings clone = new ScrollableScrollBarSettings( name );
			clone.position = position;
			clone.width = width;
			clone.allowPositionChangeOnExternalZoom = allowPositionChangeOnExternalZoom;
			clone.allowPositionChangeOnInternalZoom = allowPositionChangeOnInternalZoom;
			return clone;
		}
	}
}
