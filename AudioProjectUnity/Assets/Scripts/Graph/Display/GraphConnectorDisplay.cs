using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Graph.Display
{
	public class GraphConnectorDisplay : MonoBehaviour
	{
		public UnityEngine.UI.Image image;

		private RectTransform _imageRT;
		private Transform _imageTransform;

		private GraphDisplay _graphView;

		public RectTransform cachedRT
		{
			get;
			private set;
		}

		public Transform cachedTransform
		{
			get;
			private set;
		}

		public GraphPointDisplay previousPt = null;
		public GraphPointDisplay nextPt = null;

		private void Awake( )
		{
			cachedTransform = transform;
			cachedRT = GetComponent<RectTransform>( );
			_imageRT = image.GetComponent<RectTransform>( );
			_imageTransform = image.transform;
		}

		public void Init( GraphDisplay gvp, int num )
		{
			_graphView = gvp;
			cachedTransform.SetParent( _graphView.graphPanel.linesContainer );
			gameObject.name = "Connector_" + num.ToString( );
		}

		public void SetColour( Color c )
		{
			image.color = c;
		}

		public void UpdateDisplay( )
		{
			if (previousPt != null && nextPt != null)
			{
				Vector2 pos0 = previousPt.cachedRT.anchoredPosition;
				Vector2 pos1 = nextPt.cachedRT.anchoredPosition;

				float length = (pos0 - pos1).magnitude;
				float width = _graphView.graphDisplaySettings.lineWidth; // TODO scaling

				cachedRT.anchoredPosition = 0.5f * (pos0 + pos1);
				_imageRT.sizeDelta = new Vector2( length, width );
				_imageTransform.localEulerAngles = new Vector3( 0f, 0f, Mathf.Rad2Deg * Mathf.Atan2( pos1.y - pos0.y, pos1.x - pos0.x ) );
			}
		}
	}
}

