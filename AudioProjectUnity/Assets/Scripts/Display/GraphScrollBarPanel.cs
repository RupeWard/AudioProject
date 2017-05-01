using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RJWS.Core.UI;

namespace RJWS.Graph
{
	public class GraphScrollBarPanel : MonoBehaviour
	{
		public const float SIZE = 40f;

		public GraphScrollBar scrollBar;

		public RectTransform cachedRT
		{
			private set;
			get;
		}

		private GraphPanel _graphPanel;

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

		public void Init(GraphPanel p, EOrthoDirection eod, ELowHigh pos)
		{
			_graphPanel = p;
			eDirection = eod;
			ePosition = pos;

			gameObject.name = "ScrollBarPanel_" + eod.ToString();

			cachedRT.SetParent( _graphPanel.transform );
			cachedRT.localScale = Vector3.one;

			switch (eDirection)
			{
				case EOrthoDirection.Horizontal:
					{
						cachedRT.sizeDelta = new Vector2( _graphPanel.cachedRT.rect.width - SIZE, SIZE );
						scrollBar.Init( this );

						cachedRT.rotation = Quaternion.Euler( 0f, 0f, 0f );

						cachedRT.anchorMin = new Vector2( 0.5f, 0f );
						cachedRT.anchorMax = new Vector2( 0.5f, 0f );
						cachedRT.pivot = new Vector2( 0.5f, 0f );
						cachedRT.anchoredPosition = new Vector2( -0.5f * SIZE, 0f );

						break;
					}
				case EOrthoDirection.Vertical:
					{
						cachedRT.sizeDelta = new Vector2( _graphPanel.cachedRT.rect.height - SIZE, SIZE );
						scrollBar.Init( this );

						cachedRT.rotation = Quaternion.Euler( 0f, 0f, 90f );

						cachedRT.anchorMin = new Vector2( 1f, 0.5f );
						cachedRT.anchorMax = new Vector2( 1f, 0.5f );
						cachedRT.pivot = new Vector2( 1f, 0.5f );

						cachedRT.anchoredPosition = new Vector2( -0.5f * SIZE, 0.5f * _graphPanel.cachedRT.rect.height );

						break;
					}
			}
		}
	}

}
