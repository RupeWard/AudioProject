using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.UI
{
	[System.Serializable]
	public class ScrollSettings
	{
		public ScrollBarSettings horizontal = new ScrollBarSettings( "Horizontal");
		public ScrollBarSettings vertical = new ScrollBarSettings( "Vertical" );

		public bool linkedScaling = true;

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
			return GetScrollBarSettings(dirn).position;
		}

		public float GetScrollBarWidth( EOrthoDirection dirn )
		{
			return GetScrollBarSettings( dirn ).width;
		}

	}
}
