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

		public GraphScrollBarPanel scrollBarPanel
		{
			get;
			private set;
		}

		private void Awake()
		{
			cachedRT = GetComponent<RectTransform>( );
		}

		public void Init( GraphScrollBarPanel sbp)
		{
			scrollBarPanel = sbp;
			GameObject endPrefab = Resources.Load<GameObject>( "Graph/Prefabs/ScrollBarEnd" );

			cachedRT.sizeDelta = scrollBarPanel.cachedRT.sizeDelta;
			foreach (ELowHigh eend in System.Enum.GetValues( typeof( ELowHigh ) ))
			{
				GameObject go = GameObject.Instantiate( endPrefab );
				_ends[eend] = go.GetComponent<GraphScrollBarEnd>( );
				_ends[eend].Init( this, eend );
			}
		}

		public static readonly bool DEBUG_SCROLLBAR = true;

		public void HandleEndMoved( ELowHigh lowHigh, float delta)
		{
			if (DEBUG_SCROLLBAR)
			{
				Debug.Log( "SB moved: " + lowHigh + " " + delta );
			}

			Vector2 size = cachedRT.sizeDelta;
			Vector2 anchoredPos = cachedRT.anchoredPosition;

			if (lowHigh == ELowHigh.Low)
			{
				size.x -= delta;
				anchoredPos.x += 0.5f * delta;
			}
			else
			{
				size.x += delta;
				anchoredPos.x += 0.5f * delta;
			}

			cachedRT.sizeDelta = size;
			cachedRT.anchoredPosition = anchoredPos;
		}
	}

}
