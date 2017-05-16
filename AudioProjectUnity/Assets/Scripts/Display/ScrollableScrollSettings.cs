using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RJWS.Core.Settings;

namespace RJWS.UI.Scrollable
{
	[System.Serializable]
	public class ScrollableScrollSettings : RJWS.Core.Settings.Settings__Base
	{
		public ScrollableScrollBarSettings horizontal = new ScrollableScrollBarSettings( "Horizontal");
		public ScrollableScrollBarSettings vertical = new ScrollableScrollBarSettings( "Vertical" );

		public bool linkedScaling = true;

		public ScrollableScrollSettings() : base ("ScrollSettings")
		{ }

		public ScrollableScrollBarSettings GetScrollBarSettings( EOrthoDirection dirn)
		{
			if (dirn == EOrthoDirection.Horizontal)
			{
				return horizontal;
			}
			else
			{
				return vertical;
			}
		}

		public ELowHigh GetScrollBarPosition( EOrthoDirection dirn )
		{
			return GetScrollBarSettings(dirn).position;
		}

		public float GetScrollBarWidth( EOrthoDirection dirn )
		{
			return GetScrollBarSettings( dirn ).width;
		}


		protected override Settings__Base CloneBase( )
		{
			ScrollableScrollSettings clone = new ScrollableScrollSettings( );
			clone.horizontal = horizontal.Clone<ScrollableScrollBarSettings>( );
			clone.vertical = vertical.Clone<ScrollableScrollBarSettings>( );
			clone.linkedScaling = linkedScaling;
			return clone;
		}

	}
}
