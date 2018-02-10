using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RJWS.Core.Extensions;

public class XX_GraphViewPanel : MonoBehaviour
{
	private static readonly bool DEBUG_LOCAL = false;

	public void SetDirty( )
	{
		_displayPosDirty = true;
		_displayScaleDirty = true;
	}

	public bool IsDirty
	{
		get
		{
			return _displayScaleDirty || _displayPosDirty;
		}
	}

	public XX_GraphDisplaySettings graphDisplaySettings = new XX_GraphDisplaySettings( );

	public GameObject graphPointPrefab;
	public GameObject graphConnectorPrefab;
	public GameObject graphAxisPrefab;

	private List<XX_GraphPointDisplay> _graphPtDisplays = new List<XX_GraphPointDisplay>( );
	private List<XX_GraphConnectorDisplay> _graphConnectorDisplays = new List<XX_GraphConnectorDisplay>( );

	private List<XX_GraphAxisDisplay> _graphAxisDisplays = new List<XX_GraphAxisDisplay>( );

	private Dictionary<RJWS.EOrthoDirection, List<XX_GraphAxisDisplay> > _autoAxes = 
		new Dictionary<RJWS.EOrthoDirection, List<XX_GraphAxisDisplay> >( );
	private List<XX_GraphAxisDisplay> _allAutoAxes = new List<XX_GraphAxisDisplay>( );

	public bool autoAxisDisplays = true;

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

	private void ClearAxes()
	{
		for (int i = 0; i < _graphAxisDisplays.Count; i++)
		{
			GameObject.Destroy( _graphAxisDisplays[i].gameObject );
		}
		_graphAxisDisplays.Clear( );
	}

	private void ClearAutoAxes( )
	{
		foreach ( List<XX_GraphAxisDisplay> axes in _autoAxes.Values)
		{
			foreach(XX_GraphAxisDisplay axis in axes)
			{
				GameObject.Destroy( axis.gameObject );
			}
			axes.Clear( );
		}
		_autoAxes.Clear( );
		_allAutoAxes.Clear( );
	}


	private void CreateAutoAxes( IEnumerable<float> horizontal, IEnumerable<float> vertical)
	{
		ClearAutoAxes( );

		XX_AxisDefn axisDefn = new XX_AxisDefn( );

		axisDefn.axisType = XX_AxisDefn.EAxisType.ScreenFractionValue;

		axisDefn.eDirection = RJWS.EOrthoDirection.Horizontal;

		foreach (float f in horizontal)
		{
			axisDefn.value = f;
			axisDefn.axisName = "AUTO_H_" + f.ToString( );
			AddAutoAxis( axisDefn );
		}
		axisDefn.eDirection = RJWS.EOrthoDirection.Vertical;
		foreach (float f in vertical)
		{
			axisDefn.value = f;
			axisDefn.axisName = "AUTO_V_" + f.ToString( );
			AddAutoAxis( axisDefn );
		}
	}

	private RJWS.Grph.AbstractGraphGenerator _graphGenerator;

	public void ChangeGraph( RJWS.Audio.AbstractWaveFormGenerator graphGenerator, int n, Vector2 pxRange, bool clearAxes = true )
	{
		if (clearAxes)
		{
			ClearAxes( );
		}
		numPoints = n;
		_graphGenerator = graphGenerator;

		Vector2 yR = _graphGenerator.GetValueRange( );
		float yExtra = 0.1f * yR.magnitude;
		yR.x = yR.x - yExtra;
		yR.y = yR.y + yExtra;
		yRange = yR;

		xRange = pxRange;

		SetDirty( );
	}

	private XX_GraphAxisDisplay CreateAxis( XX_AxisDefn axisDefn )
	{
		XX_GraphAxisDisplay newAxisDisplay = Instantiate( graphAxisPrefab ).GetComponent<XX_GraphAxisDisplay>( );
		newAxisDisplay.Init( this, axisDefn );
		SetDirty( );
		return newAxisDisplay;
	}

	public void AddAxis(XX_AxisDefn axisDefn)
	{
		XX_GraphAxisDisplay newAxisDisplay = CreateAxis( axisDefn);
		_graphAxisDisplays.Add( newAxisDisplay );
	}

