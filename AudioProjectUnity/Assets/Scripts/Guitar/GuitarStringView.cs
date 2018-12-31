using UnityEngine;
using RJWS.Core.TransformExtensions;
using System;
using UnityEngine.EventSystems;

namespace RJWS.Audio
{
	public class GuitarStringView : MonoBehaviour, IGuitarStringPlucker
	{
		public bool debugMe = true;
		static public readonly bool DEBUG_STRINGVIEW = true;
		public bool debughit = false;

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
			_stringCollider = stringObject.GetComponent<CapsuleCollider>( );
		}

		public GameObject stringObject;
		private CapsuleCollider _stringCollider;

		public AudioStringBehaviour stringBehaviour
		{
			get;
			private set;
		}

		public GuitarView guitarView
		{
			get;
			private set;
		}

		IGuitarStringPlucker _plucker;

		public EPluckerType pluckerType = EPluckerType.BasicUp;
		public void ChangePluckerType(EPluckerType p)
		{
			if (p != pluckerType || _plucker == null)
			{
				pluckerType = p;
				MakePlucker( );
			}
			else
			{
				Debug.LogFormat( "Not changing: plucker lready of type {0}", p );
			}
		}

		public void Init( GuitarView gv, GuitarModel model, int stringNum)
		{
			debughit |= gv.debugHit;
			pluckerType = gv.pluckerType;
			guitarView = gv;
			stringBehaviour = model.GetString( stringNum );
			
			stringObject.transform.localScale = guitarView.StringDims;

			float maxRad = 0.5f * guitarView.stringSeparation / guitarView.stringWidth;
			float collW = Mathf.Lerp( 1f, maxRad, guitarView.guitarSettings.stringColliderSize );

			if (collW < 1f)
			{
				Debug.LogWarningFormat( "Collider too small at {0} = lerp(1f, {1}, {2}", collW, maxRad, guitarView.guitarSettings.stringColliderSize );
				collW = 1f;
			}
			_stringCollider.radius = collW;

			gameObject.SetActive( true );

			MakePlucker( );
		}

		public void ApplyGuitarSettings(GuitarSettings gs)
		{
			ChangePluckerType( gs.pluckerType );
			stringBehaviour.UseReverb( gs.useReverb );
			stringBehaviour.SetAttenuation( gs.attenuation );
		}

		private void MakePlucker()
		{
			_plucker = PluckerHelpers.CreatePluckerOfType( pluckerType, this, debughit );
			Debug.LogFormat( "Created plucker of type {0} for {1}", pluckerType, gameObject.name );
		}

		public void OnPointerClick( PointerEventData eventData )
		{
			_plucker.OnPointerClick( eventData );
		}

		public void OnPointerDown( PointerEventData eventData )
		{
			_plucker.OnPointerDown( eventData );
		}

		public void OnPointerUp( PointerEventData eventData )
		{
			_plucker.OnPointerUp( eventData );
		}

		public void OnBeginDrag( PointerEventData eventData )
		{
			_plucker.OnBeginDrag( eventData );
		}

		public void OnEndDrag( PointerEventData eventData )
		{
			_plucker.OnEndDrag( eventData );
		}

		public void OnDrag( PointerEventData eventData )
		{
			_plucker.OnDrag( eventData );
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			_plucker.OnPointerExit( eventData );
		}

		public void OnPointerEnter( PointerEventData eventData )
		{
			_plucker.OnPointerEnter( eventData );
		}
	}
}

