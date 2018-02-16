using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using RJWS.Core.Extensions;
using RJWS.Core.DebugDescribable;

public class GraphAxisDisplay : MonoBehaviour
{
	public static readonly bool DEBUG_AXES = false;

	public Image axisImage;

	public UnityEngine.UI.Text valueText;
	public RectTransform valueLabelRT;

	public RectTransform cachedRT
	{
		get;
		private set;
	}

	public AxisDefn axisDefn
	{
		get;
		private set;
	}

	public RJWS.EOrthoDirection Direction
	{
		get { return axisDefn.eDirection; }
	}

	private Vector2 _fullLabelSize;

	/* What does snapping mean, really?

	float SnapInRangeF( float v, Vector2 range, float snap)
	{
		v = SnapRoundF( v, snap );
		while (v < (range.x + snap))
		{
			v += snap;
		}
		while (v > (range.y - snap))
		{
			v -= snap;
		}
		return v;
	}

	float SnapRoundF(float f, float snap)
	{
		float numSnaps = Mathf.Abs( f ) / snap;
		float snapBelow = numSnaps * snap * Mathf.Sign( f );
		if ( f < 0f)
		{
			snapBelow -= snap;
		}

		if (f - snapBelow > 0.5f * snap)
		{
			return snapBelow + snap;
		}
		else
		{
			return snapBelow;
		}
	}
*/

	public float Value
	{
		get
		{
			switch (axisDefn.axisType)
			{
				case AxisDefn.EAxisType.FixedValue:
					{
						return axisDefn.value;
					}
				case AxisDefn.EAxisType.ScreenFractionValue:
					{
						Vector2 viewValueRange;
						float value;
						switch (axisDefn.eDirection)
						{
							case RJWS.EOrthoDirection.Horizontal:
								{
									viewValueRange = new Vector2( _graphViewPanel.firstY, _graphViewPanel.lastY );
									value = Mathf.Lerp( _graphViewPanel.firstY, _graphViewPanel.lastY, axisDefn.value );
									break;
								}
							case RJWS.EOrthoDirection.Vertical:
								{
									viewValueRange = new Vector2( _graphViewPanel.firstX, _graphViewPanel.lastX );
									value = Mathf.Lerp( _graphViewPanel.firstX, _graphViewPanel.lastX, axisDefn.value );
									break;
								}
							default:
								{
									Debug.LogError( "Unhandled direction: " + axisDefn.eDirection );
									viewValueRange = new Vector2( 0f, 1f );
									value = 0f;
									break;
								}
						}
						return value;
					}
				default:
					{
						Debug.LogError( "Unhandled type: " + axisDefn.axisType);
						return axisDefn.value;
					}
			}

		}
	}

	public string AxisName
	{
		get { return axisDefn.axisName; }
	}

//	private List<GraphAxisTick> _ticks = new List<GraphAxisTick>( );

	protected GraphViewPanel _graphViewPanel;
	public GraphViewPanel graphViewPanel
	{
		get { return _graphViewPanel; }
	}

	public void SetValueText()
	{
		valueText.text = Value.ToString( "G3");
	}

	private void Awake()
	{
		cachedRT = GetComponent<RectTransform>( );
		if (axisImage == null)
		{
			axisImage = GetComponent<UnityEngine.UI.Image>( );
		}
	}

	private void OnDestroy()
	{
		GameObject.Destroy( valueLabelRT.gameObject );
	}

	private void OnEnable()
	{
		valueLabelRT.gameObject.SetActive( true );
	}

	private void OnDisable( )
	{
		valueLabelRT.gameObject.SetActive( false );
	}

	/*
	private Vector2 ViewRange( )
	{
		switch (Direction)
		{
			case EXYDirection.X:
				{
					return graph_.graphSettings.xView;
				}
			case EXYDirection.Y:
				{
					return graph_.graphSettings.yView;
				}
		}
		return Vector2.zero;
	}
	*/

	public static readonly bool DEBUG_AUTO = false;

