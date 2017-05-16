using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RJWS.Core.Settings;

namespace RJWS.Graph
{
	[System.Serializable]
	public class GraphPanelSettings : Settings__Base
	{
		public UI.ScrollSettings scrollSettings = new UI.ScrollSettings( );
		public Vector2 contentSize = Vector2.zero;

		public GraphPanelSettings(): base ("GraphPanelSettings")
		{ }

		protected override Settings__Base CloneBase( )
		{
			GraphPanelSettings clone = new GraphPanelSettings( );
			clone.scrollSettings = scrollSettings.Clone<UI.ScrollSettings>( );
			clone.contentSize = contentSize;
			return clone;
		}

	}


}
