﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Graph
{
	public class GraphScrollBar : MonoBehaviour
	{
		private Dictionary<ELowHigh, GraphScrollBarEnd> _ends = new Dictionary<ELowHigh, GraphScrollBarEnd>( );

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

			cachedRT.sizeDelta = scrollBarPanel.cachedRT.sizeDelta;
			_sizeRange.y = cachedRT.sizeDelta.x;
			
			foreach (ELowHigh eend in System.Enum.GetValues( typeof( ELowHigh ) ))
			{
				GameObject go = GameObject.Instantiate( endPrefab );
				_ends[eend] = go.GetComponent<GraphScrollBarEnd>( );
				_ends[eend].Init( this, eend );
				_sizeRange.x += _ends[eend].cachedRT.sizeDelta.x;
			}
		}

		public static readonly bool DEBUG_SCROLLBAR = true;

		public void HandleEndMoved( ELowHigh lowHigh, float delta)
		{
			if (DEBUG_SCROLLBAR)
			{
				Debug.Log( "SB moved: " + lowHigh + " " + delta );
			}

			Vector2 size = cachedRT.sizeDelta;
			Vector2 anchoredPos = cachedRT.anchoredPosition;

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

			if (size.x >= _sizeRange.x && size.x <= _sizeRange.y)
			{
				if (0.5f * _sizeRange.y + anchoredPos.x + 0.5f * size.x <= _sizeRange.y && 0.5f * _sizeRange.y + anchoredPos.x - 0.5f * size.x >= 0f)
				{
					cachedRT.sizeDelta = size;
					cachedRT.anchoredPosition = anchoredPos;
				}
			}
		}
	}

}
