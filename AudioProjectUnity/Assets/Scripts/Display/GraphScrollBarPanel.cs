using System.Collections;
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

		private void Awake()
		{
			cachedRT = GetComponent<RectTransform>( );
		}

		public void Init( GraphPanel p, EOrthoDirection ed )
		{
			graphPanel = p;
			eDirection = ed;
			gameObject.name = "ScrollBarPanel_" + eDirection.ToString( );

			cachedRT.SetParent( graphPanel.scrollBarContainer.transform );
			cachedRT.localScale = Vector3.one;

			scrollBar.onScrollBarChanged += graphPanel.HandleScrollBarChanged;
		}

		public void SetUp()
		{ 
			ePosition = graphPanel.graphPanelSettings.positions[eDirection];

			ELowHigh otherPosition = graphPanel.graphPanelSettings.positions[eDirection.OtherDirection( )];

			float size = graphPanel.graphPanelSettings.sizes[eDirection];
			float otherSize = graphPanel.graphPanelSettings.sizes[eDirection.OtherDirection()];

			switch (eDirection)
			{
				case EOrthoDirection.Horizontal:
					{
						cachedRT.sizeDelta = new Vector2( graphPanel.cachedRT.rect.width - otherSize, size );
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
							cachedRT.anchoredPosition = new Vector2( -0.5f * otherSize, 0f );
						}
						else
						{
							cachedRT.anchoredPosition = new Vector2( 0.5f * otherSize, 0f );
						}

						break;
					}
				case EOrthoDirection.Vertical:
					{
						cachedRT.sizeDelta = new Vector2( graphPanel.cachedRT.rect.height - otherSize, size);
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

						Vector2 anchoredPos = new Vector2( 0.5f * size, -0.5f * graphPanel.cachedRT.rect.height );
						if (otherPosition == ePosition)
						{
							anchoredPos.y += otherSize;
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
