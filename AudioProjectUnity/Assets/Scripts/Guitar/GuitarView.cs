using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Audio
{
	public class GuitarView : MonoBehaviour
	{
		private static readonly bool DEBUG_LOCAL = true;
		public bool debugMe = true;
		public bool DebugMe
		{
			get { return debugMe || DEBUG_LOCAL; }
		}

		public bool debugHit = true;

		public GuitarStringView stringViewPrefab;
		public GuitarFretView fretViewPrefab;

		public float stringSeparation = 1f;
		public float stringLength = 30f;
		public float stringWidth = 0.1f;
		public int numFrets = 10;
		public float fretWidth = 0.1f;
		public float fretDepth = 0.15f;

		public EPluckerType pluckerType;

		public GuitarSettings guitarSettings;
		public PluckSettings pluckSettings;

		public float FretLength
		{
			get { return guitarModel.NumStrings * stringSeparation; }
		}
		
		public float BridgeX
		{
			get { return -0.5f * stringLength; }
		}

		public float FretX(int fretNum)
		{
			float result = stringLength;
			if (fretNum > 0)
			{
				result /= Mathf.Pow( Core.Audio.AudioConsts.FRET_FACTOR, fretNum );
			}
			result = stringLength - result;
			result = BridgeX + result;
			return result;
		}

		public int GetFretForWorldX( float x, ref float fraction)
		{
			if (DebugMe)
			{
//				Debug.LogWarningFormat( "GetFretForWorldX : {0} - {1} = {2}", x, cachedTransform.position.x, x - cachedTransform.position.x );
			}
			x -= cachedTransform.position.x;

			int result = 0;

			while (result < _fretViews.Count && result < _fretViews.Count && _fretViews[result].cachedTransform.position.x < x)
			{
				result++;
			}
			if (result >= _fretViews.Count)
			{
				result = int.MaxValue;
				fraction = 0f;
			}
			else if (result > 0)
			{
				fraction = (x - _fretViews[result-1].cachedTransform.position.x) / (_fretViews[result].cachedTransform.position.x - _fretViews[result - 1].cachedTransform.position.x);
			}
			return result;
		}

		private List<GuitarStringView> _stringViews = new List<GuitarStringView>( );
		private List<GuitarFretView> _fretViews = new List<GuitarFretView>( );

		public Transform cachedTransform
		{
			get;
			private set;
		}

		public GuitarModel guitarModel
		{
			get;
			private set;
		}

		private void Awake()
		{
			cachedTransform = transform;
			GuitarSettings.LoadIfNUll( ref guitarSettings );
			PluckSettings.LoadIfNUll( ref pluckSettings );
		}

		public void Init(GuitarModel model)
		{
			guitarModel = model;

			if (_stringViews.Count > 0)
			{
				for (int i = 0; i < _stringViews.Count; i++)
				{
					GameObject.Destroy( _stringViews[i].gameObject );
				}
				_stringViews.Clear( );
			}
			for (int i = 0; i < guitarModel.NumStrings; i++)
			{
				GuitarStringView gsv = Instantiate( stringViewPrefab );
				gsv.cachedTransform.SetParent( cachedTransform );
				gsv.gameObject.name = "String_" + i;
				gsv.cachedTransform.localPosition = new Vector3( 0f, i * stringSeparation, 0f );

				gsv.Init( this, model, i );

				_stringViews.Add( gsv );
			}

			if (_fretViews.Count > 0)
			{
				for (int i = 0; i < _fretViews.Count; i++)
				{
					GameObject.Destroy( _fretViews[i].gameObject );
				}
				_fretViews.Clear( );
			}

			for (int i = 0; i <= numFrets; i++)
			{
				GuitarFretView gfv = Instantiate( fretViewPrefab );
				gfv.cachedTransform.SetParent( cachedTransform );
				gfv.gameObject.name = "Fret_" + i;

				gfv.Init( this, i );

				_fretViews.Add( gfv );
			}

			ApplySettings( guitarSettings);
		}

		public void ApplySettings(GuitarSettings s)
		{
			if (DebugMe)
			{
				Debug.LogFormat( "Applying guitar settings to {0} strings", _stringViews.Count );
			}
			for (int i = 0; i < _stringViews.Count; i++)
			{
				_stringViews[i].ApplyGuitarSettings( s );
			}
		}

		public Vector3 StringDims
		{
			get
			{
				return new Vector3( stringWidth, 0.5f * stringLength, stringWidth);
			}
		}

		public void PlayChord()
		{
			for (int i = 0; i < _stringViews.Count; i++)
			{
				_stringViews[i].stringBehaviour.Pluck( 1f );
			}
		}
	}

}
