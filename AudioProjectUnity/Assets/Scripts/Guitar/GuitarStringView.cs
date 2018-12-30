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

		public bool debughit = false;
		public void OnPointerUp( UnityEngine.EventSystems.PointerEventData data )
		{
			if (debughit)
			{
				Debug.LogFormat( this, "OnPointerUp: {0}\n{1}", transform.GetPathInHierarchy( ), data.position);
			}
			int fret = 0;
			RaycastHit hitInfo;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(data.position), out hitInfo, 100))
			{
				if (hitInfo.collider.gameObject == stringObject)
				{
					float d=0f;
					fret = _guitarView.GetFretForWorldX( hitInfo.point.x, ref d );

					if (debughit)
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
			_stringBehaviour.Pluck( fret );
		}

		public void OnPointerDown( UnityEngine.EventSystems.PointerEventData data )
		{
			if (debughit)
			{
				Debug.LogFormat( this, "OnPointerDown: {0}\n{1}", transform.GetPathInHierarchy( ), data.position );
			}
		}

	}
}