	public void AddAutoAxis( XX_AxisDefn axisDefn )
	{
		XX_GraphAxisDisplay newAxisDisplay = CreateAxis( axisDefn );
		if (false == _autoAxes.ContainsKey(axisDefn.eDirection))
		{
			_autoAxes.Add( axisDefn.eDirection, new List<XX_GraphAxisDisplay>( ) );
		}
		_autoAxes[axisDefn.eDirection].Add(newAxisDisplay );
		_allAutoAxes.Add( newAxisDisplay );
	}

	public void AddAxes( IEnumerable<XX_AxisDefn> defns)
	{
		foreach (XX_AxisDefn defn in defns)
		{
			AddAxis( defn );
		}
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
		while (_graphConnectorDisplays.Count < numPoints -1)
		{
			XX_GraphConnectorDisplay newConnector = Instantiate( graphConnectorPrefab ).GetComponent<XX_GraphConnectorDisplay>( );
			_graphConnectorDisplays.Add( newConnector );
			SetDirty( );
		}
		while (_graphConnectorDisplays.Count > numPoints -1)
		{
			GameObject.Destroy( _graphConnectorDisplays[0].gameObject );
			_graphConnectorDisplays.RemoveAt( 0 );
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
			if (i < _graphConnectorDisplays.Count)
			{
				_graphConnectorDisplays[i].Init( this, i );
				_graphConnectorDisplays[i].previousPt = _graphPtDisplays[i];
				_graphConnectorDisplays[i].nextPt = _graphPtDisplays[i + 1];
			}
		}
	}

	System.Text.StringBuilder debugsb = new System.Text.StringBuilder( );

	private Dictionary<RJWS.EOrthoDirection, bool> _directionFlags = null;
	private void ResetDirectionFlags(bool b)
	{
		if (_directionFlags == null)
		{
			_directionFlags = new Dictionary<RJWS.EOrthoDirection, bool>( );
		}
		_directionFlags.Clear( );
		_directionFlags.Add( RJWS.EOrthoDirection.Horizontal, b );
		_directionFlags.Add( RJWS.EOrthoDirection.Vertical, b );
	}

	private Rect viewValuesRect
	{
		get
		{
			return new Rect( firstX, firstY, (lastX - firstX), (lastY - firstY) );
		}
	}

	private void LateUpdate( )
	{
		if (!IsDirty)
			return;

		if (DEBUG_LOCAL)
		{
			debugsb.Length = 0;
			debugsb.Append( "Updated GraphPoints: VVR="+viewValuesRect.ToString()+", VR=" ).Append(_scrollablePanel.scrollablePanelView.ViewRect.ToString());
			
			if (_displayPosDirty)
			{
				debugsb.Append( " Pos" );
			}
			if (_displayScaleDirty)
			{
				debugsb.Append( " Scl" );
			}
			debugsb.Append( ", xRange = (" + xRange.x + ", " + xRange.y + "), yRange = " + yRange + ", N = " + numPoints );
		}

		RecalculatePos( );

		if (_displayScaleDirty)
		{
			if (DEBUG_LOCAL)
			{
				debugsb.Append( "\n- scale dirty = " + displayScaleReadonly );
			}
			for (int i = 0; i < numPoints; i++)
			{
				_graphPtDisplays[i].HandleScaling( displayScaleReadonly );
			}
			for (int i=0; i < _graphAxisDisplays.Count; i++)
			{
				_graphAxisDisplays[i].HandleScaling( displayScaleReadonly );
			}
			for (int i = 0; i < _allAutoAxes.Count; i++)
			{
				_allAutoAxes[i].HandleScaling( displayScaleReadonly );
			}
			_displayScaleDirty = false;
		}
		if (_displayPosDirty)
		{
			if (DEBUG_LOCAL)
			{
				debugsb.Append( "\n- pos dirty = " + displayPosOfFirstEdgeFractionReadonly+", first/last = "+firstX+" / "+lastX );
			}
			double xstepD = (lastXD- firstXD) / (numPoints - 1);
			if (DEBUG_LOCAL)
			{
				debugsb.Append( "\n- xstep = " + xstepD );
			}
			for (int i = 0; i < numPoints; i++)
			{
				double x = firstXD + xstepD * i;
				float y = _graphGenerator.GetYForX( x );
				_graphPtDisplays[i].Value = new Vector2( (float)x,y);
				if (i > 0)
				{
					_graphConnectorDisplays[i-1].UpdateDisplay( );
				}
			}

			ResetDirectionFlags(false );
			for (int i = 0; i < _graphAxisDisplays.Count; i++)
			{
				if (_graphAxisDisplays[i].axisDefn.axisType != XX_AxisDefn.EAxisType.FixedValue)
				{
					_graphAxisDisplays[i].adjustPosition( );
				}
				if (_graphAxisDisplays[i].IsVisible())
				{
					_directionFlags[_graphAxisDisplays[i].axisDefn.eDirection] = true;
				}
			}

			foreach (KeyValuePair<RJWS.EOrthoDirection, List< XX_GraphAxisDisplay>> kvp in _autoAxes )
			{
				bool showing = (false == _directionFlags[kvp.Key]);
				if (XX_GraphAxisDisplay.DEBUG_AUTO)
				{
					Debug.Log( kvp.Key.ToString( ) + " needs auto : " + showing );
				}

				foreach ( XX_GraphAxisDisplay axis in kvp.Value)
				{
					axis.gameObject.SetActive( showing);
					if (showing)
					{
						axis.adjustPosition( );
					}
				}
			}
			Rect viewValues = viewValuesRect;
			_scrollablePanel.SetLeftLabel( "MinX=" + viewValues.x.ToString("F4") + " (" + xRange.x.ToString( "F4" ) + ")" );
			_scrollablePanel.SetRightLabel( "MaxX=" + viewValues.xMax.ToString( "F4" ) + " (" + xRange.y.ToString( "F4" ) + ")" );
			_scrollablePanel.SetTopLabel( "MaxY=" + viewValues.yMax.ToString( "F2" ) + " (" + yRange.y.ToString( "F2" ) + ")" );
			_scrollablePanel.SetBottomLabel( "MinY=" + viewValues.y.ToString( "F2" ) + " (" + yRange.x.ToString( "F2" ) + ")" );
			_displayPosDirty = false;
		}
		if (DEBUG_LOCAL)
		{
			if (debugsb.Length > 0)
			{
				Debug.Log( debugsb.ToString( ) );
			}
		}
	}

