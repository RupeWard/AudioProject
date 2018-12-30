using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Audio
{
	public class GuitarStringView : MonoBehaviour
	{
		public Transform cachedTransform
		{
			get;
			private set;
		}

		private void Awake( )
		{
			cachedTransform = transform;
		}

		public Transform stringView;

		private AudioStringBehaviour _stringBehaviour;
		private GuitarView _guitarView;

		public void Init( GuitarView gv, GuitarModel model, int stringNum)
		{
			_guitarView = gv;
			_stringBehaviour = model.GetString( stringNum );
			stringView.localScale = _guitarView.StringDims;

			gameObject.SetActive( true );
		}
	}
}

