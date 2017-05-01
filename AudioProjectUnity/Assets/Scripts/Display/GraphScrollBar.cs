using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Graph
{
	public class GraphScrollBar : MonoBehaviour
	{
		private Dictionary<ELowHigh, GraphScrollBarEnd> _ends = new Dictionary<ELowHigh, GraphScrollBarEnd>( );

		public RectTransform cachedRT
		{
			private set;
			get;
		}

		private GraphScrollBarPanel _scrollBarPanel;

		private void Awake()
		{
			cachedRT = GetComponent<RectTransform>( );
		}

		public void Init( GraphScrollBarPanel sbp)
		{
			_scrollBarPanel = sbp;
			GameObject endPrefab = Resources.Load<GameObject>( "Graph/Prefabs/ScrollBarEnd" );

			foreach (ELowHigh eend in System.Enum.GetValues( typeof( ELowHigh ) ))
			{
				GameObject go = GameObject.Instantiate( endPrefab );
				_ends[eend] = go.GetComponent<GraphScrollBarEnd>( );
				_ends[eend].Init( this, eend );
			}
		}
	}

}
