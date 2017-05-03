﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Graph
{
	public class GraphPanel: MonoBehaviour
	{
		public class ScrollBarSettings
		{
			public Dictionary<EOrthoDirection, ELowHigh> positions = new Dictionary<EOrthoDirection, ELowHigh>( )
			{
				{ EOrthoDirection.Horizontal, ELowHigh.Low},
				{ EOrthoDirection.Vertical, ELowHigh.High }
			};

			public Dictionary<EOrthoDirection, float> sizes = new Dictionary<EOrthoDirection, float>( )
			{
				{ EOrthoDirection.Horizontal, 40f},
				{ EOrthoDirection.Vertical, 60f }
			};

		}

		public ScrollBarSettings scrollBarSettings
		{
			get;
			private set;
		}

		private Dictionary<EOrthoDirection, GraphScrollBarPanel> _scrollBars = new Dictionary<EOrthoDirection, GraphScrollBarPanel>( );

		public RectTransform graphViewPanelRT;
		public Transform scrollBarContainer;

		public RectTransform cachedRT
		{
			private set;
			get;
		}

		private void Awake()
		{
			cachedRT = GetComponent<RectTransform>( );
			scrollBarSettings = new ScrollBarSettings( );
			SetUpScrollBars( );
		}

		private GameObject _scrollPrefab = null;

		private GraphScrollBarPanel GetScrollBar(EOrthoDirection dirn)
		{
			GraphScrollBarPanel result;
			if (false == _scrollBars.TryGetValue(dirn, out result))
			{
				if (_scrollPrefab == null)
				{
					_scrollPrefab = Resources.Load<GameObject>( "Graph/Prefabs/ScrollBarPanel" );
				}
				GameObject go = GameObject.Instantiate( _scrollPrefab );
				result = go.GetComponent<GraphScrollBarPanel>( );
				result.Init( this, dirn );
				_scrollBars[dirn] = result;
			}
			return result;
		}

		public void SetUpScrollBars()
		{
			foreach (EOrthoDirection edirn in System.Enum.GetValues( typeof( EOrthoDirection) ))
			{
				GetScrollBar(edirn).SetUp();
			}

			graphViewPanelRT.sizeDelta =
				new Vector2(
					cachedRT.rect.width - scrollBarSettings.sizes[EOrthoDirection.Vertical],
					cachedRT.rect.height - scrollBarSettings.sizes[EOrthoDirection.Horizontal] );
			Vector2 anchoredPos = Vector2.zero;
			if (scrollBarSettings.positions[EOrthoDirection.Horizontal] == ELowHigh.Low)
			{
				anchoredPos.y += 0.5f * scrollBarSettings.sizes[EOrthoDirection.Horizontal];
            }
			else
			{
				anchoredPos.y -= 0.5f * scrollBarSettings.sizes[EOrthoDirection.Horizontal];
			}
			if (scrollBarSettings.positions[EOrthoDirection.Vertical] == ELowHigh.Low)
			{
				anchoredPos.x += 0.5f * scrollBarSettings.sizes[EOrthoDirection.Vertical];
			}
			else
			{
				anchoredPos.x -= 0.5f * scrollBarSettings.sizes[EOrthoDirection.Vertical];
			}
			graphViewPanelRT.anchoredPosition = anchoredPos;

			ObjectGrabManager.Instance.OnScrollBarsSetUp( _scrollBars );
        }
	}

}
