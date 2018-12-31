﻿using UnityEngine.EventSystems;
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

		const string EXIT_REASON = "Exit";
		const string ENTER_REASON = "Enter";
		const string DOWN_REASON = "Down";
		const string UP_REASON = "Up";

		public override void OnPointerUp( UnityEngine.EventSystems.PointerEventData data )
		{
			if (_debug)
			{
				Debug.LogFormat( _stringView, "{2} OnPointerUp: {0}\n{1}", _stringView.cachedTransform.GetPathInHierarchy( ), data.position, Time.time );
			}
			PointerDepartHelper(UP_REASON, data );
		}
		

		public override void OnPointerExit( UnityEngine.EventSystems.PointerEventData data )
		{
			if (_debug)
			{
				Debug.LogFormat( _stringView, "{2} OnPointerExit: {0}\n{1}", _stringView.cachedTransform.GetPathInHierarchy( ), data.position, Time.time );
			}
			PointerDepartHelper(EXIT_REASON, data );
		}

		private void PointerDepartHelper(string reason, UnityEngine.EventSystems.PointerEventData data )
		{
			if (!_isDown)
			{
//				Debug.LogErrorFormat( _stringView, "OnPointerDepart when not down on {0}", _stringView.cachedTransform.GetPathInHierarchy( ) );
				return;
			}

			_isDown = false;

			int fret = 0;
			RaycastHit hitInfo;
			if (Physics.Raycast( Camera.main.ScreenPointToRay( data.position ), out hitInfo, 100, _fretboardLayerMask))
			{
				{
					float d = 0f;
					fret = _stringView.guitarView.GetFretForWorldX( hitInfo.point.x, ref d );

					float elapsed = Time.time - _downTime;
					float distance = Vector3.Distance( _downLocation, hitInfo.point );
					float speed = distance / elapsed;

					float volume = _stringView.guitarView.pluckSettings.GetVolumeForSpeed( speed );
					_stringView.stringBehaviour.Pluck( volume, fret );

					if (_debug)
					{
						Debug.LogWarningFormat( "({6}) String Hit {0} at {1} which is Fret {2} at d = {3}... T = {4}, D = {5}. Speed = {7} => Volume = {8}",
							hitInfo.collider.transform.GetPathInHierarchy( ),
							hitInfo.point,
							fret, d,
							elapsed.ToString("G4"),
							distance.ToString( "G4" ),
							reason,
							speed,
							volume);
					}
				}
			}
		}


		public override void OnPointerDown( UnityEngine.EventSystems.PointerEventData data )
		{
			if (_debug)
			{
				Debug.LogFormat( _stringView, "OnPointerDown: {0}\n{1}", _stringView.cachedTransform.GetPathInHierarchy( ), data.position );
			}
			PointerStartHelper(DOWN_REASON, data );
		}
		
		
		public override void OnPointerEnter( UnityEngine.EventSystems.PointerEventData data )
		{
			if (_debug)
			{
				Debug.LogFormat( _stringView, "OnPointerEnter: {0}\n{1}", _stringView.cachedTransform.GetPathInHierarchy( ), data.position );
			}
			PointerStartHelper(ENTER_REASON, data );
		}

		private void PointerStartHelper(string reason, UnityEngine.EventSystems.PointerEventData data )
		{
			if (_isDown)
			{
				Debug.LogErrorFormat( _stringView, "({1}) PointerStartHelper when already down on {0}", 
					_stringView.cachedTransform.GetPathInHierarchy( ),
					reason);
			}
			int fret = 0;
			RaycastHit hitInfo;
			if (Physics.Raycast( Camera.main.ScreenPointToRay( data.position ), out hitInfo, 100, _fretboardLayerMask ))
			{
				{
					float d = 0f;
					fret = _stringView.guitarView.GetFretForWorldX( hitInfo.point.x, ref d );

					_isDown = true;
					_downTime = Time.time;
					_downLocation = hitInfo.point;

					if (_debug)
					{
						Debug.LogWarningFormat( "({4}) String Hit {0} at {1} which is Fret {2} at d = {3}",
							hitInfo.collider.transform.GetPathInHierarchy( ),
							hitInfo.point,
							fret, d ,
							reason);
					}
				}
			}
		}

		public override void OnDrag( PointerEventData eventData )
		{
			Debug.Log( "Drag" );
		}

		public override void OnBeginDrag( PointerEventData eventData )
		{
			Debug.Log( "BeginDrag" );
		}

		public override void OnEndDrag( PointerEventData eventData )
		{
			Debug.Log( "EndDrag" );
		}
	}
}
