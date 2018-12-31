using UnityEngine.EventSystems;
using UnityEngine;
using RJWS.Core.TransformExtensions;
using RJWS.Core.DebugDescribable;

namespace RJWS.Audio
{
	public class GuitarStringPlucker_BasicUp : GuitarStringPlucker_Base
	{
		public GuitarStringPlucker_BasicUp( GuitarStringView s, bool b)
			: base( s, b )
		{

		}

		public override void OnPointerUp( UnityEngine.EventSystems.PointerEventData data )
		{
			if (_debug)
			{
				Debug.LogFormat( _stringView, "OnPointerUp: {0}\n{1}", _stringView.cachedTransform.GetPathInHierarchy( ), data.position );
			}
			int fret = 0;
			RaycastHit hitInfo;
			if (Physics.Raycast( Camera.main.ScreenPointToRay( data.position ), out hitInfo, 100 ))
			{
				if (hitInfo.collider.gameObject == _stringView.stringObject)
				{
					float d = 0f;
					fret = _stringView.guitarView.GetFretForWorldX( hitInfo.point.x, ref d );

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
					Debug.LogErrorFormat( "String Hit {0} at {1}", hitInfo.collider.transform.GetPathInHierarchy( ), hitInfo.point );
				}
			}
			_stringView.stringBehaviour.Pluck(1f, fret );
		}

		public override void OnPointerDown( UnityEngine.EventSystems.PointerEventData data )
		{
			if (_debug)
			{
				Debug.LogFormat( _stringView, "OnPointerDown: {0}\n{1}", _stringView.cachedTransform.GetPathInHierarchy( ), data.position );
			}
		}

	}
}