	public float firstX
	{
		get
		{
			return xRange.x + (_displayPosOfFirstEdgeFraction[RJWS.EOrthoDirection.Horizontal] ) * ( xRange.y - xRange.x);
		}
	}

	public double firstXD
	{
		get
		{
			return (double)xRange.x + (double)(_displayPosOfFirstEdgeFraction[RJWS.EOrthoDirection.Horizontal] ) * (double)(xRange.y - xRange.x);
		}
	}


	public float lastX
	{
		get
		{
			return firstX + _displayScaleFraction[RJWS.EOrthoDirection.Horizontal] * (xRange.y - xRange.x);
		}
	}

	public double lastXD
	{
		get
		{
			return (double)firstX + (double)_displayScaleFraction[RJWS.EOrthoDirection.Horizontal] * (double)(xRange.y - xRange.x);
		}
	}

	public float firstY
	{
		get
		{
			return yRange.x + (_displayPosOfFirstEdgeFraction[RJWS.EOrthoDirection.Vertical] ) *( yRange.y - yRange.x);
		}
	}

	public double firstYD
	{
		get
		{
			return (double)yRange.x + (double)(_displayPosOfFirstEdgeFraction[RJWS.EOrthoDirection.Vertical]  ) * (double)(yRange.y - yRange.x);
		}
	}

	public float lastY
	{
		get
		{
			return firstY + _displayScaleFraction[RJWS.EOrthoDirection.Vertical] * (yRange.y - yRange.x);
		}
	}

