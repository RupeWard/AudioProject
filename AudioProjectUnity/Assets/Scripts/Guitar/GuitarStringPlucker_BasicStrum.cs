using UnityEngine.EventSystems;
using UnityEngine;
using RJWS.Core.TransformExtensions;
using RJWS.Core.DebugDescribable;

namespace RJWS.Audio
{
	public class GuitarStringPlucker_BasicStrum: GuitarStringPlucker_Base
	{
		public GuitarStringPlucker_BasicStrum( GuitarStringView s, bool b)
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
				Debug.LogFormat( _stringView, "STRUM {2} OnPointerUp: {0}\n{1}", _stringView.cachedTransform.GetPathInHierarchy( ), data.position, Time.time );
			}
			PointerDepartHelper(UP_REASON, data );
		}
		

		public override void OnPointerExit( UnityEngine.EventSystems.PointerEventData data )
		{
			if (_debug)
			{
				Debug.LogFormat( _stringView, "{2} STRUM : {0}\n{1}", _stringView.cachedTransform.GetPathInHierarchy( ), data.position, Time.time );
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

					if (fret == int.MaxValue)
					{
						float elapsed = Time.time - _downTime;
						float distance = Vector3.Distance( _downLocation, hitInfo.point );
						float speed = distance / elapsed;

						float volume = _stringView.guitarView.pluckSettings.GetVolumeForSpeed( speed );
						_stringView.stringBehaviour.Pluck( volume, fret );

						if (_debug)
						{
							Debug.LogWarningFormat( "STRUM ({6}) String Hit {0} at {1} which is Fret {2} at d = {3}... T = {4}, D = {5}. Speed = {7} => Volume = {8}",
								hitInfo.collider.transform.GetPathInHierarchy( ),
								hitInfo.point,
								fret, d,
								elapsed.ToString( "G4" ),
								distance.ToString( "G4" ),
								reason,
								speed,
								volume );
						}
					}

				}
			}
		}


		public override void OnPointerDown( UnityEngine.EventSystems.PointerEventData data )
		{
			if (_debug)
			{
				Debug.LogFormat( _stringView, "STRUM OnPointerDown: {0}\n{1}", _stringView.cachedTransform.GetPathInHierarchy( ), data.position );
			}
			PointerStartHelper(DOWN_REASON, data );
		}
		
		
		public override void OnPointerEnter( UnityEngine.EventSystems.PointerEventData data )
		{
			if (_debug)
			{
				Debug.LogFormat( _stringView, "STRUM OnPointerEnter: {0}\n{1}", _stringView.cachedTransform.GetPathInHierarchy( ), data.position );
			}
			PointerStartHelper(ENTER_REASON, data );
		}

		private void PointerStartHelper(string reason, UnityEngine.EventSystems.PointerEventData data )
		{
			if (_isDown)
			{
				Debug.LogErrorFormat( _stringView, "STRUM ({1}) PointerStartHelper when already down on {0}", 
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

					if (_debug)
					{
						Debug.LogWarningFormat( "STRUM ({4}) String Hit {0} at {1} which is Fret {2} at d = {3}",
							hitInfo.collider.transform.GetPathInHierarchy( ),
							hitInfo.point,
							fret, d ,
							reason);
					}
					if (fret == int.MaxValue)
					{
						_isDown = true;
						_downTime = Time.time;
						_downLocation = hitInfo.point;
					}
					else
					{
						if (fret == 0)
						{
							if (_stringView.stringBehaviour.Fret == 0)
							{
								if (reason != DOWN_REASON)
								{
									return;
								}
								fret = -1;
							}
							else if (_stringView.stringBehaviour.IsDamped)
							{
								if (reason != DOWN_REASON)
								{
									return;
								}
							}
						}
						_stringView.stringBehaviour.SetFret( fret );
					}

				}
			}
		}

		public override void OnDrag( PointerEventData data )
		{
			int fret = 0;
			RaycastHit hitInfo;
			if (Physics.Raycast( Camera.main.ScreenPointToRay( data.position ), out hitInfo, 100, _fretboardLayerMask ))
			{
				{
					float d = 0f;
					fret = _stringView.guitarView.GetFretForWorldX( hitInfo.point.x, ref d );

					if (_debug)
					{
						Debug.LogWarningFormat( "STRUM () String Drag {0} at {1} which is Fret {2} at d = {3}",
							hitInfo.collider.transform.GetPathInHierarchy( ),
							hitInfo.point,
							fret, d);
					}
					if (fret == int.MaxValue)
					{
						// do nothing
					}
					else
					{
						if (fret != _stringView.stringBehaviour.Fret)
						{
							_stringView.stringBehaviour.SetFret( fret );
						}
					}

				}
			}
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
