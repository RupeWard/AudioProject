using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Graph
{
	public class GraphScrollBar : MonoBehaviour
	{
		private Dictionary<GraphScrollBarEnd.EEnd, GraphScrollBarEnd> _ends = new Dictionary<GraphScrollBarEnd.EEnd, GraphScrollBarEnd>( );
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
			GameObject endPrefab = Resources.Load<GameObject>( "Graph/Prefabs/ScrollBarEnd" );

			foreach (GraphScrollBarEnd.EEnd eend in System.Enum.GetValues( typeof( GraphScrollBarEnd.EEnd ) ))
			{
				GameObject go = GameObject.Instantiate( endPrefab );
				_ends[eend] = go.GetComponent<GraphScrollBarEnd>( );
				_ends[eend].Init( this, eend );
			}
		}
	}

}