	public bool IsVisible()
	{
		bool visible = true;

		Rect viewRect = _graphViewPanel._scrollablePanel.scrollablePanelView.ViewRect;

		float valueFraction;

		float val = Value;

		if (axisDefn.eDirection == RJWS.EOrthoDirection.Horizontal)
		{
			valueFraction = (val - _graphViewPanel.yRange.x) / (_graphViewPanel.yRange.y - _graphViewPanel.yRange.x);
			visible = (valueFraction >= viewRect.yMin) && (valueFraction <= viewRect.yMax);
		}
		else
		{
			valueFraction = (val - _graphViewPanel.xRange.x) / (_graphViewPanel.xRange.y - _graphViewPanel.xRange.x);
			visible = (valueFraction >= viewRect.xMin) && (valueFraction <= viewRect.xMax);
		}
		if (DEBUG_AUTO)
		{
			Debug.Log( "Visible = " + visible + " val = " + val + ", valueFraction = " + valueFraction + ", view = " + viewRect + ", AXIS=" + axisDefn.DebugDescribe( ) );
		}
		return visible;
	}

	public void Init( GraphViewPanel p, AxisDefn d )
	{
		axisDefn = d.clone();
		
		gameObject.name = d.axisName;
		valueLabelRT.gameObject.name = gameObject.name + " (ValueLabel)";
		_graphViewPanel = p;


		cachedRT.SetParent(_graphViewPanel.axesContainer);
		transform.localScale = Vector3.one;

		if (axisDefn.eDirection == RJWS.EOrthoDirection.Horizontal)
		{
			valueLabelRT.SetParent( _graphViewPanel.VerticalOverlaysPanel);
			valueLabelRT.anchorMin = new Vector2(0f, 0f);
			valueLabelRT.anchorMax = new Vector2(0f, 0f);
			valueLabelRT.pivot = new Vector2( 0f, 0.5f );
		}
		else
		{
			valueLabelRT.SetParent( _graphViewPanel.HorizontalOverlaysPanel);
			valueLabelRT.anchorMin = new Vector2( 0f, 0f );
			valueLabelRT.anchorMax = new Vector2( 0f, 0f );
			valueLabelRT.pivot = new Vector2( 0.5f, 0f );
		}
		valueLabelRT.localScale = Vector3.one;
		valueLabelRT.anchoredPosition = Vector2.zero;

		SetSpriteSize(  );
		CreateTicks( );

		axisImage.color = _graphViewPanel.graphDisplaySettings.GetColor( axisDefn );
		adjustPosition( );
		SetValueText( );

		_fullLabelSize = valueLabelRT.sizeDelta;
	}

	public void CreateTicks( )
	{
		/*
		ClearTicks( );

		float tickStart = _axisDefn.tickBase;

		Vector2 viewRange = ViewRange( );

		while (tickStart > viewRange.x)
		{
			tickStart -= definition_.tickSpacing;
		}
		float tickEnd = 1f;
		while (tickEnd < viewRange.y)
		{
			tickEnd += definition_.tickSpacing;
		}

		float value = tickStart;
		while (value <= tickEnd)
		{
			if (true)
			{
				// create tick 
				GraphTick newTick = null;

				switch (Direction)
				{
					case EXYDirection.X:
						{
							newTick = (GameObject.Instantiate( Resources.Load<GameObject>( "GUI/Prefabs/XTick" ) ) as GameObject).GetComponent<GraphTick>( );
							break;
						}
					case EXYDirection.Y:
						{
							newTick = (GameObject.Instantiate( Resources.Load<GameObject>( "GUI/Prefabs/YTick" ) ) as GameObject).GetComponent<GraphTick>( );
							break;
						}
				}

				// init it
				newTick.init( this, value, definition_.tickLabelSize );

				ticks_.Add( newTick );

			}
			value += definition_.tickSpacing;
		}*/
	}

	public void ClearTicks( )
	{
		/*
		foreach (GraphAxisTick tick in _ticks)
		{
			GameObject.Destroy( tick.gameObject );
		}
		_ticks.Clear( );
		*/
	}

