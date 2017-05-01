using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Graph
{
	public class GraphPanel: MonoBehaviour
	{
		public class ScrollBarSettings
		{
			public Dictionary<EOrthoDirection, ELowHigh> positions = new Dictionary<EOrthoDirection, ELowHigh>( )
			{
				{ EOrthoDirection.Horizontal, ELowHigh.Low},
				{ EOrthoDirection.Vertical, ELowHigh.High }
			};

			public Dictionary<EOrthoDirection, float> sizes = new Dictionary<EOrthoDirection, float>( )
			{
				{ EOrthoDirection.Horizontal, 40f},
				{ EOrthoDirection.Vertical, 60f }
			};
		}

		public ScrollBarSettings scrollBarSettings
		{
			get;
			private set;
		}

		private Dictionary<EOrthoDirection, GraphScrollBarPanel> _scrollBars = new Dictionary<EOrthoDirection, GraphScrollBarPanel>( );

		public RectTransform cachedRT
		{
			private set;
			get;
		}

		private void Awake()
		{
			cachedRT = GetComponent<RectTransform>( );
			scrollBarSettings = new ScrollBarSettings( );
			Init( );
		}

		public void Init()
		{
			GameObject scrollPrefab = Resources.Load<GameObject>( "Graph/Prefabs/ScrollBarPanel" );

			foreach (EOrthoDirection edirn in System.Enum.GetValues( typeof( EOrthoDirection) ))
			{
				GameObject go = GameObject.Instantiate( scrollPrefab );
				_scrollBars[edirn] = go.GetComponent<GraphScrollBarPanel>( );
				_scrollBars[edirn].Init( this, edirn);
			}

		}
	}

}
