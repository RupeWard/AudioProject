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
		private UnityEngine.UI.Button _cancelGrabButton;

		private IEnumerator DisableGrabCR()
		{
			_grabEnabled = false;
			yield return new WaitForSeconds( grabDisableDuration );
			_grabEnabled = true;
		}

		protected override void PostAwake( )
		{
		}

		private static readonly bool DEBUG_SETUP = true;

		public void OnScrollBarsSetUp(UnityEngine.UI.Button button, Dictionary<EOrthoDirection, GraphScrollBarPanel> scrollBars)
		{
			_cancelGrabButton = button;
			RectTransform cancelGrabButtonRT = _cancelGrabButton.GetComponent<RectTransform>( );

			System.Text.StringBuilder sb = null;
			if (DEBUG_SETUP)
			{
				sb = new System.Text.StringBuilder( );
				sb.Append( "OGM: OnScrollBarsSetUp" );
				foreach (KeyValuePair<EOrthoDirection, GraphScrollBarPanel> kvp in scrollBars)
				{
					sb.Append( "\n-" ).Append( kvp.Key ).Append( " " ).Append( kvp.Value.cachedRT.rect );
				}
			}
			cancelGrabButtonRT.sizeDelta =
				new Vector2(
					scrollBars[EOrthoDirection.Vertical].cachedRT.rect.height,
					scrollBars[EOrthoDirection.Horizontal].cachedRT.rect.height);

			Vector2 anchoredPos =
				new Vector2(
					0.5f * scrollBars[EOrthoDirection.Vertical].cachedRT.rect.height,
					0.5f * scrollBars[EOrthoDirection.Horizontal].cachedRT.rect.height );
			if (scrollBars[EOrthoDirection.Horizontal].ePosition == ELowHigh.Low)
			{
				anchoredPos.x += scrollBars[ EOrthoDirection.Horizontal].cachedRT.rect.width;
			}
			if (scrollBars[EOrthoDirection.Vertical].ePosition == ELowHigh.Low)
			{
				anchoredPos.y += scrollBars[ EOrthoDirection.Vertical].cachedRT.rect.width;
			}
			cancelGrabButtonRT.anchoredPosition = anchoredPos;
			if (sb != null)
			{
				sb.Append( "\n sizeDelta = " + cancelGrabButtonRT.sizeDelta);
				sb.Append( "\n anchored Pos = " + cancelGrabButtonRT.anchoredPosition );
				sb.Append( "\n rect = " + cancelGrabButtonRT.rect);
				Debug.Log( sb.ToString( ) );
			}

			_cancelGrabButton.gameObject.SetActive( false );
		}

		private bool _didHandleThisFrame = false;
		public void SetHandled(ObjectGrabber og)
		{
			if (og == _currentGrab)
			{
				_didHandleThisFrame = true;
				if (DEBUG_OBJECTGRABMANAGER)
				{
					Debug.Log( Time.time+ " OGM: handled " + _currentGrab.cachedTransform.GetPathInHierarchy( ) );
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
			Debug.Log( "CancelGrab" );
			if (_currentGrab != null)
			{
				_currentGrab.Deactivate( );
				_currentGrab = null;

				_cancelGrabButton.gameObject.SetActive( false );
				StartCoroutine( DisableGrabCR( ) );
			}
			else
			{
				Debug.LogWarning( "Null CurrentGrab" );
			}
		}

		private void Grab(ObjectGrabber og)
		{
			_currentGrab = og;
			og.Activate( );
			_cancelGrabButton.gameObject.SetActive( true );

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
