using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using RJWS.Core.TransformExtensions;

namespace RJWS.UI
{
	public class ObjectGrabber: MonoBehaviour
	{
		static public readonly bool DEBUG_OBJECTGRABBER = false;
		private static readonly bool DEBUG_DRAG = false;
		private static readonly bool DEBUG_CLICK = false;

		public float relativeSize = 3f;

		public float doubleClickPeriod = 0.5f;
		public float clickPeriod = 0.5f;
		private float _downTime = float.MinValue;
		private float _lastClickTime = float.MinValue;

		private UnityEngine.UI.Image _image;
		private Collider2D _collider;


		private Vector2? lastFramePointerPos = null;

		public System.Action<Vector2> onMovementAction;
		public System.Action<float> onXMovementAction;
		public System.Action<float> onYMovementAction;
		public System.Action onDoubleClickAction;
		public System.Action onClickAction;
		public System.Action<bool> onActivateAction;

		public Color inactiveColour = Color.white;
		public Color activeColour = Color.green;

		public void HandleObjectSizeSet(Vector2 objectSize)
		{
			cachedRT.sizeDelta = relativeSize * objectSize;
		}

		public bool isDragging
		{
			get;
			private set;
		}

		public bool isActivated
		{
			get;
			private set;
		}

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
			isActivated = false;
		}

		public void Activate()
		{
			Activate( true );
		}

		public void Deactivate()
		{
			Activate( false );
		}

		public void SetInactiveColour()
		{
			_image.color = inactiveColour;
		}

		public void Activate(bool active)
		{
			_image.color = activeColour;
			if (active != isActivated)
			{
				_image.enabled = active;
				_collider.enabled = active;
				isActivated = active;

				lastFramePointerPos = null;

				HandlePointerDown( );
				if (DEBUG_OBJECTGRABBER)
				{
					if (isActivated)
					{
						Debug.Log( Time.time + " OG: activated " + cachedTransform.GetPathInHierarchy( ) );
                    }
					else
					{
						Debug.Log( Time.time + " OG: deactivated " + cachedTransform.GetPathInHierarchy( ) );
					}
				}

				if (onActivateAction != null)
				{
					onActivateAction( active );
				}
			}
		}


		public void HandlePointerDown()
		{
			if (DEBUG_DRAG || DEBUG_CLICK)
			{
				Debug.Log( Time.time + " OG: Down " + cachedTransform.GetPathInHierarchy( ) );
			}
			_downTime = Time.time;
			ObjectGrabManager.Instance.SetHandled( this );
		}

		public void HandlePointerUp( )
		{
			if (DEBUG_DRAG || DEBUG_CLICK)
			{
				Debug.Log( Time.time + " OG: Up " + cachedTransform.GetPathInHierarchy( ) );
			}
			ObjectGrabManager.Instance.SetHandled( this );
			if (isActivated)
			{
				lastFramePointerPos = null;
				isDragging = false;

				if (Time.time - _downTime < clickPeriod)
				{
					HandlePointerClick( );
				}
				else
				{
					_lastClickTime = float.MinValue;
				}
			}
			_downTime = float.MinValue;
		}
		
		public void HandlePointerMove( )
		{
			if (DEBUG_DRAG)
			{
				Debug.Log( Time.time + " Move " + cachedTransform.GetPathInHierarchy( ) );
			}
			ObjectGrabManager.Instance.SetHandled( this );
		}


		private void DoClickAction()
		{
			if (DEBUG_CLICK || DEBUG_OBJECTGRABBER)
			{
				Debug.Log( "OG " + Time.time + " Click " + cachedTransform.GetPathInHierarchy( ) );
			}
			if (onClickAction != null)
			{
				onClickAction( );
			}
		}

		private void DoDoubleClickAction()
		{
			if (DEBUG_CLICK || DEBUG_OBJECTGRABBER)
			{
				Debug.Log( "OG Double Click " + cachedTransform.GetPathInHierarchy( ) );
			}
			CancelInvoke( "DoClickAction");
			if (onDoubleClickAction != null)
			{
				onDoubleClickAction( );
			}
		}

		private void HandlePointerClick( )
		{
			if (Time.time -_lastClickTime <= doubleClickPeriod)
			{
				DoDoubleClickAction( );
			}
			else
			{
				CancelInvoke( "DoClickAction" );
				Invoke( "DoClickAction", doubleClickPeriod + 0.2f );
			}
			_lastClickTime = Time.time;
		}

		public void HandleHit(Vector2 screenPos)
		{
			if (!isActivated)
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
					Debug.Log( Time.time + " OG MOVED " + screenPos + " centre " + cachedTransform.position +" last " + (Vector2)lastFramePointerPos+ " moved "+movement );
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
				if (DEBUG_OBJECTGRABBER || DEBUG_CLICK || DEBUG_DRAG)
				{
					Debug.Log( Time.time + " OG: GRABBED " + screenPos);
				}
			}
			lastFramePointerPos = new Vector2( screenPos.x, screenPos.y );
		}

	}


}
