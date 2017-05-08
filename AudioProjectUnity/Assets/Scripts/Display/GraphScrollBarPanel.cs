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
			ePosition = graphPanel.graphPanelSettings.GetScrollBarPosition(eDirection);

			ELowHigh otherPosition = graphPanel.graphPanelSettings.GetScrollBarPosition( eDirection.OtherDirection( ));

			float sbWidth = graphPanel.graphPanelSettings.scrollBarWidth;

			switch (eDirection)
			{
				case EOrthoDirection.Horizontal:
					{
						cachedRT.sizeDelta = new Vector2( graphPanel.cachedRT.rect.width - sbWidth, sbWidth );
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
							cachedRT.anchoredPosition = new Vector2( -0.5f * sbWidth, 0f );
						}
						else
						{
							cachedRT.anchoredPosition = new Vector2( 0.5f * sbWidth, 0f );
						}

						break;
					}
				case EOrthoDirection.Vertical:
					{
						cachedRT.sizeDelta = new Vector2( graphPanel.cachedRT.rect.height - sbWidth, sbWidth);
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
							anchoredPos.y += sbWidth;
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
