using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RJWS.Core.Extensions;
using RJWS.Enums;
using RJWS.Core.DebugDescribable;

namespace RJWS.UI.Scrollable
{
	public class ScrollableScrollBar : MonoBehaviour, IDebugDescribable
	{
		public static readonly bool DEBUG_SCROLLBAR = false;
		public static readonly bool DEBUG_MOVE = false;
		public static readonly bool DEBUG_ZOOM = false;


		private Dictionary<ELowHigh, ScrollableScrollBarEnd> _ends = new Dictionary<ELowHigh, ScrollableScrollBarEnd>( );
		private ScrollableScrollBarMiddle _middle;

		public System.Action<EOrthoDirection, float, float> onScrollBarChanged; // size, pos as fractions

		public ScrollableScrollBar parentScrollBar = null;
		public ScrollableScrollBar zoomedScrollBar = null;

		public int zoomLevel
		{
			get;
			private set;
		}

		public RectTransform cachedRT
		{
			private set;
			get;
		}

		public ScrollableScrollBarPanel scrollBarPanel
		{
			get;
			private set;
		}

		private void Awake()
		{
			cachedRT = GetComponent<RectTransform>( );
		}

		private Vector2 _sizeRange;

		public void InitZoomed( ScrollableScrollBar p )
		{
			parentScrollBar = p;
			zoomLevel = parentScrollBar.zoomLevel + 1;
			gameObject.name = parentScrollBar.gameObject.name + "_Z"+zoomLevel;
			cachedRT.SetParent( parentScrollBar.cachedRT.parent );
			cachedRT.anchoredPosition = Vector2.zero;
			cachedRT.localRotation = Quaternion.identity;
			onScrollBarChanged += parentScrollBar.HandleZoomedScrollBarChanged;
			
			ScrollableScrollBarEnd[] ends = transform.GetComponentsInChildren<ScrollableScrollBarEnd>( );
			if (ends != null && ends.Length > 0)
			{
				for (int i = 0; i < ends.Length; i++)
				{
					GameObject.Destroy( ends[i].gameObject );
				}
			}
			ScrollableScrollBarMiddle[] middles = transform.GetComponentsInChildren<ScrollableScrollBarMiddle>( );
			if (middles != null && middles.Length > 0)
			{
				for (int i = 0; i < middles.Length; i++)
				{
					GameObject.Destroy( middles[i].gameObject );
				}
			}
			Init( parentScrollBar.scrollBarPanel, zoomLevel );
		}

		public UnityEngine.UI.Image fillImage;

		public void Init( ScrollableScrollBarPanel sbp, int zoom = 0)
		{
			zoomLevel = zoom;
			scrollBarPanel = sbp;
			GameObject endPrefab = Resources.Load<GameObject>( "Prefabs/UI/ScrollBarEnd" );

			limitState = ELimitState.Upper;

			Vector2 size = scrollBarPanel.cachedRT.sizeDelta;
			size.y -= 10f;
			cachedRT.sizeDelta = size;

			_sizeRange.y = cachedRT.sizeDelta.x;
			
			foreach (ELowHigh eend in System.Enum.GetValues( typeof( ELowHigh ) ))
			{
				if (eend != ELowHigh.None)
				{
					GameObject go = GameObject.Instantiate( endPrefab );
					_ends[eend] = go.GetComponent<ScrollableScrollBarEnd>( );
					_ends[eend].Init( this, eend );
					_sizeRange.x += _ends[eend].cachedRT.sizeDelta.x;
				}
			}

			_ends[ELowHigh.Low].otherEnd = _ends[ELowHigh.High];
			_ends[ELowHigh.High].otherEnd = _ends[ELowHigh.Low];

			GameObject middlePrefab = Resources.Load<GameObject>( "Prefabs/UI/ScrollBarMiddle" );
			GameObject mgo = GameObject.Instantiate( middlePrefab );
			_middle = mgo.GetComponent<ScrollableScrollBarMiddle>( );
			_middle.Init( this );
			_sizeRange.x += _middle.cachedRT.sizeDelta.x;

        }


