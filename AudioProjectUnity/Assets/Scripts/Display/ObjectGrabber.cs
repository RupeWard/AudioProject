using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

		public void HandlePointerDown()
		{
			ObjectGrabManager.Instance.SetHandled( this );
		}

		public void HandlePointerUp( )
		{
			ObjectGrabManager.Instance.SetHandled( this );
		}

		public void HandlePointerMove( )
		{
			ObjectGrabManager.Instance.SetHandled( this );
		}

		public void HandlePointerClick( )
		{
			ObjectGrabManager.Instance.SetHandled( this );
		}

		public void OnMouseOver()
		{
			Debug.LogWarning( "MOUSEOVER" );
			ObjectGrabManager.Instance.SetHandled( this );
		}
	}


}
