using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Graph
{
	public class GraphPanel: MonoBehaviour
	{
		static readonly bool DEBUG_GRAPHPANEL = false;

		public GraphPanelSettings graphPanelSettings = new GraphPanelSettings( );

		private Dictionary<EOrthoDirection, GraphScrollBarPanel> _scrollBars = new Dictionary<EOrthoDirection, GraphScrollBarPanel>( );

		public RectTransform graphViewPanelRT;
		public Transform scrollBarContainer;
		public GraphViewPanel graphViewPanel;
		public UnityEngine.UI.Button cancelGrabButton;

		public bool InitOnAwake = false;

		public RectTransform cachedRT
		{
			private set;
			get;
		}

		public void HandleCancelGrabButtonClicked()
		{
			ObjectGrabManager.Instance.CancelCurrentGrab( );
		}

		private void Awake()
		{
			cachedRT = GetComponent<RectTransform>( );
			if (InitOnAwake)
			{
				Init( );
			}
		}

		private void Start()
		{
		}

		private GameObject _scrollPrefab = null;

		public void Init()
		{
			SetUpScrollBars( );
		}

		public GraphScrollBarPanel GetScrollBar(EOrthoDirection dirn)
		{
			GraphScrollBarPanel result;
			if (false == _scrollBars.TryGetValue(dirn, out result))
			{
				if (_scrollPrefab == null)
				{
					_scrollPrefab = Resources.Load<GameObject>( "Graph/Prefabs/ScrollBarPanel" );
				}
				GameObject go = GameObject.Instantiate( _scrollPrefab );
				result = go.GetComponent<GraphScrollBarPanel>( );
				result.Init( this, dirn );
				_scrollBars.Add(dirn, result);
				if (DEBUG_GRAPHPANEL)
				{
//					Debug.LogWarning( "Created scroll bar: " + dirn );
				}
			}
			return result;
		}

		public void HandleScrollBarChanged( EOrthoDirection direction, float sizeFraction, float posFraction)
		{
			graphViewPanel.HandleViewChange( direction, sizeFraction, posFraction );
		}

		public void SetUpScrollBars()
		{
			GetScrollBar( EOrthoDirection.Horizontal).SetUp( );
			GetScrollBar( EOrthoDirection.Vertical).SetUp( );

			float horSbWidth = graphPanelSettings.scrollSettings.GetScrollBarWidth( EOrthoDirection.Horizontal );
			float vertSbWidth = graphPanelSettings.scrollSettings.GetScrollBarWidth( EOrthoDirection.Vertical);

			graphViewPanelRT.sizeDelta =
				new Vector2(
					cachedRT.rect.width - vertSbWidth,
					cachedRT.rect.height - horSbWidth );
			Vector2 anchoredPos = Vector2.zero;

			switch(graphPanelSettings.scrollSettings.GetScrollBarPosition(EOrthoDirection.Horizontal))
			{
				case ELowHigh.Low:
				{
					anchoredPos.y += 0.5f * horSbWidth;
					break;
				}
				case ELowHigh.High:
				{
					anchoredPos.y -= 0.5f * horSbWidth;
					break;
				}
				default:
				{
					Debug.LogError( "Bad ELowHigh = " + graphPanelSettings.scrollSettings.GetScrollBarPosition(EOrthoDirection.Horizontal) );
					break;
				}
			}
			switch (graphPanelSettings.scrollSettings.GetScrollBarPosition(EOrthoDirection.Vertical))
			{
				case ELowHigh.Low:
				{
					anchoredPos.x += 0.5f * vertSbWidth;
					break;
				}
				case ELowHigh.High:
				{
					anchoredPos.x -= 0.5f * vertSbWidth;
					break;
				}
				default:
				{
					Debug.LogError( "Bad ELowHigh = " + graphPanelSettings.scrollSettings.GetScrollBarPosition(EOrthoDirection.Vertical) );
					break;
				}
			}
			graphViewPanelRT.anchoredPosition = anchoredPos;
			graphViewPanel.InitContent( );

			SetUpCancelGrabButton( );
        }

		public void SetUpCancelGrabButton( )
		{
			if (ObjectGrabManager.Instance.showCancelGrabButton)
			{
				if (cancelGrabButton == null)
				{
					Debug.LogError( "No cancelGrabButton to set up" );
				}
				else
				{
					ObjectGrabManager.Instance.SetCancelGrabButton( cancelGrabButton);

					RectTransform cancelGrabButtonRT = cancelGrabButton.GetComponent<RectTransform>( );

					System.Text.StringBuilder sb = null;
					if (DEBUG_GRAPHPANEL)
					{
						sb = new System.Text.StringBuilder( );
						sb.Append( "GP: SetUpCancelGrab" );
						foreach (KeyValuePair<EOrthoDirection, GraphScrollBarPanel> kvp in _scrollBars)
						{
							sb.Append( "\n-" ).Append( kvp.Key ).Append( " " ).Append( kvp.Value.cachedRT.rect );
						}
					}
					cancelGrabButtonRT.sizeDelta =
						new Vector2(
							_scrollBars[EOrthoDirection.Vertical].cachedRT.rect.height,
							_scrollBars[EOrthoDirection.Horizontal].cachedRT.rect.height );

					Vector2 anchorMin = Vector2.zero;
					Vector2 anchorMax = Vector2.zero;
					Vector2 pivot = Vector2.zero;

					if (_scrollBars[EOrthoDirection.Horizontal].ePosition== ELowHigh.Low)
					{
						anchorMin.x = 1f;
						anchorMax.x = 1f;
						pivot.x = 1f;
					}
					if (_scrollBars[EOrthoDirection.Vertical].ePosition == ELowHigh.Low)
					{
						anchorMin.y = 1f;
						anchorMax.y = 1f;
						pivot.y = 1f;
					}
					cancelGrabButtonRT.anchorMin = anchorMin;
					cancelGrabButtonRT.anchorMax = anchorMax;
					cancelGrabButtonRT.pivot = pivot;
					cancelGrabButtonRT.anchoredPosition = Vector2.zero;
					if (sb != null)
					{
						sb.Append( "\n sizeDelta = " + cancelGrabButtonRT.sizeDelta );
						sb.Append( "\n anchored Pos = " + cancelGrabButtonRT.anchoredPosition );
						sb.Append( "\n rect = " + cancelGrabButtonRT.rect );
						Debug.Log( sb.ToString( ) );
					}
				}
			}
			if (cancelGrabButton != null)
			{
				cancelGrabButton.gameObject.SetActive( false );
			}
		}

	}

}