	public void adjustPosition( )
	{
		Vector2 labelPos = Vector2.zero;
		Vector2 axisPos = Vector2.zero;

		switch (Direction)
		{
			case RJWS.EOrthoDirection.Horizontal:
				{
					axisPos = new Vector2(
							graphViewPanel.GetXLocation(_graphViewPanel.xRange.MidPoint()),
							graphViewPanel.GetYLocation( Value )
							);
					
					labelPos = new Vector2(
						0f,
//							graphViewPanel.GetXLocation(_graphViewPanel.lastX),
							graphViewPanel.GetYLocation( Value ));
					break;
				}
			case RJWS.EOrthoDirection.Vertical:
				{
					axisPos = new Vector2(
							_graphViewPanel.GetXLocation( Value ),
							graphViewPanel.GetYLocation( _graphViewPanel.yRange.MidPoint()) 
							);
					labelPos = new Vector2(
							_graphViewPanel.GetXLocation( Value ),
							0f
//							graphViewPanel.GetYLocation(_graphViewPanel.lastY)
							);
					break;
				}
		}
		cachedRT.anchoredPosition = axisPos;
        valueLabelRT.anchoredPosition = labelPos;

		SetValueText( );
		/*
		foreach (GraphTick t in ticks_)
		{
			t.adjustPosition( );
		}
		transform.localScale = Vector3.one;
		gameObject.SetActive( IsInView( ) );
		*/
	}


	public void SetSpriteSize( )
	{
		switch (Direction)
		{
			case RJWS.EOrthoDirection.Horizontal:
				{
					cachedRT.sizeDelta  
						= new Vector2(
									   graphViewPanel.cachedRT.sizeDelta.x,
									   graphViewPanel.graphDisplaySettings.defaultAxisWidth * axisDefn.axisLineWidthScaling);

					break;
				}
			case RJWS.EOrthoDirection.Vertical:
				{
					cachedRT.sizeDelta
						= new Vector2(
									   graphViewPanel.graphDisplaySettings.defaultAxisWidth * axisDefn.axisLineWidthScaling,
									   graphViewPanel.cachedRT.sizeDelta.y);
					break;
				}
		}

	}

	public void HandleScaling( Vector2 screenFraction )
	{
		switch(Direction)
		{
			case RJWS.EOrthoDirection.Horizontal:
				{
					axisImage.transform.localScale = new Vector2( 1f, screenFraction.y );
					valueLabelRT.sizeDelta = new Vector2( _fullLabelSize.x /* * screenFraction.x*/, _fullLabelSize.y * screenFraction.y );
					break;
				}
			case RJWS.EOrthoDirection.Vertical:
				{
					axisImage.transform.localScale = new Vector2( screenFraction.x, 1f );
					valueLabelRT.sizeDelta = new Vector2( _fullLabelSize.x * screenFraction.x, _fullLabelSize.y/* * screenFraction.y*/ );
					break;
				}
			default:
				{
					Debug.LogError( "Unhandled direction " + Direction );
					break;
				}
		}
		/*
		if (Direction == RJWS.EOrthoDirection.Horizontal)
		{
			valueLabelRT.sizeDelta = new Vector2( _fullLabelSize.x * screenFraction.y, _fullLabelSize.y * screenFraction.x  );
		}
		else
		{
			valueLabelRT.sizeDelta = new Vector2( _fullLabelSize.x * screenFraction.x, _fullLabelSize.y * screenFraction.y );
		}
		*/
//		valueLabelRT.sizeDelta = new Vector2( _fullLabelSize.x /** screenFraction.x*/, _fullLabelSize.y /** screenFraction.y*/ );
	}

	public void AdjustWidth(float scale)
	{
		if (Direction == RJWS.EOrthoDirection.Horizontal)
		{
			cachedRT.sizeDelta = new Vector2( cachedRT.sizeDelta.x, axisDefn.axisLineWidthScaling / scale );
		}
		else if (Direction == RJWS.EOrthoDirection.Vertical)
		{
			cachedRT.sizeDelta = new Vector2( axisDefn.axisLineWidthScaling / scale, cachedRT.sizeDelta.y );
		}
	}
	/*
	public bool IsInView( )
	{
		bool result = false;
		switch (Direction)
		{
			case EXYDirection.X:
				{
					result = graph_.graphSettings.IsYInView( Value );
					break;
				}
			case EXYDirection.Y:
				{
					result = graph_.graphSettings.IsXInView( Value );
					break;
				}
		}
		return result;
	}
	*/
}
