using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.UI.Scrollable
{
	public class ScrollablePanelView : MonoBehaviour
	{
		static private readonly bool DEBUG_LOCAL = true;

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

		public System.Action<EOrthoDirection, float> onScaleChangeAction;
		public System.Action<EOrthoDirection, float> onPosChangeAction;
		public System.Action<EOrthoDirection, float, float> onViewChangeAction;


		public void HandleViewChange( EOrthoDirection direction, float sizeFraction, float posFraction )
		{
			if (DEBUG_LOCAL)
			{
				Debug.Log( "SPV HandleViewChange: " + direction + " size = " + sizeFraction + ", pos = " + posFraction );
			}
			Vector3 scale = contentPanelRT.localScale;
			Vector2 anchoredPos = contentPanelRT.anchoredPosition;
			float relPosFraction = 0.5f - posFraction;
			switch (direction)
			{
				case EOrthoDirection.Horizontal:
					{
						scale.x = _baseScale.x * 1f / sizeFraction;
						if (onScaleChangeAction != null)
						{
							onScaleChangeAction( EOrthoDirection.Horizontal, scale.x );
						}

						float pos = scale.x * relPosFraction * contentPanelRT.sizeDelta.x;
						if (!Mathf.Approximately(pos, anchoredPos.x ))
						{
							anchoredPos.x = pos;
							if (onPosChangeAction != null)
							{
								onPosChangeAction( direction, posFraction );
							}
						}
						break;
					}
				case EOrthoDirection.Vertical:
					{
						scale.y = _baseScale.y * 1f / sizeFraction;
						if (onScaleChangeAction != null)
						{
							onScaleChangeAction( EOrthoDirection.Vertical, scale.y );
						}
						float pos = scale.y * relPosFraction * contentPanelRT.sizeDelta.y;
						if (!Mathf.Approximately(pos, anchoredPos.y))
						{
							anchoredPos.y = pos;
							if (onPosChangeAction != null)
							{
								onPosChangeAction( direction, posFraction );
							}
						}
						break;
					}
			}

			contentPanelRT.localScale = scale;
			contentPanelRT.anchoredPosition = anchoredPos;
		}


	}
}