	public double lastYD
	{
		get
		{
			return (double)firstY + (double)_displayScaleFraction[RJWS.EOrthoDirection.Vertical] * (double)(yRange.y - yRange.x);
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

	private void OnEnable()
	{
		SetDirty( );
	}

	public Vector2 displayScaleReadonly // TODO refactor
	{
		get
		{
			return new Vector2(_displayScaleFraction[RJWS.EOrthoDirection.Horizontal], _displayScaleFraction[RJWS.EOrthoDirection.Vertical]);
		}
	}

	public Vector2 displayPosOfFirstEdgeFractionReadonly // TODO refactor
	{
		get
		{
			return new Vector2( _displayPosOfFirstEdgeFraction[RJWS.EOrthoDirection.Horizontal], _displayPosOfFirstEdgeFraction[RJWS.EOrthoDirection.Vertical] );
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

	public RectTransform OverlaysPanel
	{
		get;
		private set;
	}

	public RectTransform HorizontalOverlaysPanel
	{
		get;
		private set;
	}

	public RectTransform VerticalOverlaysPanel
	{
		get;
		private set;
	}

	public RJWS.UI.Scrollable.ScrollablePanel _scrollablePanel;

	public void Init( RJWS.UI.Scrollable.ScrollablePanel scrollablePanel )
	{
		_scrollablePanel = scrollablePanel;

		OverlaysPanel = scrollablePanel.overlaysPanel;
		HorizontalOverlaysPanel = scrollablePanel.horizontalOverlaysPanel;
		VerticalOverlaysPanel = scrollablePanel.verticalOverlaysPanel;

		RectTransform parent = scrollablePanel.scrollablePanelView.contentPanelRT;
		cachedRT.SetParent( parent.transform);


		_scrollBarScaleFactor[RJWS.EOrthoDirection.Horizontal] = parent.transform.localScale.x;
		_scrollBarScaleFactor[RJWS.EOrthoDirection.Vertical] = parent.transform.localScale.y;

		_displayScaleFraction[RJWS.EOrthoDirection.Horizontal] = 1f / _scrollBarScaleFactor[RJWS.EOrthoDirection.Horizontal];
		_displayScaleFraction[RJWS.EOrthoDirection.Vertical] = 1f / _scrollBarScaleFactor[RJWS.EOrthoDirection.Vertical];

		cachedRT.sizeDelta = parent.sizeDelta;
		scrollablePanel.scrollablePanelView.onScaleChangeAction += HandleScrollScaleChanged;
		scrollablePanel.scrollablePanelView.onPosChangeAction += HandleScrollPosChanged;

		CreateAutoAxes( new float[] { 0.2f, 0.8f }, new float[] { 0.2f, 0.8f } );
	}

	private bool _displayScaleDirty= false;

	private Dictionary<RJWS.EOrthoDirection, float> _scrollBarPosOfCenterFraction = new Dictionary<RJWS.EOrthoDirection, float>( )
	{
		{ RJWS.EOrthoDirection.Horizontal, 0.5f },
		{ RJWS.EOrthoDirection.Vertical, 0.5f }
	};

	private Dictionary<RJWS.EOrthoDirection, float> _scrollBarScaleFactor = new Dictionary<RJWS.EOrthoDirection, float>( )
	{
		{ RJWS.EOrthoDirection.Horizontal, 1f },
		{ RJWS.EOrthoDirection.Vertical, 1f }
	};

	private Dictionary<RJWS.EOrthoDirection, float> _displayPosOfFirstEdgeFraction = new Dictionary<RJWS.EOrthoDirection, float>( )
	{
		{ RJWS.EOrthoDirection.Horizontal, 0f },
		{ RJWS.EOrthoDirection.Vertical, 0f }
	};

	private Dictionary<RJWS.EOrthoDirection, float> _displayScaleFraction = new Dictionary<RJWS.EOrthoDirection, float>( )
	{
		{ RJWS.EOrthoDirection.Horizontal, 1f },
		{ RJWS.EOrthoDirection.Vertical, 1f }
	};

	/*
	public void HandleDisplayViewChanged( RJWS.EOrthoDirection dirn, float scaleFraction, float posFraction)
	{
		HandleScrollScaleChanged( dirn, scaleFraction );
		HandleScrollPosChanged( dirn, posFraction);
	}
	*/

	public void HandleScrollScaleChanged( RJWS.EOrthoDirection dirn, float scaleFactor)
	{
		if (scaleFactor != _scrollBarScaleFactor[dirn])
		{
			_scrollBarScaleFactor[dirn] = scaleFactor;
			_displayScaleFraction[dirn] = 1f / scaleFactor;
			_displayScaleDirty = true;
			_displayPosDirty = true;
			if (dirn == RJWS.EOrthoDirection.Horizontal)
			{
				HorizontalOverlaysPanel.localScale = new Vector3( _scrollablePanel.scrollablePanelView.contentPanelRT.localScale.x, HorizontalOverlaysPanel.localScale.y, 1f );
				HorizontalOverlaysPanel.position = new Vector2( _scrollablePanel.scrollablePanelView.contentPanelRT.position.x, HorizontalOverlaysPanel.position.y );
			}
			if (dirn == RJWS.EOrthoDirection.Vertical)
			{
				VerticalOverlaysPanel.localScale = new Vector3( VerticalOverlaysPanel.localScale.x, _scrollablePanel.scrollablePanelView.contentPanelRT.localScale.y, 1f );
				VerticalOverlaysPanel.anchoredPosition = new Vector2( VerticalOverlaysPanel.anchoredPosition.x, _scrollablePanel.scrollablePanelView.contentPanelRT.anchoredPosition.y );
			}
			if (DEBUG_LOCAL)
			{
				Debug.Log( "GVP: scale factor " + dirn + " changed to " + scaleFactor+", fraction = "+_displayScaleFraction[dirn] );
			}
		}
	}

	public void HandleScrollPosChanged( RJWS.EOrthoDirection dirn, float posFraction)
	{
		if (posFraction != _scrollBarPosOfCenterFraction[dirn])
		{
			_scrollBarPosOfCenterFraction[dirn] = posFraction;
			_displayPosDirty = true;
			if (DEBUG_LOCAL)
			{
				Debug.Log( "GVP: scrollBarPosOfCenterFraction " + dirn + " changed to " + posFraction );
			}
		}
		if (dirn == RJWS.EOrthoDirection.Horizontal)
		{
			HorizontalOverlaysPanel.localScale = new Vector3( _scrollablePanel.scrollablePanelView.contentPanelRT.localScale.x, HorizontalOverlaysPanel.localScale.y, 1f );
			HorizontalOverlaysPanel.anchoredPosition = new Vector2( _scrollablePanel.scrollablePanelView.contentPanelRT.position.x, HorizontalOverlaysPanel.position.y );
		}
		if (dirn == RJWS.EOrthoDirection.Vertical)
		{
			VerticalOverlaysPanel.localScale = new Vector3( VerticalOverlaysPanel.localScale.x, _scrollablePanel.scrollablePanelView.contentPanelRT.localScale.y, 1f );
			VerticalOverlaysPanel.anchoredPosition = new Vector2( VerticalOverlaysPanel.anchoredPosition.x, _scrollablePanel.scrollablePanelView.contentPanelRT.anchoredPosition.y );
		}
	}

	private bool _displayPosDirty = false;

	private List<RJWS.EOrthoDirection> _dirnEnums = new List<RJWS.EOrthoDirection>( )
	{
		RJWS.EOrthoDirection.Horizontal,
		RJWS.EOrthoDirection.Vertical
	};

	public void RecalculatePos()
	{
		if (!IsDirty)
		{
			return;
		}

		foreach (RJWS.EOrthoDirection dirn in _dirnEnums)
		{
			float posFraction = _scrollBarPosOfCenterFraction[dirn];
			posFraction -= _displayScaleFraction[dirn] * 0.5f;
			if (posFraction != _displayPosOfFirstEdgeFraction[dirn])
			{
				if (DEBUG_LOCAL)
				{
					Debug.Log( "DisplayPos of left edge " + dirn + " " + posFraction + " with  scrollBarPos = "+_scrollBarPosOfCenterFraction[dirn] +" and scale fraction = "+_displayScaleFraction[dirn]);
				}
				_displayPosOfFirstEdgeFraction[dirn] = posFraction;
				_displayPosDirty = true;
			}
			else
			{
				if (DEBUG_LOCAL)
				{
					Debug.Log( "X DisplayPos " + dirn + " UNchanged : " + posFraction );
				}
			}
		}
	}

	/*
	public void HandleDisplayScaleChanged(RJWS.EOrthoDirection dirn, float scale)
	{
		scale = 1f / scale;
		if (dirn == RJWS.EOrthoDirection.Horizontal)
		{
			if (scale != _displayScale.x)
			{
				if (DEBUG_LOCAL)
				{
					Debug.Log( "DisplayScale changed : " + dirn + ", " + scale + " from "
						+ _displayScale.x );
				}
				_displayScale.x = scale;
				_displayScaleDirty = true;
			}
			else
			{
				if (DEBUG_LOCAL)
				{
					Debug.Log( "X DisplayScale UNchanged : " + dirn + ", " + _displayScale.x );
				}
			}
		}
		else if (dirn == RJWS.EOrthoDirection.Vertical)
		{
			if (scale != _displayScale.y)
			{
				if (DEBUG_LOCAL)
				{
					Debug.Log( "DisplayScale changed : " + dirn + ", " + scale + " from "
						+ _displayScale.y );
				}
				_displayScale.y = scale;
				_displayScaleDirty = true;
			}
			else
			{
				if (DEBUG_LOCAL)
				{
					Debug.Log( "X DisplayScale UNchanged : " + dirn + ", " + _displayScale.y );
				}
			}
		}
	}
	*/

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
