using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Audio
{
	public class GuitarFretView : MonoBehaviour
	{
		public Transform cachedTransform
		{
			get;
			private set;
		}

		public Transform fretView;

		private GuitarView _guitarView;

		private void Awake( )
		{
			cachedTransform = transform;
		}

		public void Init(GuitarView gv, int fn)
		{
			_guitarView = gv;
			FretNum = fn;

			if (fn == 0)
			{
				fretView.gameObject.GetComponent<MeshRenderer>( ).material = _guitarView.guitarSettings.bridgeMaterial;
			}
			else
			{
				fretView.gameObject.GetComponent<MeshRenderer>( ).material = _guitarView.guitarSettings.fretMaterial;
			}
			fretView.localScale = new Vector3( _guitarView.fretWidth, 0.5f * _guitarView.FretLength, _guitarView.fretWidth );
			cachedTransform.localPosition = new Vector3( cachedTransform.localPosition.x + _guitarView.FretX( fn ), 0.5f * 0.5f * _guitarView.FretLength + _guitarView.stringSeparation, cachedTransform.localPosition.z + _guitarView.fretDepth );
		}

		public int FretNum
		{
			get;
			private set;
		}

	}
}
