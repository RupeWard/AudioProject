using UnityEngine.EventSystems;
using UnityEngine;
using RJWS.Core.TransformExtensions;
using RJWS.Core.DebugDescribable;

namespace RJWS.Audio
{
	public class GuitarStringPlucker_BasicDrag : GuitarStringPlucker_Base
	{
		public GuitarStringPlucker_BasicDrag( GuitarStringView s, bool b)
			: base( s, b )
		{

		}

		private bool _isDown = false;
		private float _downTime;
		private Vector3 _downLocation;

		
		public override void OnPointerUp( UnityEngine.EventSystems.PointerEventData data )
		{
			if (_debug)
			{
				Debug.LogFormat( _stringView, "{2} OnPointerUp: {0}\n{1}", _stringView.cachedTransform.GetPathInHierarchy( ), data.position, Time.time );
			}
			PointerDepartHelper( data );
		}
		

		public override void OnPointerExit( UnityEngine.EventSystems.PointerEventData data )
		{
			if (_debug)
			{
				Debug.LogFormat( _stringView, "{2} OnPointerExit: {0}\n{1}", _stringView.cachedTransform.GetPathInHierarchy( ), data.position, Time.time );
			}
			PointerDepartHelper( data );
		}

		private void PointerDepartHelper( UnityEngine.EventSystems.PointerEventData data )
		{
			if (!_isDown)
			{
//				Debug.LogErrorFormat( _stringView, "OnPointerDepart when not down on {0}", _stringView.cachedTransform.GetPathInHierarchy( ) );
				return;
			}

			_isDown = false;

			int fret = 0;
			RaycastHit hitInfo;
			if (Physics.Raycast( Camera.main.ScreenPointToRay( data.position ), out hitInfo, 100 ))
			{
				if (hitInfo.collider.gameObject == _stringView.stringObject)
				{
					float d = 0f;
					fret = _stringView.guitarView.GetFretForWorldX( hitInfo.point.x, ref d );

					float elapsed = Time.time - _downTime;
					float distance = Vector3.Distance( _downLocation, hitInfo.point );

					if (_debug)
					{
						Debug.LogWarningFormat( "String Hit {0} at {1} which is Fret {2} at d = {3}... T = {4}, D = {5}",
							hitInfo.collider.transform.GetPathInHierarchy( ),
							hitInfo.point,
							fret, d,
							elapsed.ToString("G4"),
							distance.ToString( "G4" ) );
					}
				}
				else
				{
					Debug.LogErrorFormat( "Object mismatch when String Hit {0} at {1}", hitInfo.collider.transform.GetPathInHierarchy( ), hitInfo.point );
				}
			}
			_stringView.stringBehaviour.Pluck( fret );
		}

		
		public override void OnPointerDown( UnityEngine.EventSystems.PointerEventData data )
		{
			if (_debug)
			{
				Debug.LogFormat( _stringView, "OnPointerDown: {0}\n{1}", _stringView.cachedTransform.GetPathInHierarchy( ), data.position );
			}
			PointerStartHelper( data );
		}
		
		
		public override void OnPointerEnter( UnityEngine.EventSystems.PointerEventData data )
		{
			if (_debug)
			{
				Debug.LogFormat( _stringView, "OnPointerEnter: {0}\n{1}", _stringView.cachedTransform.GetPathInHierarchy( ), data.position );
			}
			PointerStartHelper( data );
		}

		private void PointerStartHelper( UnityEngine.EventSystems.PointerEventData data )
		{
			if (_isDown)
			{
				Debug.LogErrorFormat( _stringView, "OnPointerDown when already down on {0}", _stringView.cachedTransform.GetPathInHierarchy( ) );
			}
			int fret = 0;
			RaycastHit hitInfo;
			if (Physics.Raycast( Camera.main.ScreenPointToRay( data.position ), out hitInfo, 100 ))
			{
				if (hitInfo.collider.gameObject == _stringView.stringObject)
				{
					float d = 0f;
					fret = _stringView.guitarView.GetFretForWorldX( hitInfo.point.x, ref d );

					_isDown = true;
					_downTime = Time.time;
					_downLocation = hitInfo.point;

					if (_debug)
					{
						Debug.LogWarningFormat( "String Hit {0} at {1} which is Fret {2} at d = {3}",
							hitInfo.collider.transform.GetPathInHierarchy( ),
							hitInfo.point,
							fret, d );
					}
				}
				else
				{
					Debug.LogErrorFormat( "Object mismatch when String Hit {0} at {1}", hitInfo.collider.transform.GetPathInHierarchy( ), hitInfo.point );
				}
			}

		}

	}
}
