using UnityEngine;
using RJWS.Core.TransformExtensions;

namespace RJWS.Audio
{
	public class GuitarStringView : MonoBehaviour, UnityEngine.EventSystems.IPointerUpHandler, UnityEngine.EventSystems.IPointerDownHandler
	{
		public bool debugMe = true;
		static public readonly bool DEBUG_STRINGVIEW = true;

		public bool DebugMe
		{
			get { return debugMe || DEBUG_STRINGVIEW; }
		}

		public Transform cachedTransform
		{
			get;
			private set;
		}

		private void Awake( )
		{
			cachedTransform = transform;
		}

		public GameObject stringObject;

		private AudioStringBehaviour _stringBehaviour;
		private GuitarView _guitarView;

		public void Init( GuitarView gv, GuitarModel model, int stringNum)
		{
			_guitarView = gv;
			_stringBehaviour = model.GetString( stringNum );
			stringObject.transform.localScale = _guitarView.StringDims;

			gameObject.SetActive( true );
		}

		public void OnPointerUp( UnityEngine.EventSystems.PointerEventData data )
		{
			if (DebugMe)
			{
				Debug.LogFormat( this, "OnPointerUp: {0}\n{1}", transform.GetPathInHierarchy( ), data.pointerCurrentRaycast.gameObject.name );
			}
			_stringBehaviour.Pluck( );
		}

		public void OnPointerDown( UnityEngine.EventSystems.PointerEventData data )
		{
			if (DebugMe)
			{
				Debug.LogFormat( this, "OnPointerDown: {0}\n{1}", transform.GetPathInHierarchy( ), data.pointerCurrentRaycast.gameObject.name );
			}
		}

	}
}

