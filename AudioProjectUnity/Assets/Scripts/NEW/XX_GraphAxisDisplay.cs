using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using RJWS.Core.Extensions;

public class XX_GraphAxisDisplay : MonoBehaviour
{
	public static readonly bool DEBUG_AXES = false;

	public Image axisImage;

	public RectTransform cachedRT
	{
		get;
		private set;
	}

	public XX_AxisDefn axisDefn
	{
		get;
		private set;
	}

	public RJWS.EOrthoDirection Direction
	{
		get { return axisDefn.eDirection; }
	}

	float SnapInRangeF( float v, Vector2 range, float snap)
	{
		v = SnapRoundF( v, snap );
		while (v < (range.x /*+ snap*/))
		{
			v += snap;
		}
		while (v > (range.y /*- snap*/))
		{
			v -= snap;
		}
		return v;
	}

	float SnapRoundF(float f, float snap)
	{
		float snapped = (f % snap) * snap;
		if ( f - snapped > 0.5f * snap)
		{
			snapped += snap;
		}
		return snapped;
	}

	public float Value
	{
		get
		{
			switch (axisDefn.axisType)
			{
				case XX_AxisDefn.EAxisType.FixedValue:
					{
						return axisDefn.value;
					}
				case XX_AxisDefn.EAxisType.ScreenFractionValue:
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
//						return SnapInRangeF( value, viewValueRange, axisDefn.snap );
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

	private List<GraphAxisTick> _ticks = new List<GraphAxisTick>( );

	protected XX_GraphViewPanel _graphViewPanel;
	public XX_GraphViewPanel graphViewPanel
	{
		get { return _graphViewPanel; }
	}

	private void Awake()
	{
		cachedRT = GetComponent<RectTransform>( );
		if (axisImage == null)
		{
			axisImage = GetComponent<UnityEngine.UI.Image>( );
		}
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

	public void Init( XX_GraphViewPanel p, XX_AxisDefn d )
	{
		axisDefn = d;

		gameObject.name = d.axisName;

		_graphViewPanel = p;

		cachedRT.SetParent(_graphViewPanel.axesContainer);
		transform.localScale = Vector3.one;
		
		SetSpriteSize(  );
		CreateTicks( );

		axisImage.color = _graphViewPanel.graphDisplaySettings.GetColor( axisDefn );
		adjustPosition( );
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
		foreach (GraphAxisTick tick in _ticks)
		{
			GameObject.Destroy( tick.gameObject );
		}
		_ticks.Clear( );
	}

	public void adjustPosition( )
	{
		switch (Direction)
		{
			case RJWS.EOrthoDirection.Horizontal:
				{
					cachedRT.anchoredPosition
						= new Vector2(
							graphViewPanel.GetXLocation(_graphViewPanel.xRange.MidPoint()),
							graphViewPanel.GetYLocation( Value )
							);
					break;
				}
			case RJWS.EOrthoDirection.Vertical:
				{
					cachedRT.anchoredPosition
						= new Vector2(
							_graphViewPanel.GetXLocation( Value ),
							graphViewPanel.GetYLocation( _graphViewPanel.yRange.MidPoint()) 
							);
					break;
				}
		}

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
									   graphViewPanel.graphDisplaySettings.defaultAxisWidth * axisDefn.axisLineWidth);

					break;
				}
			case RJWS.EOrthoDirection.Vertical:
				{
					cachedRT.sizeDelta
						= new Vector2(
									   graphViewPanel.graphDisplaySettings.defaultAxisWidth * axisDefn.axisLineWidth,
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
					break;
				}
			case RJWS.EOrthoDirection.Vertical:
				{
					axisImage.transform.localScale = new Vector2( screenFraction.x, 1f );
					break;
				}
			default:
				{
					Debug.LogError( "Unhandled direction " + Direction );
					break;
				}
		}
	}

	public void AdjustWidth(float scale)
	{
		if (Direction == RJWS.EOrthoDirection.Horizontal)
		{
			cachedRT.sizeDelta = new Vector2( cachedRT.sizeDelta.x, axisDefn.axisLineWidth / scale );
		}
		else if (Direction == RJWS.EOrthoDirection.Vertical)
		{
			cachedRT.sizeDelta = new Vector2( axisDefn.axisLineWidth / scale, cachedRT.sizeDelta.y );
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
