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
			_imageRT.sizeDelta = Vector2.one;
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

				Vector2 scalingFraction = _graphView.graphPanel.displayScaleFractionReadonly;

				cachedRT.anchoredPosition = 0.5f * (pos0 + pos1);
				cachedRT.localScale = new Vector3( scalingFraction.x, scalingFraction.y, 1f );

				_imageRT.localScale = Vector3.one;// new Vector2( 1f / scalingFraction.x, 1f/scalingFraction.y);

				Vector2 lineVector = new Vector2( (pos1.x - pos0.x) * scalingFraction.y, ( pos1.y - pos0.y) * scalingFraction.x  );
				float angleDegrees = Mathf.Rad2Deg * Mathf.Atan2( lineVector.y, lineVector.x);
                _imageTransform.localEulerAngles = new Vector3( 0f, 0f, angleDegrees);

				Vector2 lineVectorNorm = new Vector2( Mathf.Abs(pos1.y - pos0.y) /* * scalingFraction.x*/, Mathf.Abs(pos1.x - pos0.x)/* *scalingFraction.y*/ );
				lineVectorNorm.Normalize();

				Vector2 invScaling = scalingFraction;// new Vector2( 1f / scalingFraction.x, 1f / scalingFraction.y );
				float xcomponentOfScaling =  Vector2.Dot( invScaling, lineVectorNorm );

				Vector2 norm = new Vector2( lineVectorNorm.y, lineVectorNorm.x );
				float ycomponentOfScaling = Vector2.Dot( invScaling, norm );
				//float diffAngleDegrees = Vector2.Angle( lineVector, scalingFraction );

				float length = (pos0 - pos1).magnitude / xcomponentOfScaling;// / Mathf.Abs( Mathf.Cos( diffAngleDegrees ));// scalingFraction.x;
				float width = _graphView.graphDisplaySettings.lineWidth / ycomponentOfScaling;// / Mathf.Abs( Mathf.Sin( diffAngleDegrees ));// scalingFraction.y; // TODO scaling
				_imageRT.sizeDelta = new Vector2( Mathf.Abs( length), Mathf.Abs(width) );

			}
			else
			{
				Debug.LogError( "Invalid GraphConnectorDisplay.UpdateDisdplay" );
			}
		}
	}
}

