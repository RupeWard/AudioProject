using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS
{
	public class AppManager : RJWS.Core.Singleton.SingletonApplicationLifetimeLazy< AppManager >
	{
		public const float MIN_CLICKABLE_mm = 7f;

		public float minClickablePixels
		{
			get;
			private set;
		}

		protected override void PostAwake( )
		{
			float minInches = MIN_CLICKABLE_mm / 25.4f;
			minClickablePixels = minInches * Screen.dpi;
		}


	}

}
