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

		public UnityEngine.UI.GraphicRaycaster grabRayCaster;

		private bool _grabEnabled = true;
		public float grabDisableDuration = 0.2f;
		public RectTransform cancelGrabButtonRT;

		private IEnumerator DisableGrabCR()
		{
			_grabEnabled = false;
			yield return new WaitForSeconds( grabDisableDuration );
			_grabEnabled = true;
		}

		protected override void PostAwake( )
		{
			cancelGrabButtonRT.gameObject.SetActive( false);
		}

		public void OnScrollBarsSetUp( Dictionary<EOrthoDirection, GraphScrollBarPanel> scrollBars)
		{
			cancelGrabButtonRT.sizeDelta =
				new Vector2(
					scrollBars[EOrthoDirection.Vertical].cachedRT.sizeDelta.y,
					scrollBars[EOrthoDirection.Horizontal].cachedRT.sizeDelta.y);
			Vector2 anchoredPos =
				new Vector2(
					0.5f * scrollBars[EOrthoDirection.Vertical].cachedRT.sizeDelta.y,
					0.5f * scrollBars[EOrthoDirection.Horizontal].cachedRT.sizeDelta.y );
			if (scrollBars[EOrthoDirection.Horizontal].ePosition == ELowHigh.Low)
			{
				anchoredPos.x += scrollBars[ EOrthoDirection.Horizontal].cachedRT.sizeDelta.x;
			}
			if (scrollBars[EOrthoDirection.Vertical].ePosition == ELowHigh.Low)
			{
				anchoredPos.y += scrollBars[ EOrthoDirection.Vertical].cachedRT.sizeDelta.x;
			}
			cancelGrabButtonRT.anchoredPosition = anchoredPos;
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

		public void CancelGrab(ObjectGrabber og)
		{
			if (_currentGrab != null)
			{
				if (og == _currentGrab)
				{
					CancelCurrentGrab( );
				}
				else
				{
					Debug.LogError( "Cancel called on grab " + og.cachedTransform.GetPathInHierarchy( ) + " when current is " + _currentGrab.cachedTransform.GetPathInHierarchy( ) );
				}
			}
			else
			{
				Debug.LogError( "Cancel called on grab " + og.cachedTransform.GetPathInHierarchy( ) + " when no _currentGrab" );
			}
		}

		public void CancelCurrentGrab()
		{
			if (_currentGrab != null)
			{
				_currentGrab.Deactivate( );
				_currentGrab = null;

				cancelGrabButtonRT.gameObject.SetActive( false );
				StartCoroutine( DisableGrabCR( ) );
			}
		}

		private void Grab(ObjectGrabber og)
		{
			_currentGrab = og;
			og.Activate( );
			cancelGrabButtonRT.gameObject.SetActive( true );

			if (DEBUG_OBJECTGRABMANAGER)
			{
				Debug.Log( Time.time + " OGM grabbed " + _currentGrab.cachedTransform.GetPathInHierarchy( ) );
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
				Grab( og );
				result = true;
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

		private readonly bool DEBUG_CAST = false;
		private System.Text.StringBuilder _debugRaycastSB = null;

		private void LateUpdate()
		{
			if (Input.GetMouseButton(0))
			{
				if (_currentGrab != null)
				{
					if (_currentGrab.isDragging)
					{
						_currentGrab.HandleHit(Input.mousePosition );
					}
					else
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
								_debugRaycastSB.Append( Time.time ).Append( " RayCast hit " );
							}
							PointerEventData ped = new PointerEventData( null );
							//Set required parameters, in this case, mouse position
							ped.position = Input.mousePosition;
							//Create list to receive all results
							List<RaycastResult> results = new List<RaycastResult>( );
							//Raycast it
							grabRayCaster.Raycast( ped, results );

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
							foreach (RaycastResult rayCastResult in results)
							{
								GameObject hitGO = rayCastResult.gameObject;
								if (DEBUG_CAST)
								{
									_debugRaycastSB.Append( "\n- " );
								}
								if (rayCastResult.gameObject == _currentGrab.gameObject)
								{
									ObjectGrabber grabber = rayCastResult.gameObject.GetComponent<ObjectGrabber>( );
									if (grabber != null)
									{
										hit = true;
										grabber.HandleHit( ped.position );
										if (DEBUG_CAST)
										{
											_debugRaycastSB.Append( "GRABBED " );
										}
									}
									else
									{
										Debug.LogError( "No grabber on " + hitGO.transform.GetPathInHierarchy( ) );
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
									_debugRaycastSB.Append( hitGO.transform.GetPathInHierarchy( ) );
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
			}

			_didHandleThisFrame = false;
		}
	}

}
