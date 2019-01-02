﻿using UnityEngine;
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

		public RectTransform fretIndicatorRT;

		private void Awake( )
		{
			cachedTransform = transform;
			_stringCollider = stringObject.GetComponent<CapsuleCollider>( );
			_stringRenderer = stringObject.GetComponent<MeshRenderer>( );
		}

		public GameObject stringObject;
		private CapsuleCollider _stringCollider;
		private MeshRenderer _stringRenderer;

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

		private Material _stringMaterial;

		IGuitarStringPlucker _plucker;

		public UnityEngine.UI.Image fretMarkerImage;

		public void SetFretMarker(int i)
		{
			Debug.LogWarningFormat( "{0} Changed fret to {1}", gameObject.name, i );
			Texture2D sprite = guitarView.guitarSettings.FretMarker( i );
			fretMarkerImage.sprite = Sprite.Create( sprite, fretMarkerImage.sprite.rect, fretMarkerImage.sprite.pivot);
		}

		private static readonly bool DEBUG_TONE = false;

		private void Update()
		{
			if (stringBehaviour != null)
			{
				if (stringBehaviour.Amplitude( ) > guitarView.guitarSettings.minToColourString)
				{
					_stringMaterial.color = Color.Lerp( guitarView.guitarSettings.minVolColour, guitarView.guitarSettings.maxVolColour, stringBehaviour.Amplitude( ) / 0.5f );
					if (DEBUG_TONE)
					{
						Debug.LogFormat( "({0}) - {1}: {2} - threshold = {3}",
							Time.time,
							gameObject.name,
							stringBehaviour.Amplitude( ).ToString( "F6" ),
							guitarView.guitarSettings.zeroThreshold
							);
					}
					fretMarkerImage.enabled = true;
				}
				else
				{
					_stringMaterial.color = guitarView.guitarSettings.idleColour;
					fretMarkerImage.enabled = false;
				}
			}
		}

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
			stringBehaviour.onFretChanged -= HandleFretChanged;
			stringBehaviour.onFretChanged += HandleFretChanged;

			stringObject.transform.localScale = guitarView.StringDims;
			fretIndicatorRT.anchoredPosition = new Vector2( 0f, 1.025f ); // MAGIC NUMBER - DERIVE IT!
			_stringMaterial =  new Material( guitarView.guitarSettings.stringMaterial );
			_stringRenderer.material = _stringMaterial;

			SetStringColliderSize( );

			ApplyGuitarSettings( guitarView.guitarSettings);

			gameObject.SetActive( true );

			MakePlucker( );
		}

		private void OnDestroy()
		{
			stringBehaviour.onFretChanged -= HandleFretChanged;
		}

		public void HandleFretChanged(int i)
		{
			SetFretMarker( i );
		}

		private void SetStringColliderSize()
		{
			float maxRad = 0.5f * guitarView.stringSeparation / guitarView.stringWidth;
			float collW = Mathf.Lerp( 1f, maxRad, guitarView.guitarSettings.stringColliderSize );

			if (collW < 1f)
			{
				Debug.LogWarningFormat( "Collider too small at {0} = lerp(1f, {1}, {2}", collW, maxRad, guitarView.guitarSettings.stringColliderSize );
				collW = 1f;
			}
			_stringCollider.radius = collW;
		}

		public void ApplyGuitarSettings(GuitarSettings gs)
		{ 
			ChangePluckerType( gs.pluckerType );
			stringBehaviour.UseReverb( gs.useReverb );
			stringBehaviour.SetAttenuation( gs.attenuation );
			stringBehaviour.SetZeroThreshold( gs.zeroThreshold );
			SetStringColliderSize( );
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

