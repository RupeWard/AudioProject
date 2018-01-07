﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RJWS.Core.Extensions;

public class GraphPanel : MonoBehaviour
{
	private GameObject _graphPointDisplayPrefab_BACKING = null;
    private GameObject _graphPointDisplayPrefab
	{
		get
		{
			if (_graphPointDisplayPrefab_BACKING == null)
			{
				_graphPointDisplayPrefab_BACKING = Resources.Load<GameObject>( "Prefabs/Graph/GraphPoint" );
			}
			return _graphPointDisplayPrefab_BACKING;
		}
	}

	private GraphPointDisplay CreateGraphPoint( )
	{
		GraphPointDisplay newGpd = (GameObject.Instantiate( _graphPointDisplayPrefab )).GetComponent<GraphPointDisplay>( );
		newGpd.cachedRT.SetParent( pointsContainer );
		newGpd.cachedRT.anchoredPosition = Vector2.zero;
		newGpd.cachedTransform.localScale = Vector3.one;
		return newGpd;
	}

	private GameObject _graphLineDisplayPrefab_BACKING = null;
	private GameObject _graphLineDisplayPrefab
	{
		get
		{
			if (_graphLineDisplayPrefab_BACKING == null)
			{
				_graphLineDisplayPrefab_BACKING = Resources.Load<GameObject>( "Prefabs/Graph/GraphLine" );
			}
			return _graphLineDisplayPrefab_BACKING;
		}
	}

	private GraphLineDisplay CreateGraphLine()
	{
		GraphLineDisplay newGpd = (GameObject.Instantiate( _graphLineDisplayPrefab )).GetComponent<GraphLineDisplay>( );
		newGpd.cachedRT.SetParent( linesContainer );
		newGpd.cachedRT.anchoredPosition = Vector2.zero;
		newGpd.cachedTransform.localScale = Vector3.one;		
		return newGpd;
	}

	public RectTransform cachedRT
	{
		get;
		private set;
	}

	private void Awake()
	{
		cachedRT = GetComponent<RectTransform>( );
	}

	private Vector2 _displayScale = Vector2.one;

	public Vector2 displayScale
	{
		get
		{
			return _displayScale;
		}
	}

	public RectTransform axesContainer;
	public RectTransform linesContainer;
	public RectTransform pointsContainer;

	public GameObject axisPrefab;

	private List<GraphAxis> _axes = new List<GraphAxis>( );

	public Vector2 xRange
	{
		get { return _ranges[RJWS.EOrthoDirection.Horizontal]; }
		set { _ranges[RJWS.EOrthoDirection.Horizontal] = value; }
	}

	public Vector2 yRange
	{
		get { return _ranges[RJWS.EOrthoDirection.Vertical]; }
		set { _ranges[RJWS.EOrthoDirection.Vertical] = value; }
	}

	private Dictionary<RJWS.EOrthoDirection, Vector2> _ranges = new Dictionary<RJWS.EOrthoDirection, Vector2>( )
	{
		{  RJWS.EOrthoDirection.Horizontal, new Vector2()},
		{  RJWS.EOrthoDirection.Vertical, new Vector2()}
	};

    public void Init( RJWS.UI.Scrollable.ScrollablePanel scrollablePanel )
	{
		RectTransform parent = scrollablePanel.scrollablePanelView.contentPanelRT;
		cachedRT.SetParent( parent.transform);
		_displayScale.x = parent.transform.localScale.x;
		_displayScale.y = parent.transform.localScale.y;

		cachedRT.sizeDelta = parent.sizeDelta;

		scrollablePanel.scrollablePanelView.onScaleChangeAction += HandleDisplayScaleChanged;
	}

	public void HandleDisplayScaleChanged(RJWS.EOrthoDirection dirn, float scale)
	{
		if (dirn == RJWS.EOrthoDirection.Horizontal)
		{
			if (scale != _displayScale.x)
			{
				_displayScale.x = scale;
				foreach (GraphAxis axis in _axes)
				{
					if (axis.Direction == RJWS.EOrthoDirection.Vertical)
					{
						axis.AdjustWidth( scale );
					}
				}
			}
		}
		else if (dirn == RJWS.EOrthoDirection.Vertical)
		{
			if (scale != _displayScale.y)
			{
				_displayScale.y = scale;
				foreach (GraphAxis axis in _axes)
				{
					if (axis.Direction == RJWS.EOrthoDirection.Horizontal)
					{
						axis.AdjustWidth( scale );
					}
				}
			}
		}
	}

