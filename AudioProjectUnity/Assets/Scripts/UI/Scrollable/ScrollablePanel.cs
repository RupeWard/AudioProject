using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.UI.Scrollable
{
	public class ScrollablePanel: MonoBehaviour
	{
		static readonly bool DEBUG_GRAPHPANEL = false;

		public ScrollablePanelSettings settings = new ScrollablePanelSettings( );

		private Dictionary<EOrthoDirection, ScrollableScrollBarPanel> _scrollBars = new Dictionary<EOrthoDirection, ScrollableScrollBarPanel>( );

		public Transform scrollBarContainer;
		public ScrollablePanelView scrollablePanelView;
		public UnityEngine.UI.Button cancelGrabButton;
		public RectTransform cancelGrabButtonHiderRT;

		public ObjectGrabber grabber;
		public GameObject grabberLeft;
		public GameObject grabberRight;
		public GameObject grabberUp;
		public GameObject grabberDown;

		public RectTransform overlaysPanel;
		public RectTransform verticalOverlaysPanel;
		public RectTransform horizontalOverlaysPanel;

		public GameObject leftLabelPanel;
		public GameObject rightLabelPanel;
		public GameObject topLabelPanel;
		public GameObject bottomLabelPanel;

		private UnityEngine.UI.Text _leftLabelText;
		private UnityEngine.UI.Text _rightLabelText;
		private UnityEngine.UI.Text _topLabelText;
		private UnityEngine.UI.Text _bottomLabelText;


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
			_leftLabelText = leftLabelPanel.GetComponentInChildren<UnityEngine.UI.Text>( );
			_rightLabelText = rightLabelPanel.GetComponentInChildren<UnityEngine.UI.Text>( );
			_topLabelText = topLabelPanel.GetComponentInChildren<UnityEngine.UI.Text>( );
			_bottomLabelText = bottomLabelPanel.GetComponentInChildren<UnityEngine.UI.Text>( );

			leftLabelPanel.SetActive( false );
			rightLabelPanel.SetActive( false );
			topLabelPanel.SetActive( false );
			bottomLabelPanel.SetActive( false );

			if (InitOnAwake)
			{
				Init( );
			}
		}

		public void SetLeftLabel(string str = null)
		{
			if (string.IsNullOrEmpty( str ))
			{
				leftLabelPanel.SetActive( false );
			}
			else
			{
				leftLabelPanel.SetActive( true );
				_leftLabelText.text = str;
			}
		}

		public void SetRightLabel( string str = null )
		{
			if (string.IsNullOrEmpty(str))
			{
				rightLabelPanel.SetActive( false );
			}
			else
			{
				rightLabelPanel.SetActive( true );
				_rightLabelText.text = str;
			}
		}

		public void SetTopLabel( string str = null )
		{
			if (string.IsNullOrEmpty(str))
			{
				topLabelPanel.SetActive( false );
			}
			else
			{
				topLabelPanel.SetActive( true );
				_topLabelText.text = str;
			}
		}

		public void SetBottomLabel( string str )
		{
			if (string.IsNullOrEmpty(str))
			{
				bottomLabelPanel.SetActive( false );
			}
			else
			{
				bottomLabelPanel.SetActive( true );
				_bottomLabelText.text = str;
			}
		}

		private void Start()
		{
			grabber.onMovementAction += HandleGrabberMoved;
			grabber.onActivateAction += HandleGrabberActivated;

			ActivateGrabberArrows( false );
		}

		private GameObject _scrollPrefab = null;

		public void Init()
		{
			SetUpScrollBars( );
		}

		public ScrollableScrollBarPanel GetScrollBar(EOrthoDirection dirn)
		{
			ScrollableScrollBarPanel result;
			if (false == _scrollBars.TryGetValue(dirn, out result))
			{
				if (_scrollPrefab == null)
				{
					_scrollPrefab = Resources.Load<GameObject>( "Prefabs/UI/ScrollBarPanel" );
				}
				GameObject go = GameObject.Instantiate( _scrollPrefab );
				result = go.GetComponent<ScrollableScrollBarPanel>( );
				result.Init( this, dirn, settings.scrollSettings.GetScrollBarSettings( dirn ) );
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
			scrollablePanelView.HandleViewChange( direction, sizeFraction, posFraction );
			verticalOverlaysPanel.sizeDelta = new Vector2( verticalOverlaysPanel.sizeDelta.x, scrollablePanelView.contentPanelRT.sizeDelta.y);
			verticalOverlaysPanel.anchoredPosition = new Vector2(0f, scrollablePanelView.contentPanelRT.anchoredPosition.y );
			horizontalOverlaysPanel.sizeDelta = new Vector2( scrollablePanelView.contentPanelRT.sizeDelta.x, horizontalOverlaysPanel.sizeDelta.y);
			horizontalOverlaysPanel.anchoredPosition = new Vector2( scrollablePanelView.contentPanelRT.anchoredPosition.x, 0f);
		}

		public void SetUpScrollBars()
		{
			GetScrollBar( EOrthoDirection.Horizontal).SetUp( );
			GetScrollBar( EOrthoDirection.Vertical).SetUp( );

			float horSbWidth = settings.scrollSettings.GetScrollBarWidth( EOrthoDirection.Horizontal );
			float vertSbWidth = settings.scrollSettings.GetScrollBarWidth( EOrthoDirection.Vertical);

			scrollablePanelView.cachedRT.sizeDelta =
				new Vector2(
					cachedRT.rect.width - vertSbWidth,
					cachedRT.rect.height - horSbWidth );
			Vector2 anchoredPos = Vector2.zero;

			switch(settings.scrollSettings.GetScrollBarPosition(EOrthoDirection.Horizontal))
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
					Debug.LogError( "Bad ELowHigh = " + settings.scrollSettings.GetScrollBarPosition(EOrthoDirection.Horizontal) );
					break;
				}
			}
			switch (settings.scrollSettings.GetScrollBarPosition(EOrthoDirection.Vertical))
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
					Debug.LogError( "Bad ELowHigh = " + settings.scrollSettings.GetScrollBarPosition(EOrthoDirection.Vertical) );
					break;
				}
			}
			scrollablePanelView.cachedRT.anchoredPosition = anchoredPos;
			scrollablePanelView.InitContent( this);
			overlaysPanel.sizeDelta = verticalOverlaysPanel.sizeDelta = horizontalOverlaysPanel.sizeDelta = scrollablePanelView.contentPanelRT.sizeDelta;
			SetUpCancelGrabButton( );
        }

		public void Reset()
		{
			scrollablePanelView.Reset( );
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
						foreach (KeyValuePair<EOrthoDirection, ScrollableScrollBarPanel> kvp in _scrollBars)
						{
							sb.Append( "\n-" ).Append( kvp.Key ).Append( " " ).Append( kvp.Value.cachedRT.rect );
						}
					}
					cancelGrabButtonRT.sizeDelta =
						new Vector2(
							_scrollBars[EOrthoDirection.Vertical].cachedRT.rect.height,
							_scrollBars[EOrthoDirection.Horizontal].cachedRT.rect.height );
					cancelGrabButtonHiderRT.sizeDelta = cancelGrabButtonRT.sizeDelta;

					Vector2 anchorMin = Vector2.zero;
					Vector2 anchorMax = Vector2.zero;
					Vector2 pivot = Vector2.zero;

					if (_scrollBars[EOrthoDirection.Horizontal].ePosition== ELowHigh.High)
					{
						anchorMin.y = 1f;
						anchorMax.y = 1f;
						pivot.y = 1f;
					}
					else if (_scrollBars[EOrthoDirection.Horizontal].ePosition == ELowHigh.Low)
					{
						anchorMin.y = 0f;
						anchorMax.y = 0f;
						pivot.y = 0f;
					}
					else
					{
						Debug.LogError( "Hor posn = " + _scrollBars[EOrthoDirection.Horizontal].ePosition );
                    }

					if (_scrollBars[EOrthoDirection.Vertical].ePosition == ELowHigh.High)
					{
						anchorMin.x = 1f;
						anchorMax.x = 1f;
						pivot.x = 1f;
					}
					else if (_scrollBars[EOrthoDirection.Vertical].ePosition == ELowHigh.Low)
					{
						anchorMin.x = 0f;
						anchorMax.x = 0f;
						pivot.x = 0f;
					}
					else
					{
						Debug.LogError( "Vert posn = " + _scrollBars[EOrthoDirection.Vertical].ePosition );
					}
					cancelGrabButtonRT.anchorMin = anchorMin;
					cancelGrabButtonRT.anchorMax = anchorMax;
					cancelGrabButtonRT.pivot = pivot;
					cancelGrabButtonRT.anchoredPosition = Vector2.zero;

					cancelGrabButtonHiderRT.anchorMin = anchorMin;
					cancelGrabButtonHiderRT.anchorMax = anchorMax;
					cancelGrabButtonHiderRT.pivot = pivot;
					cancelGrabButtonHiderRT.anchoredPosition = Vector2.zero;

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

		public void HandleClick()
		{
			if (ObjectGrabManager.Instance.HasGrabbed)
			{
				if (ObjectGrabManager.Instance.IsGrabbed(grabber))
				{
					ObjectGrabManager.Instance.CancelCurrentGrab( );
				}
			}
			else
			{
				ObjectGrabManager.Instance.HandleGrabRequest( grabber );
			}
		}

		public void HandleGrabberMoved(Vector2 delta)
		{
			_scrollBars[EOrthoDirection.Vertical].scrollBar.HandleMiddleMoved( delta.y );
			_scrollBars[EOrthoDirection.Horizontal].scrollBar.HandleMiddleMoved( delta.x );
			ActivateGrabberArrows(true );
		}

		private void ActivateGrabberArrows(bool b)
		{
			grabberDown.SetActive( b );
			grabberUp.SetActive( b );
			grabberLeft.SetActive( b );
			grabberRight.SetActive( b );
			if (b)
			{
				DeactivateUnusableGrabberArrows( );
			}
		}

		public void HandleGrabberActivated(bool b)
		{
			ActivateGrabberArrows( b );
		}

		private void DeactivateUnusableGrabberArrows()
		{
			if (!_scrollBars[EOrthoDirection.Vertical].scrollBar.CanMoveUp())
			{
				grabberUp.SetActive( false );
			}
			if (!_scrollBars[EOrthoDirection.Vertical].scrollBar.CanMoveDown( ))
			{
				grabberDown.SetActive( false );
			}
			if (!_scrollBars[EOrthoDirection.Horizontal].scrollBar.CanMoveUp( ))
			{
				grabberRight.SetActive( false );
			}
			if (!_scrollBars[EOrthoDirection.Horizontal].scrollBar.CanMoveDown( ))
			{
				grabberLeft.SetActive( false );
			}
		}

		private bool AnyActiveGrabberArrows()
		{
			return (grabberUp.activeSelf || grabberDown.activeSelf || grabberLeft.activeSelf || grabberRight.activeSelf);
		}
	}

}
