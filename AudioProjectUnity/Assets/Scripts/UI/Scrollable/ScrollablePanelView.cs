using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.UI.Scrollable
{
	public class ScrollablePanelView : MonoBehaviour
	{
		static private readonly bool DEBUG_LOCAL = false;

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
			SetSizePosFractions( EOrthoDirection.Horizontal, 1f, 0.5f );
			SetSizePosFractions( EOrthoDirection.Vertical, 1f, 0.5f );
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

		private Dictionary<EOrthoDirection, float> _sizeFractions
			= new Dictionary<EOrthoDirection, float>( )
			{
				{ EOrthoDirection.Horizontal, 1f },
				{ EOrthoDirection.Vertical, 1f }
			};

		private Dictionary<EOrthoDirection, float> _posFractions
			= new Dictionary<EOrthoDirection, float>( )
			{
				{ EOrthoDirection.Horizontal, 0.5f },
				{ EOrthoDirection.Vertical, 0.5f }
			};

		private void SetSizePosFractions( EOrthoDirection dirn, float sizeFr, float posFr )
		{
			_sizeFractions[dirn] = sizeFr;
			_posFractions[dirn] = posFr;
			SetViewRect( );
		}

		private void SetViewRect()
		{
			_viewRect.x = _posFractions[EOrthoDirection.Horizontal] - 0.5f * _sizeFractions[EOrthoDirection.Horizontal];
			_viewRect.width = _sizeFractions[EOrthoDirection.Horizontal];

			_viewRect.y = _posFractions[EOrthoDirection.Vertical] - 0.5f * _sizeFractions[EOrthoDirection.Vertical];
			_viewRect.height= _sizeFractions[EOrthoDirection.Vertical];
		}

		private Rect _viewRect = new Rect( );

		public Rect ViewRect
		{
			get
			{
				return _viewRect;
			}
			private set
			{
				_viewRect = value;
			}
		}

		public void HandleViewChange( EOrthoDirection direction, float sizeFrac, float posFract)
		{
			SetSizePosFractions( direction, sizeFrac, posFract );

			if (DEBUG_LOCAL)
			{
				Debug.Log( "SPV HandleViewChange: " + direction + " size = " + sizeFrac + ", pos = " + posFract+", viewRect = "+_viewRect.ToString() );
			}
			
			Vector3 scale = contentPanelRT.localScale;
			Vector2 anchoredPos = contentPanelRT.anchoredPosition;
			float relPosFraction = 0.5f - posFract;
			switch (direction)
			{
				case EOrthoDirection.Horizontal:
					{
						scale.x = _baseScale.x * 1f / sizeFrac;
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
								onPosChangeAction( direction, posFract );
							}
						}
						break;
					}
				case EOrthoDirection.Vertical:
					{
						scale.y = _baseScale.y * 1f / sizeFrac;
						if (onScaleChangeAction != null)
						{
							onScaleChangeAction( EOrthoDirection.Vertical, scale.y);
						}
						float pos = scale.y * relPosFraction * contentPanelRT.sizeDelta.y;
						if (!Mathf.Approximately(pos, anchoredPos.y))
						{
							anchoredPos.y = pos;
							if (onPosChangeAction != null)
							{
								onPosChangeAction( direction, posFract);
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

