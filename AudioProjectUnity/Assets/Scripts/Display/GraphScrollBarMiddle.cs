using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RJWS.Core.TransformExtensions;

namespace RJWS.Graph
{
	public class GraphScrollBarMiddle : MonoBehaviour
	{
		public RectTransform bgRT;
		public ObjectGrabber objectGrabber;

		public RectTransform cachedRT
		{
			private set;
			get;
		}

		private GraphScrollBar _graphScrollBar;

		private void Awake( )
		{
			cachedRT = GetComponent<RectTransform>( );
		}

		public void Init( GraphScrollBar gsb)
		{
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
		}

		private void SetPos()
		{
			float height = _graphScrollBar.cachedRT.rect.height;
			cachedRT.anchorMin = new Vector2( 0.5f, 0.5f );
			cachedRT.anchorMax = new Vector2( 0.5f, 0.5f );
			cachedRT.pivot = new Vector2( 0.5f, 0.5f );

			cachedRT.anchoredPosition = Vector2.zero;
			cachedRT.sizeDelta = new Vector2( height, height );
		}

		public void HandleClick()
		{
			Debug.Log( Time.time + " Click on " + transform.GetPathInHierarchy( ) );
			if (!objectGrabber.isActivated)
			{
				ObjectGrabManager.Instance.HandleGrabRequest( objectGrabber );
			}
		}

		public void HandleMovement( float delta)
		{
			_graphScrollBar.HandleMiddleMoved( delta );
		}
	}

}
