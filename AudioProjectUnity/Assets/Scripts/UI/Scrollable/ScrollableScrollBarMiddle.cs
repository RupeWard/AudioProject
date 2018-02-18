using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RJWS.Core.TransformExtensions;

namespace RJWS.UI.Scrollable
{
	public class ScrollableScrollBarMiddle : MonoBehaviour
	{
		public RectTransform bgRT;
		public ObjectGrabber objectGrabber;
		public UnityEngine.UI.Text text;
		public UnityEngine.UI.Image buttonImage;

		private Color _normButtonColour;
		public Color activaButtonColour =  Color.red;

		public RectTransform cachedRT
		{
			private set;
			get;
		}

		private ScrollableScrollBar _graphScrollBar;

		private void Awake( )
		{
			cachedRT = GetComponent<RectTransform>( );
			_normButtonColour = buttonImage.color;
		}

		public void Init( ScrollableScrollBar gsb)
		{
			if (gsb.zoomLevel == 0)
			{
				text.text = string.Empty;
			}
			else
			{
				text.text = gsb.zoomLevel.ToString( );
			}
			_graphScrollBar = gsb;

			gameObject.name = "ScrollMiddle";

			cachedRT.SetParent( _graphScrollBar.cachedRT );
			cachedRT.localScale = new Vector3( 1f, 1f, 1f );
			SetPos( );

			if (_graphScrollBar.scrollBarPanel.eDirection == EOrthoDirection.Horizontal)
			{
				objectGrabber.onXMovementAction += HandleMovement;
			}
			else
			{
				objectGrabber.onYMovementAction += HandleMovement;
			}
			objectGrabber.onActivateAction += HandleGrabberActivated;
		}

		private void SetPos()
		{
			float height = _graphScrollBar.cachedRT.rect.height;
			cachedRT.anchorMin = new Vector2( 0.5f, 0.5f );
			cachedRT.anchorMax = new Vector2( 0.5f, 0.5f );
			cachedRT.pivot = new Vector2( 0.5f, 0.5f );

			cachedRT.anchoredPosition = Vector2.zero;
			cachedRT.sizeDelta = new Vector2( height, height );
			objectGrabber.HandleObjectSizeSet( cachedRT.sizeDelta );
		}


		public void LateUpdate()
		{
			if (_graphScrollBar != null)
			{
				if (_graphScrollBar.zoomLevel > 0 && _graphScrollBar.limitState == ScrollableScrollBar.ELimitState.Upper)
				{
					buttonImage.color = activaButtonColour;
				}
				else
				{
					buttonImage.color = _normButtonColour;
				}
			}
		}

		public void HandleClick()
		{
			//			Debug.Log( Time.time + " Click on " + transform.GetPathInHierarchy( ) );
			if (_graphScrollBar.zoomLevel > 0 && _graphScrollBar.limitState == ScrollableScrollBar.ELimitState.Upper)
			{
				_graphScrollBar.parentScrollBar.DestroyZoomed( );
			}
			else
			{
				if (ScrollableScrollBar.DEBUG_SCROLLBAR)
				{
					Debug.Log( "zoomLevel = " + _graphScrollBar.zoomLevel + ", state= " + _graphScrollBar.limitState );
				}
				if (!objectGrabber.isActivated)
				{
					ObjectGrabManager.Instance.HandleGrabRequest( objectGrabber );
				}
			}
		}

		public void HandleMovement( float delta)
		{
			_graphScrollBar.HandleMiddleMoved( delta );
		}

		public void HandleGrabberActivated( bool b )
		{
			if (_graphScrollBar.IsAtRangeEnd(ELowHigh.High))
			{
				objectGrabber.SetInactiveColour( );
			}
		}

	}

}
