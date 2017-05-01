using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Graph
{
	public class GraphScrollBarEnd : MonoBehaviour
	{
		public RectTransform bgRT;

		private ELowHigh _end;

		public RectTransform cachedRT
		{
			private set;
			get;
		}

		private GraphScrollBar _graphScrollBar;

		private void Awake( )
		{
			cachedRT = GetComponent<RectTransform>( );
		}

		public void Init( GraphScrollBar gsb, ELowHigh ee)
		{
			_graphScrollBar = gsb;
			_end = ee;

			gameObject.name = "ScrollEnd_"+ ee.ToString( );

			cachedRT.SetParent( _graphScrollBar.cachedRT );
			cachedRT.localScale = new Vector3( 1f, 1f, 1f );
			SetPos( );
		}

		private void SetPos()
		{
			float height = _graphScrollBar.cachedRT.rect.height;
			switch (_end)
			{
				case ELowHigh.Low:
					{
						cachedRT.anchorMin = new Vector2( 0f, 0.5f );
						cachedRT.anchorMax = new Vector2( 0f, 0.5f );
						cachedRT.pivot = new Vector2( 0f, 0.5f );

						bgRT.localRotation = Quaternion.Euler( 0f, 0f, 180f );
						break;
					}
				case ELowHigh.High:
					{
						cachedRT.anchorMin = new Vector2( 1f, 0.5f );
						cachedRT.anchorMax = new Vector2( 1f, 0.5f );
						cachedRT.pivot = new Vector2( 1f, 0.5f );

						bgRT.localRotation = Quaternion.Euler( 0f, 0f, 0f );
						break;
					}
			}
			cachedRT.anchoredPosition = Vector2.zero;
			cachedRT.sizeDelta = new Vector2( 0.5f * height, height );
		}
	}

}
