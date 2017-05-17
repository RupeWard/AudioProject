using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.UI.Scrollable
{
	public class ScrollablePanelView : MonoBehaviour
	{
		public RectTransform contentPanelRT;

		private Vector3 _baseScale = Vector3.one;
		private ScrollablePanel _scrollablePanel;


		public RectTransform cachedRT
		{
			get;
			private set;
		}

		private void Awake()
		{
			cachedRT = GetComponent<RectTransform>( );
		}

		public void InitContent(ScrollablePanel sp)
		{
			_scrollablePanel = sp;
			Vector2 cSize = _scrollablePanel.settings.contentSize;
			if (cSize.sqrMagnitude > 0f)
			{
				contentPanelRT.sizeDelta = cSize;

				float xScale = cachedRT.sizeDelta.x / cSize.x;
				float yScale = cachedRT.sizeDelta.y / cSize.y;

				if (_scrollablePanel.settings.scrollSettings.linkedScaling)
				{
					xScale = yScale = Mathf.Min( xScale, yScale );
				}
				_baseScale = new Vector3( xScale, yScale, 1f );
				contentPanelRT.localScale = _baseScale;
			}
			else
			{
				contentPanelRT.sizeDelta = cachedRT.sizeDelta;
				contentPanelRT.localScale = Vector3.one;
			}
			contentPanelRT.anchoredPosition = Vector2.zero;
		}

		public void HandleViewChange( EOrthoDirection direction, float sizeFraction, float posFraction )
		{
			Vector3 scale = contentPanelRT.localScale;
			Vector2 anchoredPos = contentPanelRT.anchoredPosition;
			float relPosFraction = 0.5f - posFraction;
			switch (direction)
			{
				case EOrthoDirection.Horizontal:
					{
						scale.x = _baseScale.x * 1f / sizeFraction;
						anchoredPos.x = scale.x * relPosFraction * contentPanelRT.sizeDelta.x;// cachedRT.sizeDelta.x;
						break;
					}
				case EOrthoDirection.Vertical:
					{
						scale.y = _baseScale.y * 1f / sizeFraction;
						anchoredPos.y = scale.y * relPosFraction * contentPanelRT.sizeDelta.y;// cachedRT.sizeDelta.y;
						break;
					}
			}

			contentPanelRT.localScale = scale;
			contentPanelRT.anchoredPosition = anchoredPos;
		}


	}
}

