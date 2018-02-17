using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Graph.Display
{
	[System.Serializable]
	public class GraphDisplaySettings
	{
		public Color fractionalPointColour = Color.green;
		public Color samplePointColour = Color.red;

		public Color pureConnectorColour = Color.green;
		public Color sampleHidingConnectorColor = Color.red;

		public float lineWidth = 2f;
		public float pointSizeSampled = 5f;
		public float pointSizeFractional = 3.5f;

		public int numFractional = 30;
		public int numSampled = 15;

		public string pointPrefabPath = string.Empty;
		public string connectorPrefabPath = string.Empty;
	}
}

