using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using RJWS.Core.Extensions;

public class GraphAxis : MonoBehaviour
{
	public static readonly bool DEBUG_AXES = false;

	public Image axisImage;

	public RectTransform cachedRT
	{
		get;
		private set;
	}

	private AxisDefn _axisDefn = null;

	public RJWS.EOrthoDirection Direction
	{
		get { return _axisDefn.eDirection; }
	}

	public float Value
	{
		get { return _axisDefn.value; }
	}

	public string AxisName
	{
		get { return _axisDefn.axisName; }
	}

	private List<GraphAxisTick> _ticks = new List<GraphAxisTick>( );

	protected GraphPanel _graphPanel;
	public GraphPanel graphPanel
	{
		get { return _graphPanel; }
	}

	private void Awake()
	{
		cachedRT = GetComponent<RectTransform>( );
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

	public void init( GraphPanel p, AxisDefn d )
	{
		_axisDefn = d;

		gameObject.name = d.axisName;

		_graphPanel = p;

		cachedRT.SetParent(_graphPanel.axesContainer);
		transform.localScale = Vector3.one;

		SetSpriteSize( _axisDefn.value );
		CreateTicks( );

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
							graphPanel.GetXLocationForPoint(_graphPanel.xRange.MidPoint()),
							graphPanel.GetYLocationForPoint( Value )
							);
					break;
				}
			case RJWS.EOrthoDirection.Vertical:
				{
					cachedRT.anchoredPosition
						= new Vector2(
							_graphPanel.GetXLocationForPoint( Value ),
							graphPanel.GetYLocationForPoint( _graphPanel.yRange.MidPoint()) 
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

	public void SetSpriteSize( float f )
	{
		switch (Direction)
		{
			case RJWS.EOrthoDirection.Horizontal:
				{
					cachedRT.sizeDelta  
						= new Vector2(
									   graphPanel.cachedRT.sizeDelta.x,
									   _axisDefn.axisLineWidth);

					break;
				}
			case RJWS.EOrthoDirection.Vertical:
				{
					cachedRT.sizeDelta
						= new Vector2(
									   _axisDefn.axisLineWidth,
									   graphPanel.cachedRT.sizeDelta.y);
					break;
				}
		}

	}

	public void AdjustWidth(float scale)
	{
		if (Direction == RJWS.EOrthoDirection.Horizontal)
		{
			cachedRT.sizeDelta = new Vector2( cachedRT.sizeDelta.x, _axisDefn.axisLineWidth / scale );
		}
		else if (Direction == RJWS.EOrthoDirection.Vertical)
		{
			cachedRT.sizeDelta = new Vector2( _axisDefn.axisLineWidth / scale, cachedRT.sizeDelta.y );
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
