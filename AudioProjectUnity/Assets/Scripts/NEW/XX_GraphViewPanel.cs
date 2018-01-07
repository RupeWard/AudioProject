using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RJWS.Core.Extensions;

public class XX_GraphViewPanel : MonoBehaviour
{
	private bool _isDirty = false;
	public void SetDirty( )
	{
		_isDirty = true;
	}

	public GameObject graphPointPrefab;

	private List<XX_GraphPointDisplay> _graphPtDisplays = new List<XX_GraphPointDisplay>( );

	private int _numPointsBACKING = 0;
	public int numPoints
	{
		get { return _numPointsBACKING; }
		set
		{
			if (value != _numPointsBACKING)
			{
				_numPointsBACKING = value;
				HandleNumPointsChanged( );
			}
		}
	}

	private RJWS.Grph.AbstractGraphGenerator _graphGenerator;

	public void ChangeGraph( RJWS.Grph.AbstractGraphGenerator graphGenerator, int n )
	{
		numPoints = n;
		_graphGenerator = graphGenerator;
	}

	private void HandleNumPointsChanged( )
	{
		while (_graphPtDisplays.Count < numPoints)
		{
			XX_GraphPointDisplay newPoint = Instantiate( graphPointPrefab ).GetComponent<XX_GraphPointDisplay>( );
			_graphPtDisplays.Add( newPoint );
			SetDirty( );
		}
		while (_graphPtDisplays.Count > numPoints)
		{
			GameObject.Destroy( _graphPtDisplays[0].gameObject );
			_graphPtDisplays.RemoveAt( 0 );
			SetDirty( );
		}
		for (int i = 0; i < _graphPtDisplays.Count; i++)
		{
			_graphPtDisplays[i].Init( this, i );
			if (i == 0)
			{
				_graphPtDisplays[i].previousPt = null;
			}
			else
			{
				_graphPtDisplays[i].previousPt = _graphPtDisplays[i - 1];
			}
			if (i == _graphPtDisplays.Count - 1)
			{
				_graphPtDisplays[i].nextPt = null;
			}
			else
			{
				_graphPtDisplays[i].nextPt = _graphPtDisplays[i + 1];
			}
		}
	}

	private static readonly bool DEBUG_LOCAL = true;

	System.Text.StringBuilder debugsb = new System.Text.StringBuilder( );

	private void LateUpdate( )
	{
		if (_isDirty || _displayScaleDirty)
		{
			float xstep = (xRange.y - xRange.x) / (numPoints - 1);
			if (DEBUG_LOCAL)
			{
				debugsb.Length = 0;
				debugsb.Append( "Positioning GraphPoints. xRange = (" + xRange.x+", "+xRange.y + "), yRange = " + yRange + ", N = " + numPoints + ", xstep = " + xstep );
				if (_displayScaleDirty)
				{
					debugsb.Append( "\n - displayscale = " + _displayScale );
				}
			}
			for (int i = 0; i < numPoints; i++)
			{
				float x = xRange.x + xstep * i;
				float y = _graphGenerator.GetYForX( x );
				_graphPtDisplays[i].Value = new Vector2( x,y);
				if (_displayScaleDirty)
				{
					_graphPtDisplays[i].HandleScaling( _displayScale );
				}
			}
			_isDirty = false;
			_displayScaleDirty = false;
			if (DEBUG_LOCAL)
			{
				Debug.Log( debugsb.ToString( ) );
			}
		}
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
		scrollablePanel.scrollablePanelView.onViewChangeAction += HandleDisplayViewChanged;
	}

	private bool _displayScaleDirty= false;

	public void HandleDisplayViewChanged( RJWS.EOrthoDirection dirn, float scaleFraction, float posFraction)
	{
		HandleDisplayScaleChanged( dirn, scaleFraction );
	}

	public void HandleDisplayScaleChanged(RJWS.EOrthoDirection dirn, float scale)
	{
		if (DEBUG_LOCAL)
		{
			Debug.Log( "DisplayScaleChanged : " + dirn + ", " + scale );
		}
		if (dirn == RJWS.EOrthoDirection.Horizontal)
		{
			if (scale != _displayScale.x)
			{
				_displayScale.x = scale;
				_displayScaleDirty = true;
			}
		}
		else if (dirn == RJWS.EOrthoDirection.Vertical)
		{
			if (scale != _displayScale.y)
			{
				_displayScale.y = scale;
				_displayScaleDirty = true;
			}
		}
	}

	public static float LerpFree( float from, float to, float fraction )
	{
		return from + (to - from) * fraction;
	}

	public float GetXLocation( float xIn )
	{
		float xFraction = (xIn - xRange.x) / (xRange.y - xRange.x);
		return LerpFree( 0, cachedRT.sizeDelta.x, xFraction ); 
	}

	public float GetYLocation( float yIn )
	{
		float yFraction = (yIn - yRange.x) / (yRange.y - yRange.x);
		return LerpFree( 0, cachedRT.sizeDelta.y, yFraction );
	}

	public Vector2 GetLocationForPoint( float x, float y )
	{
		return new Vector2( GetXLocation( x ), GetYLocation( y ) );
	}

	public Vector2 GetLocationForPoint( Vector2 v )
	{
		return new Vector2( GetXLocation( v.x ), GetYLocation( v.y ) );
	}

	public float GetXLocationLerp( float x1, float x2, float fraction )
	{
		return GetXLocation( Mathf.Lerp( x1, x2, fraction ) );
	}

	public float GetXLocationLerp( Vector2 x, float fraction )
	{
		return GetXLocationLerp( x.x, x.y, fraction );
	}

	public float GetYLocationLerp( float y1, float y2, float fraction )
	{
		return GetYLocation( Mathf.Lerp( y1, y2, fraction ) );
	}

	public float GetYLocationLerp( Vector2 y, float fraction )
	{
		return GetYLocationLerp( y.x, y.y, fraction );
	}

	public Vector2 GetLocation( Vector2 v)
	{
		return new Vector2( GetXLocation( v.x ), GetYLocation( v.y ) );
	}
}
