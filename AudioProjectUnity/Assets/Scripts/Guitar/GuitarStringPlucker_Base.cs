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
		IDragHandler,
		IPointerExitHandler,
		IPointerEnterHandler
	{


	}

	public enum EPluckerType
	{
		BasicUp,
		BasicDrag
	}

	public static class PluckerHelpers
	{
		public static IGuitarStringPlucker CreatePluckerOfType(EPluckerType t, GuitarStringView gsv, bool debug = false)
		{
			switch (t)
			{
				case EPluckerType.BasicUp:
					{
						return new GuitarStringPlucker_BasicUp( gsv, debug );
					}
				case EPluckerType.BasicDrag:
					{
						return new GuitarStringPlucker_BasicDrag( gsv, debug );
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

		public virtual void OnPointerExit( PointerEventData eventData )
		{
		}

		public virtual void OnPointerEnter( PointerEventData eventData )
		{
		}
	}

}
