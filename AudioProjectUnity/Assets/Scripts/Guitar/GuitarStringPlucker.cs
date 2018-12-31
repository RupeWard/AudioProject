using UnityEngine.EventSystems;
using UnityEngine;
using RJWS.Core.TransformExtensions;
using RJWS.Core.DebugDescribable;

namespace RJWS.Audio
{
	public interface IGuitarStringPlucker : 
		IPointerClickHandler, 
		IPointerDownHandler, 
		IPointerUpHandler,
		IBeginDragHandler, 
		IEndDragHandler, 
		IDragHandler
	{


	}

	public enum EPluckerType
	{
		BasicUp
	}

	public static class PluckerHelpers
	{
		public static IGuitarStringPlucker CreatePluckerOfType(EPluckerType t, GuitarStringView gsv, bool debug = false)
		{
			switch (t)
			{
				case EPluckerType.BasicUp:
					{
						return new BasicUpPlucker( gsv, debug );
					}
				default:
					{
						throw new System.Exception( "Unhanlded type: " + t );
					}
			}
		}
	}

	public abstract class GuitarStringPlucker_Base :IGuitarStringPlucker
	{
		protected GuitarStringView _stringView;
		protected bool _debug = false;

		public GuitarStringPlucker_Base(GuitarStringView s, bool b)
		{
			_stringView = s;
			_debug = b;
		}

		public virtual void OnBeginDrag( PointerEventData eventData )
		{
		}

		public virtual void OnDrag( PointerEventData eventData )
		{
		}

		public virtual void OnDrop( PointerEventData eventData )
		{
		}

		public virtual void OnEndDrag( PointerEventData eventData )
		{
		}

		public virtual void OnPointerClick( PointerEventData eventData )
		{
		}

		public virtual void OnPointerDown( PointerEventData eventData )
		{
		}

		public virtual void OnPointerUp( PointerEventData eventData )
		{
		}
	}

	public class BasicUpPlucker : GuitarStringPlucker_Base
	{
		public BasicUpPlucker( GuitarStringView s, bool b)
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
			_stringView.stringBehaviour.Pluck( fret );
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