		public bool SetSizeFraction(float fraction)
		{
			bool didChange = false;

			if (fraction < 0f || fraction > 1f)
			{
				throw new System.Exception( "Size Fraction out of range: " + fraction );
			}
			float newSize = fraction * _sizeRange.y;

			if (newSize >= _sizeRange.x && newSize <= _sizeRange.y)
			{
				limitState = ELimitState.None;
				float highOffset = (0.5f * _sizeRange.y + cachedRT.anchoredPosition.x + 0.5f * newSize) - _sizeRange.y;
				float lowOffset = 0.5f * newSize - (0.5f * _sizeRange.y + cachedRT.anchoredPosition.x);
                if (highOffset <= 0 &&  lowOffset <= 0f)
				{
					didChange = (newSize != cachedRT.sizeDelta.x);
					cachedRT.sizeDelta = new Vector2( newSize, cachedRT.sizeDelta.y );
					DoScrollBarChangedAction( );
				}
				else if (scrollBarPanel.settings.allowPositionChangeOnExternalZoom)
				{
					if (highOffset > 0)
					{
						cachedRT.anchoredPosition = new Vector2( cachedRT.anchoredPosition.x - highOffset, cachedRT.anchoredPosition.y );
					}
					else
					{
						cachedRT.anchoredPosition = new Vector2( cachedRT.anchoredPosition.x + lowOffset, cachedRT.anchoredPosition.y );
					}
					didChange = (newSize != cachedRT.sizeDelta.y);	
					cachedRT.sizeDelta = new Vector2( newSize, cachedRT.sizeDelta.y );
					DoScrollBarChangedAction( );
				}
			}
			else
			{
				if (newSize < _sizeRange.x)
				{
					limitState = ELimitState.Lower;
				}
				if (newSize > _sizeRange.y)
				{
					limitState = ELimitState.Upper;
				}
			}
			if (DEBUG_SCROLLBAR)
			{
				Debug.Log( "Scrollbar "+gameObject.name+" SetSizeFraction(" + fraction + ") limit = " + limitState );
			}
			return didChange;
		}

		public enum ELimitState
		{
			None,
			Lower,
			Upper
		}

		public ELimitState limitState
		{
			get;
			private set;
		}

		public void HandleMiddleMoved( float delta )
		{
			if (DEBUG_MOVE)
			{
				Debug.Log( "SB "+gameObject.name + " Middle moved: " + delta );
			}

			Vector2 size = cachedRT.sizeDelta;
			Vector2 anchoredPos = cachedRT.anchoredPosition;

			anchoredPos.x += delta;

			if (0.5f * _sizeRange.y + anchoredPos.x + 0.5f * size.x <= _sizeRange.y && 0.5f * _sizeRange.y + anchoredPos.x - 0.5f * size.x >= 0f)
			{
				cachedRT.anchoredPosition = anchoredPos;

				DoScrollBarChangedAction( );
			}
		}

		public bool CanMoveUp()
		{
			return SpaceAtEnd( ELowHigh.High ) > 0.5f;
		}

		public bool CanMoveDown( )
		{
			return SpaceAtEnd( ELowHigh.Low) > 0.5f;
		}

		System.Text.StringBuilder _debugSB = new System.Text.StringBuilder( );