	public void DrawDefaultAxes()
	{
		foreach (GraphAxis ga in _axes)
		{
			GameObject.Destroy( ga.gameObject );
		}
		_axes.Clear( );

		AxisDefn xDefn = new AxisDefn( );
		xDefn.axisName = "XAxis";

		GraphAxis xAxis = GameObject.Instantiate( axisPrefab ).GetComponent< GraphAxis>();
		xAxis.init( this, xDefn );

		_axes.Add( xAxis );

		AxisDefn yDefn = new AxisDefn( );
		yDefn.axisName = "YAxis";
		yDefn.eDirection = RJWS.EOrthoDirection.Vertical;

		GraphAxis yAxis = GameObject.Instantiate( axisPrefab ).GetComponent<GraphAxis>( );
		yAxis.init( this, yDefn );

		_axes.Add( yAxis );
	}

	public static float LerpFree( float from, float to, float fraction )
	{
		return from + (to - from) * fraction;
	}

	public float GetXLocationForPoint( float xIn )
	{
		float xFraction = (xIn - xRange.x) / (xRange.y - xRange.x);
		return LerpFree( 0, cachedRT.sizeDelta.x, xFraction ); 
	}

	public float GetYLocationForPoint( float yIn )
	{
		float yFraction = (yIn - yRange.x) / (yRange.y - yRange.x);
		return LerpFree( 0, cachedRT.sizeDelta.y, yFraction );
	}

	public Vector2 GetLocationForPoint( float x, float y )
	{
		return new Vector2( GetXLocationForPoint( x ), GetYLocationForPoint( y ) );
	}

	public Vector2 GetLocationForPoint( Vector2 v )
	{
		return new Vector2( GetXLocationForPoint( v.x ), GetYLocationForPoint( v.y ) );
	}

	public float GetXLocationLerp( float x1, float x2, float fraction )
	{
		return GetXLocationForPoint( Mathf.Lerp( x1, x2, fraction ) );
	}

	public float GetXLocationLerp( Vector2 x, float fraction )
	{
		return GetXLocationLerp( x.x, x.y, fraction );
	}

	public float GetYLocationLerp( float y1, float y2, float fraction )
	{
		return GetYLocationForPoint( Mathf.Lerp( y1, y2, fraction ) );
	}

	public float GetYLocationLerp( Vector2 y, float fraction )
	{
		return GetYLocationLerp( y.x, y.y, fraction );
	}

	private HashSet<GraphPointDisplay> _graphPointDisplays = new HashSet<GraphPointDisplay>( );
	private HashSet<GraphLineDisplay> _graphLineDisplays = new HashSet<GraphLineDisplay>( );

	private RJWS.Grph.Graph _graph;
	public void DisplayGraph( RJWS.Grph.Graph graph )
	{
		ClearGraph( );

		_graph = graph;

		GraphPointDisplay previousPt = null;
		foreach (RJWS.Grph.GraphPoint gpt in graph.points)
		{
			GraphPointDisplay newPt = AddGraphPoint( gpt );
			AddGraphLine( previousPt, newPt );
			previousPt = newPt;
		}
	}

	private GraphLineDisplay AddGraphLine( GraphPointDisplay gpd0, GraphPointDisplay gpd1)
	{
		GraphLineDisplay newLine = null;
        if (gpd0 != null)
		{
			newLine = CreateGraphLine( );
			newLine.Init( this, gpd0, gpd1 );
			_graphLineDisplays.Add( newLine );
		}
		return newLine;
	}

	private GraphPointDisplay AddGraphPoint( RJWS.Grph.GraphPoint graphPoint)
	{
		GraphPointDisplay newDisplay = CreateGraphPoint( );
		newDisplay.Init( this, graphPoint );

		_graphPointDisplays.Add( newDisplay );
		return newDisplay;
	}

	private void ClearGraph()
	{
		// TODO
		foreach( GraphPointDisplay gpd in _graphPointDisplays)
		{
			GameObject.Destroy( gpd.gameObject );
		}
		_graphPointDisplays.Clear( );
		foreach (GraphLineDisplay gpd in _graphLineDisplays)
		{
			GameObject.Destroy( gpd.gameObject );
		}
		_graphLineDisplays.Clear( );
	}
}