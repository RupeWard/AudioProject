using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Graph
{
	public class GraphPanel: MonoBehaviour
	{
		static private Dictionary<EOrthoDirection, ELowHigh> s_defaultPositions = new Dictionary<EOrthoDirection, ELowHigh>( )
		{
			{ EOrthoDirection.Horizontal, ELowHigh.Low },
			{ EOrthoDirection.Vertical, ELowHigh.High }
		};

		private Dictionary<EOrthoDirection, GraphScrollBarPanel> _scrollBars = new Dictionary<EOrthoDirection, GraphScrollBarPanel>( );

		public RectTransform cachedRT
		{
			private set;
			get;
		}

		private void Awake()
		{
			cachedRT = GetComponent<RectTransform>( );
			Init( );
		}

		public void Init()
		{
			GameObject scrollPrefab = Resources.Load<GameObject>( "Graph/Prefabs/ScrollBarPanel" );

			foreach (EOrthoDirection edirn in System.Enum.GetValues( typeof( EOrthoDirection) ))
			{
				GameObject go = GameObject.Instantiate( scrollPrefab );
				_scrollBars[edirn] = go.GetComponent<GraphScrollBarPanel>( );
				_scrollBars[edirn].Init( this, edirn, s_defaultPositions[ edirn] );
			}

		}
	}

}