		public void HandleEndMoved( ELowHigh lowHigh, float delta, bool doubleEnded)
		{
			// XXXX TODO if zoomed, call on zoomed

			if (DEBUG_MOVE)
			{
				_debugSB.Length = 0;
				_debugSB.Append( gameObject.name + " " +Time.time + " SB End moved: " + lowHigh + " " + delta );
			}

			bool canChange = false;

			if (delta != 0f)
			{
				Vector2 size = cachedRT.sizeDelta;
				Vector2 anchoredPos = cachedRT.anchoredPosition;

				if (doubleEnded)
				{
					switch (lowHigh)
					{
						case ELowHigh.Low:
							{
								size.x -= 2f * delta;
								break;
							}
						case ELowHigh.High:
							{
								size.x += 2f * delta;
								break;
							}
						default:
							{
								Debug.LogError( "Bad ELowHigh: " + lowHigh );
								break;
							}
					}
				}
				else
				{
					switch (lowHigh)
					{
						case ELowHigh.Low:
							{
								size.x -= delta;
								anchoredPos.x += 0.5f * delta;
								break;
							}
						case ELowHigh.High:
							{
								size.x += delta;
								anchoredPos.x += 0.5f * delta;
								break;
							}
						default:
							{
								Debug.LogError( "Bad ELowHigh: " + lowHigh );
								break;
							}
					}

				}

				if (size.x >= _sizeRange.x && size.x <= _sizeRange.y)
				{
					float highOffset = (0.5f * _sizeRange.y + anchoredPos.x + 0.5f * size.x) - _sizeRange.y;
					float lowOffset = 0.5f * size.x - (0.5f * _sizeRange.y + anchoredPos.x);
					if (highOffset <= 0 && lowOffset <= 0f)
					{
						canChange = true;
						limitState = ELimitState.None;
					}
					else
					{
						if (scrollBarPanel.settings.allowPositionChangeOnInternalZoom)
						{
							if (lowOffset > 0f)
							{
								anchoredPos.x = anchoredPos.x + lowOffset;
							}
							else
							{
								anchoredPos.x = anchoredPos.x - highOffset;
							}
							canChange = true;
							limitState = ELimitState.None;
						}
						else
						{
							if (highOffset > 0)
							{
								limitState = ELimitState.Upper;
							}
							else
							{
								limitState = ELimitState.Lower;
							}
						}
					}

				}
				else
				{
					if (size.x <= _sizeRange.x)
					{
						limitState = ELimitState.Lower;
					}
					if (size.x >= _sizeRange.y)
					{
						limitState = ELimitState.Upper;
					}
				}

				if (canChange && scrollBarPanel.scrollablePanel.settings.scrollSettings.linkedScaling)
				{
					canChange = scrollBarPanel.scrollablePanel.GetScrollBar( scrollBarPanel.eDirection.OrthogonalDirection( ) ).scrollBar.SetSizeFraction( size.x / _sizeRange.y );
				}

				if (canChange)
				{
					cachedRT.sizeDelta = size;
					cachedRT.anchoredPosition = anchoredPos;
					DoScrollBarChangedAction( );
					limitState = ELimitState.None;
				}
				else
				{
					if (size.x < _sizeRange.MidPoint( ))
					{
						limitState = ELimitState.Lower;
						CreateZoomed( );
					}
					else
					{
						limitState = ELimitState.Upper;
					}
				}

			}
			else //if (delta != 0f)
			{
				if (DEBUG_MOVE)
				{
					_debugSB.Append( " delta is zero! " );

				}
			}
			if (DEBUG_MOVE)
			{
				_debugSB.Append("\nScrollbar HandleEndMoved(" + lowHigh + ", "+delta+" ) limit = " + limitState+" canChange = "+canChange);
				if (limitState == ELimitState.None)
				{
					Debug.Log( _debugSB.ToString());
				}
				else
				{
					Debug.LogWarning( _debugSB.ToString( ) );
				}
			}

		}

		public bool IsAtRangeEnd( ELowHigh eLowHigh )
		{
			bool result = false;
			if ((eLowHigh == ELowHigh.Low || eLowHigh == ELowHigh.None) && (cachedRT.sizeDelta.x < _sizeRange.x + 0.2f))
			{
				result = true;
			}
			if ((eLowHigh == ELowHigh.High || eLowHigh == ELowHigh.None) && (cachedRT.sizeDelta.x >  _sizeRange.y- 0.2f ))
			{
				result = true;
			}
			return result;
		}

		public float SpaceAtEnd(ELowHigh eLowHigh)
		{
			switch (eLowHigh)
			{
				case ELowHigh.Low:
					{
						return 0.5f * _sizeRange.y + cachedRT.anchoredPosition.x - 0.5f * cachedRT.rect.width;
					}
				case ELowHigh.High:
					{
						return 0.5f * _sizeRange.y - (cachedRT.anchoredPosition.x + 0.5f * cachedRT.rect.width);  
					}
				default:
					{
						Debug.LogError( "Bad LowHigh: " + eLowHigh );
						return 0f;
					}
			}
		}

