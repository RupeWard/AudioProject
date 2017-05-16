using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RJWS.Core.Settings;

namespace RJWS.UI
{
	[System.Serializable]
	public class ScrollSettings : RJWS.Core.Settings.Settings__Base
	{
		public ScrollBarSettings horizontal = new ScrollBarSettings( "Horizontal");
		public ScrollBarSettings vertical = new ScrollBarSettings( "Vertical" );

		public bool linkedScaling = true;

		public ScrollSettings() : base ("ScrollSettings")
		{ }

		public ScrollBarSettings GetScrollBarSettings( EOrthoDirection dirn)
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
			return GetScrollBarSettings(dirn).position.value;
		}

		public float GetScrollBarWidth( EOrthoDirection dirn )
		{
			return GetScrollBarSettings( dirn ).width.value;
		}


		protected override Settings__Base CloneBase( )
		{
			ScrollSettings clone = new ScrollSettings( );
			clone.horizontal = horizontal.Clone<ScrollBarSettings>( );
			clone.vertical = vertical.Clone<ScrollBarSettings>( );
			clone.linkedScaling = linkedScaling;
			return clone;
		}

	}
}
