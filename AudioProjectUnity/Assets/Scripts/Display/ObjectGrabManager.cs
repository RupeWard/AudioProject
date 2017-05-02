using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using RJWS.Core.TransformExtensions;

namespace RJWS.Graph
{
	public class ObjectGrabManager : RJWS.Core.Singleton.SingletonSceneLifetime<ObjectGrabManager>
	{
		public static readonly bool DEBUG_OBJECTGRABMANAGER = true;

		ObjectGrabber _currentGrab = null;

		UnityEngine.UI.GraphicRaycaster _rayCaster;

		private bool _grabEnabled = true;
		public float grabDisableDuration = 0.2f;

		private IEnumerator DisableGrabCR()
		{
			_grabEnabled = false;
			yield return new WaitForSeconds( grabDisableDuration );
			_grabEnabled = true;
		}

		protected override void PostAwake( )
		{
			_rayCaster = GetComponent<UnityEngine.UI.GraphicRaycaster>( );
		}

		private bool _didHandleThisFrame = false;
		public void SetHandled(ObjectGrabber og)
		{
			if (og == _currentGrab)
			{
				_didHandleThisFrame = true;
				if (DEBUG_OBJECTGRABMANAGER)
				{
					Debug.Log( "OGM: handled " + _currentGrab.cachedTransform.GetPathInHierarchy( ) );
				}
			}
		}

		public bool HasGrabbed
		{
			get { return _currentGrab != null; }
		}

		public bool IsGrabbed(ObjectGrabber og)
		{
			return (og == _currentGrab);
		}

		public ObjectGrabber CurrentGrab
		{
			get { return _currentGrab; }
		}

		private void CancelCurrentGrab()
		{
			if (_currentGrab != null)
			{
				_currentGrab.Deactivate( );
				_currentGrab = null;

				StartCoroutine( DisableGrabCR( ) );
			}
		}

		public bool HandleGrabRequest( ObjectGrabber og )
		{
			if (!_grabEnabled)
			{
				return false;
			}
			bool result = false;
			if (og == null)
			{
				throw new System.Exception( "NULL GRAB" );
			}
			if (!HasGrabbed)
			{
				_currentGrab = og;
				og.Activate( );
				result = true;

				if (DEBUG_OBJECTGRABMANAGER)
				{
					Debug.Log( Time.time+" OGM grabbed " + _currentGrab.cachedTransform.GetPathInHierarchy( ) );
				}
			}
			else
			{
				if (IsGrabbed( og ))
				{
					if (DEBUG_OBJECTGRABMANAGER)
					{
						Debug.Log( Time.time + "OGM already grabbed " + _currentGrab.cachedTransform.GetPathInHierarchy( ) );
						_didHandleThisFrame = true;
						result = true;
					}
				}
				else
				{
					if (DEBUG_OBJECTGRABMANAGER)
					{
						Debug.Log( Time.time+" OGM can't grab " + og.cachedTransform.GetPathInHierarchy( )
							+ " because already grabbed "+ _currentGrab.cachedTransform.GetPathInHierarchy( ) );
					}
				}
			}
			return result;
		}

		private readonly bool DEBUG_CAST = true;
		private System.Text.StringBuilder _debugRaycastSB = null;

		private void LateUpdate()
		{
			if (Input.GetMouseButton(0))
			{
				if (_currentGrab != null)
				{
					if (!_didHandleThisFrame)
					{
						if (DEBUG_CAST)
						{
							if (_debugRaycastSB == null)
							{
								_debugRaycastSB = new System.Text.StringBuilder( );
							}
							else
							{
								_debugRaycastSB.Length = 0;
							}
							_debugRaycastSB.Append(Time.time ).Append( " RayCast hit " );
						}
						PointerEventData ped = new PointerEventData( null );
						//Set required parameters, in this case, mouse position
						ped.position = Input.mousePosition;
						//Create list to receive all results
						List<RaycastResult> results = new List<RaycastResult>( );
						//Raycast it
						_rayCaster.Raycast( ped, results );

						if (DEBUG_CAST)
						{
							_debugRaycastSB.Append( results.Count ).Append( " objects" );
						}
						bool hit = false;
						if (results.Count > 0)
						{
							if (DEBUG_CAST)
							{
								_debugRaycastSB.Append( ":" );
							}
						}
						foreach( RaycastResult rayCastResult in results)
						{
							GameObject hitGO = rayCastResult.gameObject;
							if (DEBUG_CAST)
							{
								_debugRaycastSB.Append( "\n- " );
							}
							if (rayCastResult.gameObject == _currentGrab.gameObject)
							{
								hit = true;
								if (DEBUG_CAST)
								{
									_debugRaycastSB.Append( "GRABBED " );
								}
							}
							else
							{
								if (DEBUG_CAST)
								{
									_debugRaycastSB.Append( "NOT GRABBED" );
								}
							}
							if (DEBUG_CAST)
							{
								_debugRaycastSB.Append( hitGO.transform.GetPathInHierarchy() );
							}
						}
						if (!hit)
						{
							if (DEBUG_CAST)
							{
								_debugRaycastSB.Append( "\nCANCELLING GRAB" );
							}
							CancelCurrentGrab( );
							
						}
						if (DEBUG_CAST)
						{
							Debug.Log( _debugRaycastSB.ToString( ) );
						}
					}

				}
			}

			_didHandleThisFrame = false;
		}
	}

}
