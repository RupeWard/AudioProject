﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Graph
{
	public class GraphScrollBar : MonoBehaviour
	{
		public static readonly bool DEBUG_SCROLLBAR = true;
		public static readonly bool DEBUG_MOVE = false;

		private Dictionary<ELowHigh, GraphScrollBarEnd> _ends = new Dictionary<ELowHigh, GraphScrollBarEnd>( );
		private GraphScrollBarMiddle _middle;

		public System.Action<EOrthoDirection, float, float> onScrollBarChanged; // size, pos as fractions

		public RectTransform cachedRT
		{
			private set;
			get;
		}

		public GraphScrollBarPanel scrollBarPanel
		{
			get;
			private set;
		}

		private void Awake()
		{
			cachedRT = GetComponent<RectTransform>( );
		}

		private Vector2 _sizeRange;

		public void Init( GraphScrollBarPanel sbp)
		{
			scrollBarPanel = sbp;
			GameObject endPrefab = Resources.Load<GameObject>( "Graph/Prefabs/ScrollBarEnd" );

			Vector2 size = scrollBarPanel.cachedRT.sizeDelta;
			size.y -= 10f;
			cachedRT.sizeDelta = size;

			_sizeRange.y = cachedRT.sizeDelta.x;
			
			foreach (ELowHigh eend in System.Enum.GetValues( typeof( ELowHigh ) ))
			{
				GameObject go = GameObject.Instantiate( endPrefab );
				_ends[eend] = go.GetComponent<GraphScrollBarEnd>( );
				_ends[eend].Init( this, eend );
				_sizeRange.x += _ends[eend].cachedRT.sizeDelta.x;
			}

			_ends[ELowHigh.Low].otherEnd = _ends[ELowHigh.High];
			_ends[ELowHigh.High].otherEnd = _ends[ELowHigh.Low];

			GameObject middlePrefab = Resources.Load<GameObject>( "Graph/Prefabs/ScrollBarMiddle" );
			GameObject mgo = GameObject.Instantiate( middlePrefab );
			_middle = mgo.GetComponent<GraphScrollBarMiddle>( );
			_middle.Init( this );
			_sizeRange.x += _middle.cachedRT.sizeDelta.x;
        }

		public bool SetSizeFraction(float fraction)
		{
			if (fraction < 0f || fraction > 1f)
			{
				throw new System.Exception( "Size Fraction out of range: " + fraction );
			}
			float newSize = fraction * _sizeRange.y;
			if (newSize < _sizeRange.x || newSize > _sizeRange.y)
			{
				Debug.LogWarning( "Scrollbar new size " + newSize + " for fraction " + fraction + " out of range " + _sizeRange );
				return false;
			}
			cachedRT.sizeDelta = new Vector2( newSize, cachedRT.sizeDelta.y );
			DoScrollBarChangedAction( );
			return true;
		}

		public void HandleMiddleMoved( float delta )
		{
			if (DEBUG_MOVE)
			{
				Debug.Log( "SB Middle moved: " + delta );
			}

			Vector2 size = cachedRT.sizeDelta;
			Vector2 anchoredPos = cachedRT.anchoredPosition;

			anchoredPos.x += delta;

			if (0.5f * _sizeRange.y + anchoredPos.x + 0.5f * size.x <= _sizeRange.y && 0.5f * _sizeRange.y + anchoredPos.x - 0.5f * size.x >= 0f)
			{
				cachedRT.anchoredPosition = anchoredPos;
			}

			DoScrollBarChangedAction( );
		}

		public void HandleEndMoved( ELowHigh lowHigh, float delta, bool doubleEnded)
		{
			if (DEBUG_MOVE)
			{
				Debug.Log( Time.time + " SB End moved: " + lowHigh + " " + delta );
			}

			Vector2 size = cachedRT.sizeDelta;
			Vector2 anchoredPos = cachedRT.anchoredPosition;

			if (doubleEnded)
			{
				if (lowHigh == ELowHigh.Low)
				{
					size.x -= 2f * delta;
				}
				else
				{
					size.x += 2f * delta;
				}
			}
			else
			{
				if (lowHigh == ELowHigh.Low)
				{
					size.x -= delta;
					anchoredPos.x += 0.5f * delta;
				}
				else
				{
					size.x += delta;
					anchoredPos.x += 0.5f * delta;
				}
			}

			bool canChange = false;

			if (size.x >= _sizeRange.x && size.x <= _sizeRange.y)
			{
				if (0.5f * _sizeRange.y + anchoredPos.x + 0.5f * size.x <= _sizeRange.y && 0.5f * _sizeRange.y + anchoredPos.x - 0.5f * size.x >= 0f)
				{
					canChange = true;
				}
			}

			if (canChange && scrollBarPanel.graphPanel.scrollBarSettings.scaleInBothDirections)
			{
				canChange = scrollBarPanel.graphPanel.GetScrollBar( scrollBarPanel.eDirection.OtherDirection( ) ).scrollBar.SetSizeFraction( size.x/_sizeRange.y );
			}

			if (canChange)
			{
				cachedRT.sizeDelta = size;
				cachedRT.anchoredPosition = anchoredPos;
				DoScrollBarChangedAction( );
			}

		}

		private void DoScrollBarChangedAction()
		{
			if (onScrollBarChanged != null)
			{
				float sizeFraction = cachedRT.sizeDelta.x / _sizeRange.y;
				float posFraction = (cachedRT.anchoredPosition.x + 0.5f * _sizeRange.y) / _sizeRange.y;

				if (sizeFraction > 1f || sizeFraction < 0f)
				{
					Debug.LogError( "Bad sizeFraction: " + sizeFraction );
					sizeFraction = Mathf.Clamp01( sizeFraction );
				}
				if (posFraction > 1f || posFraction < 0f)
				{
					Debug.LogError( "Bad posFraction: " + posFraction );
					posFraction = Mathf.Clamp01( posFraction );
				}
				onScrollBarChanged( scrollBarPanel.eDirection, sizeFraction, posFraction );
            }
			else
			{
				Debug.LogError( "No onScrollBarChanged!" );
			}
		}
	}

}