		private void DoZoomedScrollbarChangedAction(float zoomedSizeFraction, float zoomedPosFraction)
		{
			if (onScrollBarChanged != null)
			{
				float mySizeFraction = cachedRT.sizeDelta.x / _sizeRange.y;
				float sizeFraction = zoomedSizeFraction * mySizeFraction;
				float myPosFraction = (cachedRT.anchoredPosition.x + 0.5f * _sizeRange.y) / _sizeRange.y;

				float posFraction = Mathf.Lerp( myPosFraction - mySizeFraction * 0.5f, myPosFraction + mySizeFraction * 0.5f, zoomedPosFraction );

				if (sizeFraction > 1f || sizeFraction < 0f)
				{
					Debug.LogError( "Bad sizeFraction: " + sizeFraction );
					sizeFraction = Mathf.Clamp01( sizeFraction );
				}
				if (posFraction > 1f || posFraction < 0f)
				{
					Debug.LogError( "Bad posFraction: " + posFraction );
					posFraction = Mathf.Clamp01( posFraction );
				}
				onScrollBarChanged( scrollBarPanel.eDirection, sizeFraction, posFraction );
			}
			else
			{
				Debug.LogError( "No onScrollBarChanged!" );
			}

		}

		private void DoScrollBarChangedAction()
		{
			if (onScrollBarChanged != null)
			{
				float sizeFraction = cachedRT.sizeDelta.x / _sizeRange.y;
				float posFraction = (cachedRT.anchoredPosition.x + 0.5f * _sizeRange.y) / _sizeRange.y;

				if (sizeFraction > 1f || sizeFraction < 0f)
				{
					Debug.LogError( "Bad sizeFraction: " + sizeFraction );
					sizeFraction = Mathf.Clamp01( sizeFraction );
				}
				if (posFraction > 1f || posFraction < 0f)
				{
					Debug.LogError( "Bad posFraction: " + posFraction );
					posFraction = Mathf.Clamp01( posFraction );
				}
				onScrollBarChanged( scrollBarPanel.eDirection, sizeFraction, posFraction );
            }
			else
			{
				Debug.LogError( "No onScrollBarChanged!" );
			}
		}

		private void CreateZoomed()
		{
			if (zoomedScrollBar != null)
			{
				Debug.LogError( "Request to create second zoom" );
				return;
			}
			if (DEBUG_ZOOM)
			{
				Debug.Log( "CreateZoomed " + this.DebugDescribe( ) );
			}
			zoomedScrollBar = GameObject.Instantiate( this.gameObject ).GetComponent<ScrollableScrollBar>( );
			zoomedScrollBar.InitZoomed( this );
			ObjectGrabManager.Instance.CancelCurrentGrab( );
			gameObject.SetActive( false );
		}

		public void DestroyZoomed()
		{
			if (zoomedScrollBar == null)
			{
				Debug.LogError( "Can;t destroy numm zoomed" );
				return;
			}
			if (DEBUG_ZOOM)
			{
				Debug.Log( gameObject.name + " DestroyZoomed " + this.DebugDescribe( )+" \nzoomed = "+zoomedScrollBar.DebugDescribe() );
			}
			GameObject.Destroy( zoomedScrollBar.gameObject );
			zoomedScrollBar = null;
			ObjectGrabManager.Instance.CancelCurrentGrab( );
			gameObject.SetActive( true );
		}

		private void HandleZoomedScrollBarChanged(EOrthoDirection dirn, float sizeFraction, float posFraction)
		{
			if (DEBUG_ZOOM)
			{
				Debug.Log( gameObject.name + " Handle Zoomed changed " + dirn + " SF=" + sizeFraction + " PF=" + posFraction + " zoomLevel (receiver) = " + zoomLevel );
			}
			DoZoomedScrollbarChangedAction( sizeFraction, posFraction );
		}

		public void DebugDescribe(System.Text.StringBuilder sb)
		{
			sb.Append( "[SBar: " ).Append(gameObject.name).Append(" D=").Append( scrollBarPanel.eDirection );
			sb.Append( " Z=" ).Append( zoomLevel );
			sb.Append( "]" );
		}

	}

}
