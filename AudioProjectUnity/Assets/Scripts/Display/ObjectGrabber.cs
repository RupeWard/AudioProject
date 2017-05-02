using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using RJWS.Core.TransformExtensions;

namespace RJWS.Graph
{
	public class ObjectGrabber: MonoBehaviour
	{
		static public readonly bool DEBUG_OBJECTGRABBER = true;

		private UnityEngine.UI.Image _image;
		private Collider2D _collider;

		private bool _activated = false;

		public Transform cachedTransform
		{
			get;
			private set;
		}

		public RectTransform cachedRT
		{
			get;
			private set;
		}

		protected virtual void Awake()
		{
			cachedTransform = transform;
			cachedRT = GetComponent<RectTransform>( );

			_image = GetComponent<UnityEngine.UI.Image>( );
			_collider = GetComponent<Collider2D>( );

			_image.enabled = false;
			_collider.enabled = false;
			_activated = false;
		}

		public void Activate()
		{
			Activate( true );
		}

		public void Deactivate()
		{
			Activate( false );
		}

		public void Activate(bool active)
		{
			if (active != _activated)
			{
				_image.enabled = active;
				_collider.enabled = active;
				_activated = active;

				lastFramePointerPos = null;

				if (DEBUG_OBJECTGRABBER)
				{
					if (_activated)
					{
						Debug.Log( Time.time + " OG: activated " + cachedTransform.GetPathInHierarchy( ) );
                    }
					else
					{
						Debug.Log( Time.time + " OG: deactivated " + cachedTransform.GetPathInHierarchy( ) );
					}
				}
			}
		}

		private static readonly bool DEBUG_DRAG = true;

		private Vector2? lastFramePointerPos =null;

		public bool isDragging
		{
			get;
			private set;
		}

		public void HandlePointerDown()
		{
			if (DEBUG_DRAG)
			{
				Debug.Log( Time.time + " Down " + cachedTransform.GetPathInHierarchy( ) );
			}
			ObjectGrabManager.Instance.SetHandled( this );
		}

		public void HandlePointerUp( )
		{
			if (DEBUG_DRAG)
			{
				Debug.Log( Time.time + " Up " + cachedTransform.GetPathInHierarchy( ) );
			}
			ObjectGrabManager.Instance.SetHandled( this );
			if (_activated)
			{
				//				ObjectGrabManager.Instance.CancelGrab( this );
				lastFramePointerPos = null;
				isDragging = false;
			}
		}

		public void HandlePointerMove( )
		{
			if (DEBUG_DRAG)
			{
				Debug.Log( Time.time + " Move " + cachedTransform.GetPathInHierarchy( ) );
			}
			ObjectGrabManager.Instance.SetHandled( this );
		}

		public void HandlePointerClick( )
		{
			if (DEBUG_DRAG)
			{
				Debug.Log( Time.time + " Click " + cachedTransform.GetPathInHierarchy( ) );
			}
			ObjectGrabManager.Instance.SetHandled( this );
		}

		public void HandleDrag( )
		{
			if (DEBUG_DRAG)
			{
				Debug.Log( Time.time + " Drag " + cachedTransform.GetPathInHierarchy( ) );
			}
			ObjectGrabManager.Instance.SetHandled( this );
			
		}

		public void HandleDrop( )
		{
			if (DEBUG_DRAG)
			{
				Debug.Log( Time.time + " Drop" + cachedTransform.GetPathInHierarchy( ) );
			}
			ObjectGrabManager.Instance.SetHandled( this );
		}

		public void OnMouseOver()
		{
			Debug.LogWarning( "MOUSEOVER" );
			ObjectGrabManager.Instance.SetHandled( this );
		}

		public void HandleHit(Vector2 screenPos)
		{
			if (!_activated)
			{
				Debug.LogError( "HandleHit when not activated" );
				return;
			}
			isDragging = true;
			if (lastFramePointerPos != null)
			{
				Vector2 movement = screenPos - (Vector2)lastFramePointerPos; //raycastResult.screenPosition - (Vector2)lastFramePointerPos;
				if (DEBUG_DRAG)
				{
					Debug.Log( Time.time + " MOVED " + screenPos + " centre " + cachedTransform.position +" last " + (Vector2)lastFramePointerPos+ " moved "+movement );
				}
				if (onMovementAction != null)
				{
					onMovementAction( movement );
                }
				if (onXMovementAction != null)
				{
					onXMovementAction( movement.x );
				}
				if (onYMovementAction != null)
				{
					onYMovementAction( movement.y );
				}

			}
			else
			{
				if (DEBUG_DRAG)
				{
					Debug.Log( Time.time + " GRABBED " + screenPos);
				}
			}
			lastFramePointerPos = new Vector2( screenPos.x, screenPos.y );
		}

		public System.Action< Vector2 > onMovementAction;
		public System.Action< float > onXMovementAction;
		public System.Action< float > onYMovementAction;
	}


}
