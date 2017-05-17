using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RJWS.Core.Settings;

namespace RJWS.UI.Scrollable
{
	[System.Serializable]
	public class ScrollablePanelSettings : Settings__Base
	{
		public ScrollableScrollSettings scrollSettings = new ScrollableScrollSettings( );
		public Vector2 contentSize = Vector2.zero;

		public ScrollablePanelSettings(): base ("GraphPanelSettings")
		{ }

		protected override Settings__Base CloneBase( )
		{
			ScrollablePanelSettings clone = new ScrollablePanelSettings( );
			clone.scrollSettings = scrollSettings.Clone<ScrollableScrollSettings>( );
			clone.contentSize = contentSize;
			return clone;
		}

	}


}
