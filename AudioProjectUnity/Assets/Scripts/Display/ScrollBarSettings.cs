using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.UI
{
	[System.Serializable]
	public class ScrollBarSettings
	{
		public string name
		{
			get;
			private set;
		}
		public ELowHigh position = ELowHigh.High;
		public float width = 40f;

		public ScrollBarSettings(string s)
		{
			name = s;
		}
	}
}
