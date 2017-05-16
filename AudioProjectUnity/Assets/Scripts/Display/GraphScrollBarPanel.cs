﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RJWS.Core.UI;

namespace RJWS.Graph
{
	public class GraphScrollBarPanel : MonoBehaviour
	{
		public GraphScrollBar scrollBar;

		public RectTransform cachedRT
		{
			private set;
			get;
		}

		public GraphPanel graphPanel
		{
			get;
			private set;
		}

		public EOrthoDirection eDirection
		{
			private set;
			get;
		}

		public ELowHigh ePosition
		{
			private set;
			get;
		}

		public RJWS.UI.ScrollBarSettings settings
		{
			get;
			private set;
		}

		private void Awake()
		{
			cachedRT = GetComponent<RectTransform>( );
		}

		public void Init( GraphPanel p, EOrthoDirection ed, RJWS.UI.ScrollBarSettings sets )
		{
			settings = sets;

			graphPanel = p;
			eDirection = ed;
			gameObject.name = "ScrollBarPanel_" + eDirection.ToString( );

			cachedRT.SetParent( graphPanel.scrollBarContainer.transform );
			cachedRT.localScale = Vector3.one;

			scrollBar.onScrollBarChanged += graphPanel.HandleScrollBarChanged;
		}

		public void SetUp()
		{ 
			ePosition = graphPanel.graphPanelSettings.scrollSettings.GetScrollBarPosition(eDirection);

			ELowHigh otherPosition = graphPanel.graphPanelSettings.scrollSettings.GetScrollBarPosition( eDirection.OrthogonalDirection( ));

			float sbWidth = graphPanel.graphPanelSettings.scrollSettings.GetScrollBarWidth(eDirection);
			float otherWidth = graphPanel.graphPanelSettings.scrollSettings.GetScrollBarWidth( eDirection.OrthogonalDirection() );

			switch (eDirection)
			{
				case EOrthoDirection.Horizontal:
					{
						cachedRT.sizeDelta = new Vector2( graphPanel.cachedRT.rect.width - otherWidth, sbWidth );
						scrollBar.Init( this );

						cachedRT.rotation = Quaternion.Euler( 0f, 0f, 0f );

						Vector2 anchorV = new Vector2( 0.5f, 0f ); // anchors for low position
						if (ePosition == ELowHigh.High)
						{
							anchorV.y = 1f;
						}
						cachedRT.anchorMin = anchorV;
						cachedRT.anchorMax = anchorV;
						cachedRT.pivot = anchorV;

						if (otherPosition == ELowHigh.High)
						{
							cachedRT.anchoredPosition = new Vector2( -0.5f * otherWidth, 0f );
						}
						else
						{
							cachedRT.anchoredPosition = new Vector2( 0.5f * otherWidth, 0f );
						}

						break;
					}
				case EOrthoDirection.Vertical:
					{
						cachedRT.sizeDelta = new Vector2( graphPanel.cachedRT.rect.height - otherWidth, sbWidth);
						scrollBar.Init(  this );

						cachedRT.rotation = Quaternion.Euler( 0f, 0f, 90f );

						Vector2 anchorV = new Vector2( 0f, 0.5f ); // anchors for low position
						if (ePosition == ELowHigh.High)
						{
							anchorV.x = 1f;
						}

						cachedRT.anchorMin = anchorV;
						cachedRT.anchorMax = anchorV;
						cachedRT.pivot = anchorV;

						Vector2 anchoredPos = new Vector2( 0.5f * sbWidth, -0.5f * graphPanel.cachedRT.rect.height );
						if (otherPosition == ePosition)
						{
							anchoredPos.y += otherWidth;
						}
						if (ePosition == ELowHigh.High)
						{
							anchoredPos = -1f * anchoredPos;
						}
						cachedRT.anchoredPosition = anchoredPos;

						break;
					}
			}
		}
	}

}
