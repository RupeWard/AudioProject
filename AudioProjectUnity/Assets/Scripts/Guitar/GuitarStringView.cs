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

		private void Awake( )
		{
			cachedTransform = transform;
		}

		public GameObject stringObject;

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

		EPluckerType _pluckerType = EPluckerType.BasicUp;

		public void Init( GuitarView gv, GuitarModel model, int stringNum)
		{
			guitarView = gv;
			stringBehaviour = model.GetString( stringNum );
			stringObject.transform.localScale = guitarView.StringDims;

			gameObject.SetActive( true );

			MakePlucker( );
		}

		private void MakePlucker()
		{
			_plucker = PluckerHelpers.CreatePluckerOfType( _pluckerType, this, debughit );
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

	}
}

