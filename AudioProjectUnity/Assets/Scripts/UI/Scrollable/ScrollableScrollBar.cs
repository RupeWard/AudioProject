using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RJWS.Core.Extensions;

namespace RJWS.UI.Scrollable
{
	public class ScrollableScrollBar : MonoBehaviour
	{
		public static readonly bool DEBUG_SCROLLBAR = true;
		public static readonly bool DEBUG_MOVE = false;

		private Dictionary<ELowHigh, ScrollableScrollBarEnd> _ends = new Dictionary<ELowHigh, ScrollableScrollBarEnd>( );
		private ScrollableScrollBarMiddle _middle;

		public System.Action<EOrthoDirection, float, float> onScrollBarChanged; // size, pos as fractions

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

		public void Init( ScrollableScrollBarPanel sbp)
		{
			scrollBarPanel = sbp;
			GameObject endPrefab = Resources.Load<GameObject>( "Prefabs/UI/ScrollBarEnd" );

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
				float highOffset = (0.5f * _sizeRange.y + cachedRT.anchoredPosition.x + 0.5f * newSize) - _sizeRange.y;
				float lowOffset = 0.5f * newSize - (0.5f * _sizeRange.y + cachedRT.anchoredPosition.x);
                if (highOffset <= 0 &&  lowOffset <= 0f)
				{
					cachedRT.sizeDelta = new Vector2( newSize, cachedRT.sizeDelta.y );
					DoScrollBarChangedAction( );
					didChange = true;
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
					cachedRT.sizeDelta = new Vector2( newSize, cachedRT.sizeDelta.y );
					DoScrollBarChangedAction( );
					didChange = true;
				}
			}

			return didChange;
		}

		public void HandleMiddleMoved( float delta )
		{
			if (DEBUG_MOVE)
			{
				Debug.Log( "SB Middle moved: " + delta );
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

		public void HandleEndMoved( ELowHigh lowHigh, float delta, bool doubleEnded)
		{
			if (DEBUG_MOVE)
			{
				Debug.Log( Time.time + " SB End moved: " + lowHigh + " " + delta );
			}

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

			bool canChange = false;

			if (size.x >= _sizeRange.x && size.x <= _sizeRange.y)
			{
				float highOffset = (0.5f * _sizeRange.y + anchoredPos.x + 0.5f * size.x) - _sizeRange.y;
				float lowOffset = 0.5f * size.x - (0.5f * _sizeRange.y + anchoredPos.x);
				if (highOffset <= 0 && lowOffset <= 0f)
				{
					canChange = true;
				}
				else if (scrollBarPanel.settings.allowPositionChangeOnInternalZoom)
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
				}
			}

			if (canChange && scrollBarPanel.scrollablePanel.settings.scrollSettings.linkedScaling)
			{
				canChange = scrollBarPanel.scrollablePanel.GetScrollBar( scrollBarPanel.eDirection.OrthogonalDirection( ) ).scrollBar.SetSizeFraction( size.x/_sizeRange.y );
			}

			if (canChange)
			{
				cachedRT.sizeDelta = size;
				cachedRT.anchoredPosition = anchoredPos;
				DoScrollBarChangedAction( );
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
	}

}
