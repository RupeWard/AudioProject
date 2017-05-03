using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Graph
{
	public class GraphViewPanel : MonoBehaviour
	{
		public RectTransform contentPanelRT;

		public RectTransform cachedRT
		{
			get;
			private set;
		}

		private void Awake()
		{
			cachedRT = GetComponent<RectTransform>( );
			contentPanelRT.localScale = Vector3.one;
            contentPanelRT.sizeDelta = cachedRT.sizeDelta;
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
						scale.x = 1f / sizeFraction;
						anchoredPos.x = scale.x * relPosFraction * cachedRT.sizeDelta.x;
						break;
					}
				case EOrthoDirection.Vertical:
					{
						scale.y = 1f / sizeFraction;
						anchoredPos.y = scale.y * relPosFraction * cachedRT.sizeDelta.y;
						break;
					}
			}

			contentPanelRT.localScale = scale;
			contentPanelRT.anchoredPosition = anchoredPos;
		}

	}
}

