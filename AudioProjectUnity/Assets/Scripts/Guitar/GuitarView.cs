using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Audio
{
	public class GuitarView : MonoBehaviour
	{
		public GuitarStringView guitarStringViewPrefab;
		public float stringSeparation = 1f;
		public float stringLength = 15f;
		public float stringWidth = 0.1f;

		private List<GuitarStringView> _stringViews = new List<GuitarStringView>( );

		public Transform cachedTransform
		{
			get;
			private set;
		}

		private void Awake()
		{
			cachedTransform = transform;
		}

		public void Init(GuitarModel model)
		{
			if (_stringViews.Count > 0)
			{
				for (int i = 0; i < _stringViews.Count; i++)
				{
					GameObject.Destroy( _stringViews[i].gameObject );
				}
				_stringViews.Clear( );
			}
			for (int i = 0; i < model.NumStrings; i++)
			{
				GuitarStringView gsv = Instantiate( guitarStringViewPrefab );
				gsv.cachedTransform.SetParent( cachedTransform );
				gsv.gameObject.name = "String_" + i;
				gsv.cachedTransform.localPosition = new Vector3( 0f, i * stringSeparation, 0f );

				gsv.Init( this, model, i );
			}
		}

		public Vector3 StringDims
		{
			get
			{
				return new Vector3( stringWidth, stringLength, stringWidth);
			}
		}
	}

}
