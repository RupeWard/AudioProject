using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Graph.Display
{
	public class GraphPointDisplay : MonoBehaviour
	{
		private static readonly bool DEBUG_LOCAL = false;

		private GraphDisplay graphView;

		public UnityEngine.UI.Image image;
		public float size = 1;

		public enum EPtType
		{
			Fractional,
			Sampled
		}

		public EPtType PtType
		{
			get;
			private set;
		}

		public class PtXComparer : IComparer<GraphPointDisplay>
		{
			public int Compare( GraphPointDisplay pt0, GraphPointDisplay pt1 )
			{
				float x0 = pt0.Value.x;
				float x1 = pt1.Value.x;
				return (int)Mathf.Sign( x0 - x1 );
			}

		}

		private Vector2 _value = Vector2.zero;
		public Vector2 Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
				cachedRT.anchoredPosition = graphView.graphPanel.GetLocation( _value );
				if (DEBUG_LOCAL)
				{
					Debug.Log( gameObject.name + ": val = (" + _value.x + "," + _value.y + "), pos = " + cachedRT.anchoredPosition );
				}
			}
		}

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
		public GraphConnectorDisplay previousConnector = null;
		public GraphConnectorDisplay nextConnector = null;

		private float _xValue = float.NaN;
		public float xValue
		{
			get { return _xValue; }
			set
			{
				_xValue = value;
			}
		}

		private void Awake( )
		{
			cachedRT = GetComponent<RectTransform>( );
			cachedTransform = transform;
		}

		public void SetColour( Color c )
		{
			image.color = c;
		}

		public void Init( GraphDisplay gvp, string n, EPtType t )
		{
			graphView = gvp;
			PtType = t;
			cachedTransform.SetParent( graphView.graphPanel.pointsContainer );
			gameObject.name = "Point_" + n;

			if (PtType == EPtType.Fractional)
			{
				image.transform.localScale = graphView.graphDisplaySettings.pointSizeFractional * Vector3.one;
			}
			else
			{
				image.transform.localScale = graphView.graphDisplaySettings.pointSizeSampled * Vector3.one;
			}
		}

		public void HandleScaling( Vector2 screenFraction )
		{
			float ptSize = 0f;
			if (PtType == EPtType.Fractional)
			{
				ptSize = graphView.graphDisplaySettings.pointSizeFractional;
			}
			else
			{
				ptSize = graphView.graphDisplaySettings.pointSizeSampled;
			}
			image.transform.localScale = new Vector2( ptSize * size * screenFraction.x, ptSize * size * screenFraction.y );
		}
	}
}

